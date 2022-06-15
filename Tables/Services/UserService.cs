using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Tables.Data;

namespace Tables.Services
{
    public class UserService : IUserService
    {

        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public UserService(DataContext context, IConfiguration config)
        {
            this._context = context;
            this._config = config;

            if (this._context.Users.ToList().Count == 0)
            {
                generateMockData();
            }
        }

        public UserModel Authenticate(LoginDTO userLogin)
        {

            var currentUser = _context.Users.Where(
                u => u.Username.ToLower() == userLogin.Username.ToLower() &&
                u.Password == userLogin.Password
            ).FirstOrDefault();

            return currentUser;
        }

        public string GenerateToken(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void generateMockData()
        {
            _context.Users.Add(new UserModel() { Username = "Admin", Password = "Admin123", Role = "Administrator" });
            _context.Users.Add(new UserModel() { Username = "dominik", Password = "dominik", Role = "Employee" });
            _context.SaveChanges();
        }

        public UserModel GetCurrentUser(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserModel
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

        public async Task<ResponseModel> BookDesk(ReservationDTO reservationDTO, ClaimsIdentity identity)
        {
            ResponseModel res = new ResponseModel();
            var currentUser = GetCurrentUser(identity);
            currentUser = _context.Users.Where(u => u.Username == currentUser.Username).Select(r => r).FirstOrDefault();
            var currentReservation = _context.Reservations
                                            .Where(r => r.User.Username == currentUser.Username)
                                            .Select(r => r).FirstOrDefault();
            if (currentReservation != null)
            {
                _context.Reservations.Remove(currentReservation);
            }

            if ((currentReservation.DateFrom - DateTime.Now).TotalHours < 24)
            {
                res.IsSuccess = false;
                res.Messsage = "Can't change reservation less than 24h before";
                return res;
            }

            var reservationDesk = _context.Desks.Find(reservationDTO.deskId);
            if (reservationDesk == null)
            {
                res.IsSuccess = false;
                res.Messsage = "No such desk";
                return res;
            }

            var dayDiff = (reservationDTO.DateTo - reservationDTO.DateFrom).TotalDays;
            if (dayDiff > 7)
            {
                res.IsSuccess = false;
                res.Messsage = "Can't book desk for more than a week";
                return res;
            }

            Reservation reservation = new Reservation()
            {
                DateFrom = reservationDTO.DateFrom,
                DateTo = reservationDTO.DateTo,
                Desk = reservationDesk,
                User = currentUser,
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return res;
        }
    }
}
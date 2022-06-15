using System.Security.Claims;
using Tables.Data;

namespace Tables.Services
{
    public interface IUserService
    {
        public string GenerateToken(UserModel user);
        public UserModel Authenticate(LoginDTO userLogin);
        public UserModel GetCurrentUser(ClaimsIdentity identity);
        public Task<ResponseModel> BookDesk(ReservationDTO reservationDTO, ClaimsIdentity identity);


    }
}
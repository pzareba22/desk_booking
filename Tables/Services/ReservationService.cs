using Tables.Data;

namespace Tables.Services
{
    public class ReservationService : IReservationService
    {
        private readonly DataContext _context;
        public ReservationService(DataContext context)
        {
            this._context = context;
        }


        public async Task<ResponseModel> DeleteReservation(int id)
        {
            ResponseModel res = new ResponseModel();
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                res.IsSuccess = false;
                res.Messsage = "No such reservation";
                return res;
            }
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return res;
        }

        public async Task<List<Reservation>> GetReservations()
        {
            var reservations = await _context.Reservations.Include("User").Include("Desk").ToListAsync();
            return reservations;
        }
    }
}
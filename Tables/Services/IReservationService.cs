
using Tables.Data;

namespace Tables.Services
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetReservations();
        Task<ResponseModel> DeleteReservation(int id);
    }
}
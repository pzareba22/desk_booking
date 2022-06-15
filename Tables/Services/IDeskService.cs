using Tables.Data;

namespace Tables.Services
{
    public interface IDeskService
    {
        Task<List<Desk>> GetDesks();

        Task<List<Desk>> GetDesksByLocation(string locationName);

        Task<Desk> GetDeskById(int id);

        Task<ResponseModel> UpdateDesk(Desk desk);

        Task<ResponseModel> DeleteDesk(int id);

        Task<ResponseModel> AddDesk(Desk desk);

        Task<ResponseModel> ChangeDeskAvailiability(int id, bool availiability);
    }
}

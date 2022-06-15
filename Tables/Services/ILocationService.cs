using Tables.Data;

namespace Tables.Services
{
    public interface ILocationService
    {
        Task<List<Location>> GetLocations();

        Task<Location> GetLocationById(int id);

        Task<ResponseModel> UpdateLocation(Location location);

        Task<ResponseModel> DeleteLocation(int id);

        Task<ResponseModel> AddLocation(Location location);

        Task<Location> GetLocationByName(string locationName);
    }
}
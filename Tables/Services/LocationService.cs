using Tables.Data;

namespace Tables.Services
{
    public class LocationService : ILocationService
    {
        private readonly DataContext _context;

        public LocationService(DataContext context)
        {
            this._context = context;
        }

        public async Task<ResponseModel> AddLocation(Location location)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                location.Id = 0;
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                res.IsSuccess = false;
                res.Messsage = "Error adding location to the database";
            }

            return res;
        }

        public async Task<ResponseModel> DeleteLocation(int id)
        {

            ResponseModel res = new ResponseModel();
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                res.IsSuccess = false;
                res.Messsage = "No such Location in the database";
            }
            else
            {
                _context.Locations.Remove(location);
                _context.SaveChangesAsync();
            }
            return res;
        }

        public async Task<Location> GetLocationById(int id)
        {
            var location = await _context.Locations.Include("Desks").FirstOrDefaultAsync(l => l.Id == id);
            return location;
        }

        public async Task<Location> GetLocationByName(string locationName)
        {
            var location = _context.Locations
                .Include("Desks")
                .Where(l => l.Name == locationName)
                .Select(l => l);

            return location.FirstOrDefault();
        }

        public async Task<List<Location>> GetLocations()
        {
            var res = await _context.Locations.Include("Desks").ToListAsync();
            return res;
        }

        public async Task<ResponseModel> UpdateLocation(Location location)
        {

            ResponseModel res = new ResponseModel();
            var current_location = await _context.Locations.FindAsync(location.Id);
            if (current_location == null)
            {
                res.IsSuccess = false;
                res.Messsage = "No such location";
            }
            else
            {
                current_location.Name = location.Name;
                await _context.SaveChangesAsync();
            }
            return res;
        }
    }
}
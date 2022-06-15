using Tables.Data;

namespace Tables.Services
{
    public class DeskService : IDeskService
    {

        private readonly DataContext _context;

        public DeskService(DataContext context)
        {
            this._context = context;
        }

        public async Task<ResponseModel> AddDesk(Desk desk)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                desk.Id = 0;
                _context.Desks.Add(desk);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                res.IsSuccess = false;
                res.Messsage = "Error adding desk to the Database";
            }

            return res;
        }

        public async Task<ResponseModel> DeleteDesk(int id)
        {
            ResponseModel res = new ResponseModel();
            var desk = await _context.Desks.FindAsync(id);
            if (desk == null)
            {
                res.IsSuccess = false;
                res.Messsage = "No such Desk in the database";
            }
            else
            {
                _context.Desks.Remove(desk);
                await _context.SaveChangesAsync();
            }
            return res;
        }

        public async Task<Desk> GetDeskById(int id)
        {
            var desk = await _context.Desks.FindAsync(id);
            return desk;
        }

        public async Task<List<Desk>> GetDesks()
        {
            var desks = await _context.Desks.Include("DeskLocation").ToListAsync();
            return desks;
        }

        public async Task<List<Desk>> GetDesksByLocation(string locationName)
        {
            var desks = _context.Desks
                                    .Include("DeskLocation")
                                    .Where(p => p.DeskLocation.Name == locationName)
                                    .Select(p => p);
            return desks.ToList();
        }

        public async Task<ResponseModel> UpdateDesk(Desk desk)
        {
            ResponseModel res = new ResponseModel();
            var current_desk = await _context.Desks.FindAsync(desk.Id);
            if (current_desk == null)
            {
                res.IsSuccess = false;
                res.Messsage = "No such desk";
            }
            else
            {
                current_desk.Capacity = desk.Capacity;
                current_desk.DeskLocation = desk.DeskLocation;
                await _context.SaveChangesAsync();
            }
            return res;
        }

        public async Task<ResponseModel> ChangeDeskAvailiability(int id, bool availiability)
        {
            ResponseModel res = new ResponseModel();
            var desk = await _context.Desks.FindAsync(id);
            if (desk == null)
            {
                res.IsSuccess = false;
                res.Messsage = "No such desk";
                return res;
            }
            desk.IsAvailiable = availiability;
            await _context.SaveChangesAsync();

            return res;
        }
    }
}
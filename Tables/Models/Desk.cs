using System.ComponentModel.DataAnnotations.Schema;

namespace Tables.Data
{
    public class Desk
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailiable { get; set; } = true;

        public Location DeskLocation { get; set; }
    }
}
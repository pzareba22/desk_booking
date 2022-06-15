using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Tables.Data
{

    public class Reservation
    {
        public int Id { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Desk Desk { get; set; }
        public UserModel User { get; set; }
    }

}
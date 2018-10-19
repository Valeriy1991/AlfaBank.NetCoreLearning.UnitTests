using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Database
{
    public class DriverOrder
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; }

        [NotMapped]
        public Driver Driver { get; set; }

        [NotMapped]
        public Order Order { get; set; }
    }
}
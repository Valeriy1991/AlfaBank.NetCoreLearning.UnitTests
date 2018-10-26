using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }

        [NotMapped]
        public ICollection<Order> Orders { get; set; }
    }
}
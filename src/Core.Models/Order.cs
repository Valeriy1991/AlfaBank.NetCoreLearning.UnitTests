using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Comments { get; set; }
        public DateTime When { get; set; }
        public string Status { get; set; }
        public DateTime? FinishDateTime { get; set; }
        
        [NotMapped]
        public ICollection<Driver> Drivers { get; set; }
    }
}
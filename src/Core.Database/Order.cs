using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Database
{
    public class Order
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Comments { get; set; }
        public DateTime When { get; set; }
        
        [NotMapped]
        public ICollection<Driver> Drivers { get; set; }
    }
}
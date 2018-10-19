using System;
using System.Collections.Generic;

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

        public ICollection<Driver> Drivers { get; set; }
    }
}
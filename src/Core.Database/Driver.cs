using System.Collections.Generic;

namespace Core.Database
{
    public class Driver
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
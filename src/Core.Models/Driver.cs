using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    [DataContract]
    public class Driver
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "fullName")]
        public string FullName { get; set; }

        [DataMember(Name = "phone")]
        public string Phone { get; set; }
        
        [DataMember(Name = "orders")]
        [NotMapped]
        public ICollection<Order> Orders { get; set; }

        [IgnoreDataMember] public string SomeIgnoredProperty { get; set; } = "some-ignored-value";
    }
}
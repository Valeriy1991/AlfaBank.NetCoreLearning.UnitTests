using System;
using System.Runtime.Serialization;

namespace Core.Models
{
    /// <summary>
    /// Модель заказа такси
    /// </summary>
    [DataContract]
    public class MakeOrderTaxiModel
    {
        [DataMember(Name = "phone")]
        public string Phone { get; set; }

        [DataMember(Name = "addressFrom")]
        public string From { get; set; }

        [DataMember(Name = "addressTo")]
        public string To { get; set; }

        [DataMember(Name = "comments")]
        public string Comments { get; set; }

        [DataMember(Name = "when")]
        public DateTime When { get; set; }
    }
}

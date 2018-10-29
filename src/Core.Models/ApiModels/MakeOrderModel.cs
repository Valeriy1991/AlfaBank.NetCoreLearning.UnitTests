using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Core.Models.ApiModels
{
    /// <summary>
    /// Модель заказа такси
    /// </summary>
    [DataContract]
    public class MakeOrderModel
    {
        [DataMember(Name = "phone")]
        [Required]
        public string Phone { get; set; }

        [DataMember(Name = "addressFrom")]
        [Required]
        public string From { get; set; }

        [DataMember(Name = "addressTo")]
        [Required]
        public string To { get; set; }

        [DataMember(Name = "comments")]
        public string Comments { get; set; }

        [DataMember(Name = "when")]
        [Required]
        public DateTime When { get; set; }
    }
}

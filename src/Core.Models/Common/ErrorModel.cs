using System.Runtime.Serialization;

namespace Core.Models.Common
{
    [DataContract]
    public class ErrorModel
    {
        [DataMember(Name = "isError")]
        public bool IsError => true;

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
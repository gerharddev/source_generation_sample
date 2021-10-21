using System;
using SampleApi.Attributes;

namespace SampleApi.Models
{
    [Mappable]  //This class is mappable
    public class Part
    {
        [MappableIgnore]    //Will ignore the Id when generating the DTO
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public int NodeTypeId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Car
{
    public class CarDto
    {
        public int UserId { get; set; }
        public string Registration { get; set; }
        public string Vin { get; set; }
        public string Status { get; set; }
        public string Kind { get; set; }
        public string Usage { get; set; }
        public string Category { get; set; }
        public int ModelYear { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Variant { get; set; }
        public string FuelType { get; set; }
        public string ExtraEquipment { get; set; }
    }
}

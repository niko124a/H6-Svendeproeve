using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Common.Entities
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        [MaxLength(7)]
        public string Registration { get; set; }
        [Required]
        public string Vin { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Kind { get; set; }
        [Required]
        public string Usage { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int ModelYear { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Variant { get; set; }
        [Required]
        public string FuelType { get; set; }
        [Required]
        public string ExtraEquipment { get; set; }
    }
}

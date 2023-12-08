using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public ReservationType Type { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public string Note { get; set; }
        [Required]
        public Car Car { get; set; }
        [Required]
        public string Status { get; set; }
    }
}

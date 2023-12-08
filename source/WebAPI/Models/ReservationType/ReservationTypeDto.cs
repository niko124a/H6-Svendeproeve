using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.ReservationType
{
    public class ReservationTypeDto
    {
        public string Name { get; set; }
        public int DurationEstimate { get; set; }
    }
}

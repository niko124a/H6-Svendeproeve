using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Reservation
{
    public class ReservationDto
    {
        public int UserId { get; set; }
        public int ReservationTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public string Note { get; set; }
        public int CarId { get; set; }
        public string Status { get; set; }
    }
}

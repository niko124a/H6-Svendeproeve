using Common.Entities;

namespace Website.Models
{
    public class CreateReservationDto
    {
        public int UserId { get; set; }
        public ReservationType? ReservationType { get; set; }
        public DateTime StartDate { get; set; }
        public string Note { get; set; } = string.Empty;
        public Car Car { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}

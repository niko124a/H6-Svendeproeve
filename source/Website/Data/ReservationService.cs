using Website.Models;

namespace Website.Data
{
    public class ReservationService
    {
        public CreateReservationDto ReservationDto { get; set; }
        public ReservationService()
        {
            ReservationDto = new CreateReservationDto();
            ReservationDto.ReservationType = new();
            ReservationDto.ReservationType.Name = string.Empty;
        }
    }
}

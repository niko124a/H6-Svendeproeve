using Common.Entities;
using DatabaseAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Reservation;

namespace WebAPI.Controllers
{
    [Route("api/reservations")]
    [Authorize]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReservationTypeRepository _reservationTypeRepository;
        private readonly ICarRepository _carRepository;
        public ReservationsController(IReservationRepository reservationRepository, IUserRepository userRepository, IReservationTypeRepository reservationTypeRepository, ICarRepository carRepository)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _reservationTypeRepository = reservationTypeRepository;
            _carRepository = carRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _reservationRepository.GetAllReservationsAsync();
            if (reservations.Count() == 0)
                return NoContent();

            return Ok(reservations);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActiveReservations()
        {
            var reservations = await _reservationRepository.GetAllActiveReservationsAsync();
            if (reservations.Count() == 0)
                return NoContent();

            return Ok(reservations);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAllReservationsByUserId(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            var reservations = await _reservationRepository.GetAllReservationsByUserIdAsync(id);
            if (reservations.Count() == 0)
                return NoContent();

            return Ok(reservations);
        }

        [HttpGet("user/{id}/active")]
        public async Task<IActionResult> GetAllActiveReservationsByUserId(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            var reservations = await _reservationRepository.GetAllActiveReservationsByUserIdAsync(id);
            if (reservations.Count() == 0)
                return NoContent();

            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationDto reservationDto)
        {
            if (reservationDto.UserId < 1)
                return BadRequest("User Id have to be 1 or higher.");

            if (reservationDto.ReservationTypeId < 1)
                return BadRequest("Reservation type Id have to be 1 or higher.");

            var user = await _userRepository.GetUserByIdAsync(reservationDto.UserId);
            if (user == null)
                return NotFound($"No user with id {reservationDto.UserId} was found.");

            var reservationType = await _reservationTypeRepository.GetReservationTypeByIdAsync(reservationDto.ReservationTypeId);
            if (reservationType == null)
                return NotFound($"No reservation type with id {reservationDto.ReservationTypeId} was found.");

            var car = await _carRepository.GetCarByIdAsync(reservationDto.CarId);
            if (car == null)
                return NotFound();

            var reservation = new Reservation();
            reservation.User = user;
            reservation.Type = reservationType;
            reservation.Note = reservationDto.Note;
            reservation.StartDate = reservationDto.StartDate;
            reservation.Car = car;
            reservation.Status = reservationDto.Status;

            _reservationRepository.CreateReservation(reservation);
            await _reservationRepository.SaveChangesAsync();

            return CreatedAtAction("CreateReservation", reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] ReservationDto reservationDto)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            if (reservationDto.UserId < 1)
                return BadRequest("User Id have to be 1 or higher.");

            if (reservationDto.ReservationTypeId < 1)
                return BadRequest("Reservation type Id have to be 1 or higher.");

            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null) 
                return NotFound($"No reservation with id {id} was found.");

            var user = await _userRepository.GetUserByIdAsync(reservationDto.UserId);
            if (user == null) 
                return NotFound($"No user with id {reservationDto.UserId} was found.");

            var reservationType = await _reservationTypeRepository.GetReservationTypeByIdAsync(reservationDto.ReservationTypeId);
            if (reservationType == null)
                return NotFound($"No reservation type with id {reservationDto.ReservationTypeId} was found.");

            reservation.User = user;
            reservation.Type = reservationType;
            reservation.Note = reservationDto.Note;
            reservation.StartDate = reservationDto.StartDate;
            reservation.Note = reservationDto.Note;
            reservation.Status = reservationDto.Status;

            await _reservationRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
                return NotFound($"No reservation with id {id} was found.");

            _reservationRepository.DeleteReservation(reservation);
            await _reservationRepository.SaveChangesAsync();

            return Ok();
        }
    }
}

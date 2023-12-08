using Common.Entities;
using DatabaseAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebAPI.Models.ReservationType;
using WebAPI.Models.Role;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/reservationtypes")]
    [ApiController]
    public class ReservationTypesController : ControllerBase
    {
        private readonly IReservationTypeRepository _reservationTypeRepository;

        public ReservationTypesController(IReservationTypeRepository reservationTypeRepository)
        {
            _reservationTypeRepository = reservationTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservationTypes()
        {
            var reservationTypes = await _reservationTypeRepository.GetAllReservationTypesAsync();
            if (reservationTypes.Count() == 0)
                return NoContent();

            return Ok(reservationTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationTypeById(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var reservationType = await _reservationTypeRepository.GetReservationTypeByIdAsync(id);
            if (reservationType == null) 
                return NotFound();

            return Ok(reservationType);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetReservationTypeByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Invalid name.");

            var reservationType = await _reservationTypeRepository.GetReservationTypeByNameAsync(name);
            if (reservationType == null)
                return NotFound();

            return Ok(reservationType);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservationType([FromBody] ReservationTypeDto reservationTypeDto)
        {
            if (await _reservationTypeRepository.ReservationTypeExistsAsync(reservationTypeDto.Name))
                return BadRequest($"There is already a reservation type with the name {reservationTypeDto.Name}.");

            ReservationType reservationType = new ReservationType();
            reservationType.Name = reservationTypeDto.Name;
            reservationType.DurationEstimate = reservationTypeDto.DurationEstimate;

            _reservationTypeRepository.CreateReservationType(reservationType);
            await _reservationTypeRepository.SaveChangesAsync();

            return CreatedAtAction("CreateReservationType", reservationType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservationType(int id, [FromBody] ReservationTypeDto reservationTypeDto)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            ReservationType? reservationType = await _reservationTypeRepository.GetReservationTypeByIdAsync(id);
            if (reservationType == null) 
                return NotFound();

            reservationType.Name = reservationTypeDto.Name;
            reservationType.DurationEstimate = reservationTypeDto.DurationEstimate;

            await _reservationTypeRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservationType(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            ReservationType? reservationType = await _reservationTypeRepository.GetReservationTypeByIdAsync(id);
            if(reservationType == null)
                return NotFound();

            _reservationTypeRepository.DeleteReservationType(reservationType);
            await _reservationTypeRepository.SaveChangesAsync();

            return Ok();
        }
    }
}

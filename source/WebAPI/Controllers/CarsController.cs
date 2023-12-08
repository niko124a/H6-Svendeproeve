using Common.Entities;
using Common.Helpers;
using DatabaseAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebAPI.Models.Car;

namespace WebAPI.Controllers
{
    [Route("api/cars")]
    [Authorize]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly CarRegistrationHelper _carRegistrationHelper;
        public CarsController(ICarRepository carRepository, IUserRepository userRepository, CarRegistrationHelper carRegistrationHelper)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
            _carRegistrationHelper = carRegistrationHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carRepository.GetAllCarsAsync();
            if (cars.Count() == 0)
                return NoContent();

            return Ok(cars);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetCarsByUserId(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var cars = await _carRepository.GetCarsByUserIdAsync(id);
            if (cars.Count() == 0)
                return NoContent();

            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var car = await _carRepository.GetCarByIdAsync(id);
            if (car == null)
                return NotFound();

            return Ok(car);
        }

        [HttpGet("registration/{registration}")]
        public async Task<IActionResult> GetCarByRegistrationAsync(string registration, [FromQuery] bool isCustom = false)
        {
            if (!isCustom)
            {
                if (!_carRegistrationHelper.ValidateNormalRegistration(registration))
                    return BadRequest("Registration has an invalid format.");
            }
            else
            {
                if (!_carRegistrationHelper.ValidateCustomRegistration(registration))
                    return BadRequest("Invalid registration.");
            }

            var car = await _carRepository.GetCarByRegistrationAsync(registration);
            if (car == null)
                return NotFound();

            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] CarDto carDto, [FromQuery] bool isCustom = false)
        {
            if (carDto.UserId < 1)
                return BadRequest("User Id have to be 1 or higher.");

            if (!isCustom)
            {
                if (!_carRegistrationHelper.ValidateNormalRegistration(carDto.Registration))
                    return BadRequest("Registration has an invalid format.");
            }
            else
            {
                if (!_carRegistrationHelper.ValidateCustomRegistration(carDto.Registration))
                    return BadRequest("Invalid registration.");
            }

            if (await _carRepository.CarExistsAsync(carDto.Registration))
                return BadRequest($"There is already a car with the registration {carDto.Registration}");

            var user = await _userRepository.GetUserByIdAsync(carDto.UserId);
            if (user == null)
                return NotFound("The specified user do not exist.");

            var car = new Car();
            car.User = user;
            car.Registration = carDto.Registration;
            car.Vin = carDto.Vin;
            car.Status = carDto.Status;
            car.Kind = carDto.Kind;
            car.Usage = carDto.Usage;
            car.Category = carDto.Category;
            car.ModelYear = carDto.ModelYear;
            car.Brand = carDto.Brand;
            car.Model = carDto.Model;
            car.Variant = carDto.Variant;
            car.FuelType = carDto.FuelType;
            car.ExtraEquipment = carDto.ExtraEquipment;

            _carRepository.CreateCar(car);
            await _carRepository.SaveChangesAsync();

            return CreatedAtAction("CreateCar", car);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CarDto carDto, [FromQuery] bool isCustom = false)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var car = await _carRepository.GetCarByIdAsync(id);
            if (car == null)
                return NotFound($"No car with the id {id} was found.");

            var user = await _userRepository.GetUserByIdAsync(carDto.UserId);
            if (user == null)
                return NotFound("The specified user do not exist.");

            if (!isCustom)
            {
                if (!_carRegistrationHelper.ValidateNormalRegistration(carDto.Registration))
                    return BadRequest("Registration has an invalid format.");
            }
            else
            {
                if (!_carRegistrationHelper.ValidateCustomRegistration(carDto.Registration))
                    return BadRequest("Invalid registration.");
            }

            car.User = user;
            car.Registration = carDto.Registration;
            car.Vin = carDto.Vin;
            car.Status = carDto.Status;
            car.Kind = carDto.Kind;
            car.Usage = carDto.Usage;
            car.Category = carDto.Category;
            car.ModelYear = carDto.ModelYear;
            car.Brand = carDto.Brand;
            car.Model = carDto.Model;
            car.Variant = carDto.Variant;
            car.FuelType = carDto.FuelType;
            car.ExtraEquipment = carDto.ExtraEquipment;

            await _carRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var car = await _carRepository.GetCarByIdAsync(id);
            if (car == null)
                return NotFound($"No car with the id {id} was found.");

            _carRepository.DeleteCar(car);
            await _carRepository.SaveChangesAsync();

            return Ok();
        }
    }
}

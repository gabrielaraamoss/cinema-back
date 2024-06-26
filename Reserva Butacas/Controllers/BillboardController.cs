using Microsoft.AspNetCore.Mvc;
using Reserva_Butacas.Dtos;
using Reserva_Butacas.Interfaces;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Reserva_Butacas.Exceptions;
using Reserva_Butacas.Models;
using Reserva_Butacas.Services;

namespace Reserva_Butacas.Controllers
{
    [Route("api/billboards")]
    [ApiController]
    public class BillboardController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;
        private readonly IBillboardRepository _billboardRepository;
        private readonly IBookingRepository _bookingRepository;

        public BillboardController(ICinemaService cinemaService, IBillboardRepository billboardRepository, IBookingRepository bookingRepository)
        {
            _cinemaService = cinemaService;
            _billboardRepository = billboardRepository;
            _bookingRepository = bookingRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllBillboards()
        {
            var billboards = await _billboardRepository.GetAll();
            return Ok(billboards);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateBillboard([FromBody] BillboardDTO billboardDto)
        {
            try {
                DateTime dateUtc = DateTime.ParseExact(billboardDto.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
                TimeSpan startTime = TimeSpan.ParseExact(billboardDto.StartTime, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                TimeSpan endTime = TimeSpan.ParseExact(billboardDto.EndTime, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);

                var billboard = new DatabaseEntities.BillboardEntity
                {
                    Date = dateUtc,
                    StartTime = startTime,
                    EndTime = endTime,
                    MovieId = billboardDto.MovieId,
                    RoomId = billboardDto.RoomId,
                    Status = true
                };

                await _billboardRepository.Add(billboard);

                return Ok("Billboard created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillboard(int id)
        {
            var billboard = await _billboardRepository.GetById(id);
            if (billboard == null)
            {
                return NotFound("Billboard not found.");
            }

            return Ok(billboard);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBillboard(int id, [FromBody] BillboardDTO billboardDto)
        {
            var billboard = await _billboardRepository.GetById(id);
            if (billboard == null)
            {
                return NotFound("Billboard not found.");
            }

            try
            {
                billboard.Date = DateTime.ParseExact(billboardDto.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();

                billboard.StartTime = TimeSpan.ParseExact(billboardDto.StartTime, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                billboard.EndTime = TimeSpan.ParseExact(billboardDto.EndTime, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);

                billboard.MovieId = billboardDto.MovieId;
                billboard.RoomId = billboardDto.RoomId;

                await _billboardRepository.Update(billboard);
                return Ok("Billboard updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillboard(int id)
        {
            var billboard = await _billboardRepository.GetById(id);
            if (billboard == null)
            {
                return NotFound("Billboard not found.");
            }

            await _billboardRepository.Delete(id);
            return Ok("Billboard deleted successfully.");
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelBillboardAndBookings([FromBody] BillboardDTO cancellationDTO)
        {
            try
            {
                var result = await _cinemaService.CancelBillboardAndBookings(cancellationDTO);
                if (result)
                {
                    return Ok("Billboard and bookings cancelled successfully.");
                }
                return BadRequest("Failed to cancel billboard and bookings.");
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
        
        
        [HttpGet("reservations")]
        public async Task<IActionResult> GetMovieReservations([FromQuery] DatabaseEntities.MovieGenreEnum genre, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var reservations = await _cinemaService.GetMovieReservations(genre, startDate, endDate);
            return Ok(reservations);
        }

        [HttpGet("seat-status")]
        public async Task<IActionResult> GetSeatStatusByRoom([FromQuery] int roomId)
        {
            try
            {
                var seatStatus = await _cinemaService.GetSeatStatusByRoom(roomId);
                return Ok(seatStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpGet("reservations/terror")]
        public async Task<IActionResult> GetTerrorMovieReservations([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var genre = DatabaseEntities.MovieGenreEnum.THRILLER;
                var reservations = await _cinemaService.GetMovieReservations(genre, startDate, endDate);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpGet("seat-status/current")]
        public async Task<IActionResult> GetCurrentSeatStatusByRoom()
        {
            try
            {
                DateTime currentDate = DateTime.UtcNow.Date; 
                var seatStatus = await _cinemaService.GetSeatStatusByRoomForDate(currentDate);
                return Ok(seatStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

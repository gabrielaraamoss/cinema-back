using Microsoft.AspNetCore.Mvc;
using Reserva_Butacas.Dtos;
using Reserva_Butacas.Interfaces;
using System;
using System.Threading.Tasks;
using Reserva_Butacas.Exceptions;
using Reserva_Butacas.Services;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Controllers
{
    [Route("api/seats")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatRepository _seatRepository;
        private readonly ICinemaService _cinemaService;

        public SeatController(ISeatRepository seatRepository, ICinemaService cinemaService)
        {
            _seatRepository = seatRepository;
            _cinemaService = cinemaService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeat([FromBody] SeatDTO seatDto)
        {
            var seat = new DatabaseEntities.SeatEntity
            {
                Number = seatDto.Number,
                RowNumber = seatDto.RowNumber,
                RoomId = seatDto.RoomId,
                Status = seatDto.Status
            };

            await _seatRepository.Add(seat);
            return Ok("Seat created successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeat(int id)
        {
            var seat = await _seatRepository.GetById(id);
            if (seat == null)
            {
                return NotFound("Seat not found.");
            }

            return Ok(seat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(int id, [FromBody] SeatDTO seatDto)
        {
            var seat = await _seatRepository.GetById(id);
            if (seat == null)
            {
                return NotFound("Seat not found.");
            }

            seat.Number = seatDto.Number;
            seat.RowNumber = seatDto.RowNumber;
            seat.RoomId = seatDto.RoomId;
            seat.Status = seatDto.Status;

            await _seatRepository.Update(seat);
            return Ok("Seat updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var seat = await _seatRepository.GetById(id);
            if (seat == null)
            {
                return NotFound("Seat not found.");
            }

            await _seatRepository.Delete(seat);
            return Ok("Seat deleted successfully.");
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelSeatAndBooking([FromBody] SeatDTO cancellationDTO)
        {
            try
            {
                var result = await _cinemaService.CancelSeatAndBooking(cancellationDTO);
                if (result)
                {
                    return Ok("Seat and booking cancelled successfully.");
                }
                return BadRequest("Failed to cancel seat and booking.");
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
    }
}

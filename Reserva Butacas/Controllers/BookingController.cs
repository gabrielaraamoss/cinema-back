using Microsoft.AspNetCore.Mvc;
using Reserva_Butacas.Dtos;
using Reserva_Butacas.Interfaces;
using Reserva_Butacas.Models;
using System;
using System.Threading.Tasks;

namespace Reserva_Butacas.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDto)
        {
            var booking = new DatabaseEntities.BookingEntity
            {
                Date = bookingDto.Date,
                CustomerId = bookingDto.CustomerId,
                SeatId = bookingDto.SeatId,
                BillboardId = bookingDto.BillboardId
            };

            await _bookingRepository.Add(booking);
            return Ok("Booking created successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            return Ok(booking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingDTO bookingDto)
        {
            var booking = await _bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            booking.Date = bookingDto.Date;
            booking.CustomerId = bookingDto.CustomerId;
            booking.SeatId = bookingDto.SeatId;
            booking.BillboardId = bookingDto.BillboardId;

            await _bookingRepository.Update(booking);
            return Ok("Booking updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            await _bookingRepository.Delete(id);
            return Ok("Booking deleted successfully.");
        }
    }
}

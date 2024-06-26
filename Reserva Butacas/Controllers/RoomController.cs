using Microsoft.AspNetCore.Mvc;
using Reserva_Butacas.Dtos;
using Reserva_Butacas.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ISeatRepository _seatRepository;

        public RoomController(IRoomRepository roomRepository, ISeatRepository seatRepository)
        {
            _roomRepository = roomRepository;
            _seatRepository = seatRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomRepository.GetAllAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound("Room not found.");
            }

            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] RoomDTO roomDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var room = new DatabaseEntities.RoomEntity
            {
                Name = roomDto.Name,
                Number = roomDto.Number,
            };

            await _roomRepository.AddAsync(room);

            return Ok(new { message = "Room created successfully.", roomId = room.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDTO roomDto)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound("Room not found.");
            }

            room.Name = roomDto.Name;
            room.Number = roomDto.Number;

            await _roomRepository.UpdateAsync(room);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound("Room not found.");
            }

            var seats = await _seatRepository.GetByRoomId(id);
            foreach (var seat in seats)
            {
                await _seatRepository.Delete(seat);
            }

            await _roomRepository.DeleteAsync(room);
            return Ok();
        }

        [HttpGet("{roomId}/seats")]
        public async Task<IActionResult> GetSeatsByRoom(int roomId)
        {
            var seats = await _seatRepository.GetByRoomId(roomId);
            return Ok(seats);
        }
    }
}

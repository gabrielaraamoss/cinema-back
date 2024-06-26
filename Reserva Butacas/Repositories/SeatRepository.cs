using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reserva_Butacas.Data;
using Reserva_Butacas.Interfaces;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly Context _context;

        public SeatRepository(Context context)
        {
            _context = context;
        }

        public async Task<DatabaseEntities.SeatEntity> GetById(int id)
        {
            return await _context.Seats.FindAsync(id);
        }

        public async Task<List<DatabaseEntities.SeatEntity>> GetAll()
        {
            return await _context.Seats.ToListAsync();
        }

        public async Task Add(DatabaseEntities.SeatEntity seat)
        {
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();
        }

        public async Task Update(DatabaseEntities.SeatEntity seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(DatabaseEntities.SeatEntity seat)
        {
            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<DatabaseEntities.SeatEntity>> GetByRoomId(int roomId)
        {
            return await _context.Seats.Where(s => s.RoomId == roomId).ToListAsync();
        }
        
        public async Task<int> GetAvailableSeats(int roomId, DateTime date)
        {
            var reservedSeats = await _context.Bookings
                .Where(b => b.Billboard.RoomId == roomId && b.Billboard.Date == date)
                .Select(b => b.SeatId)
                .ToListAsync();

            var totalSeats = await _context.Seats
                .Where(s => s.RoomId == roomId)
                .CountAsync();

            return totalSeats - reservedSeats.Count;
        }

        public async Task<int> GetOccupiedSeats(int roomId, DateTime date)
        {
            var reservedSeats = await _context.Bookings
                .Where(b => b.Billboard.RoomId == roomId && b.Billboard.Date == date)
                .Select(b => b.SeatId)
                .ToListAsync();

            return reservedSeats.Count;
        }
    }
}
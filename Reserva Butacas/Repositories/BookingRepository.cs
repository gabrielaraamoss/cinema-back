using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reserva_Butacas.Data;
using Reserva_Butacas.Interfaces;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly Context _context;

    public BookingRepository(Context context)
    {
        _context = context;
    }

    public async Task<DatabaseEntities.BookingEntity> GetById(int id)
    {
        return await _context.Bookings.FindAsync(id);
    }

    public async Task<List<DatabaseEntities.BookingEntity>> GetByBillboardId(int billboardId)
    {
        return await _context.Bookings
            .Where(b => b.BillboardId == billboardId)
            .ToListAsync();
    }
    
    public async Task<List<DatabaseEntities.BookingEntity>> GetAll()
    {
        return await _context.Bookings.ToListAsync();
    }

    public async Task Add(DatabaseEntities.BookingEntity booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
    }

    public async Task Update(DatabaseEntities.BookingEntity booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<List<DatabaseEntities.BookingEntity>> GetReservationsByGenreAndDateRangeAsync(DatabaseEntities.MovieGenreEnum genre, DateTime startDate, DateTime endDate)
    {
        return await _context.Bookings
            .Include(b => b.Billboard)
            .ThenInclude(bb => bb.Movie)
            .Where(b => b.Billboard.Movie.Genre == genre && b.Billboard.Date >= startDate && b.Billboard.Date <= endDate)
            .ToListAsync();
    }
}
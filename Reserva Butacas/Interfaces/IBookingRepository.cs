using System.Collections.Generic;
using System.Threading.Tasks;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Interfaces;

public interface IBookingRepository
{
    Task<DatabaseEntities.BookingEntity> GetById(int id);
    Task<List<DatabaseEntities.BookingEntity>> GetByBillboardId(int billboardId);
    Task<List<DatabaseEntities.BookingEntity>> GetAll();
    Task Add(DatabaseEntities.BookingEntity booking);
    Task Update(DatabaseEntities.BookingEntity booking);
    Task Delete(int id);
    
    Task<List<DatabaseEntities.BookingEntity>> GetReservationsByGenreAndDateRangeAsync(DatabaseEntities.MovieGenreEnum genre, DateTime startDate, DateTime endDate);
}
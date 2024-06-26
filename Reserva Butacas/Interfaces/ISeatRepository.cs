using System.Collections.Generic;
using System.Threading.Tasks;
using Reserva_Butacas.Models;
namespace Reserva_Butacas.Interfaces;

public interface ISeatRepository
{
    Task<DatabaseEntities.SeatEntity> GetById(int id);
    Task<List<DatabaseEntities.SeatEntity>> GetAll();
    Task Add(DatabaseEntities.SeatEntity seat);
    Task Update(DatabaseEntities.SeatEntity seat);
    Task Delete(DatabaseEntities.SeatEntity seat);
    
    Task<List<DatabaseEntities.SeatEntity>> GetByRoomId(int roomId);
    Task<int> GetAvailableSeats(int roomId, DateTime date);
    Task<int> GetOccupiedSeats(int roomId, DateTime date);
}
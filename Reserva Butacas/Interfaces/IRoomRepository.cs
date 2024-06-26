using System.Collections.Generic;
using System.Threading.Tasks;
using Reserva_Butacas.Models;
namespace Reserva_Butacas.Interfaces;

public interface IRoomRepository
{
    Task<DatabaseEntities.RoomEntity> GetByIdAsync(int id);
    Task<List<DatabaseEntities.RoomEntity>> GetAllAsync();
    Task AddAsync(DatabaseEntities.RoomEntity room);
    Task UpdateAsync(DatabaseEntities.RoomEntity room);
    Task DeleteAsync(DatabaseEntities.RoomEntity room);
}
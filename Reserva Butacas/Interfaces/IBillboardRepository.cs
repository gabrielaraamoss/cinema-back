using System.Collections.Generic;
using System.Threading.Tasks;
using Reserva_Butacas.Models;
namespace Reserva_Butacas.Interfaces;

public interface IBillboardRepository
{
    Task<DatabaseEntities.BillboardEntity> GetById(int id);
    Task<List<DatabaseEntities.BillboardEntity>> GetAll();
    Task Add(DatabaseEntities.BillboardEntity billboard);
    Task Update(DatabaseEntities.BillboardEntity billboard);
    Task Delete(int id);
    Task<List<DatabaseEntities.BillboardEntity>> GetByDate(DateTime date);
}

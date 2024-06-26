using System.Collections.Generic;
using System.Threading.Tasks;
using Reserva_Butacas.Models;
namespace Reserva_Butacas.Interfaces;

public interface IMovieRepository
{
    Task<DatabaseEntities.MovieEntity> GetByIdAsync(int id);
    Task<List<DatabaseEntities.MovieEntity>> GetAllAsync();
    Task AddAsync(DatabaseEntities.MovieEntity movie);
    Task UpdateAsync(DatabaseEntities.MovieEntity movie);
    Task DeleteAsync(DatabaseEntities.MovieEntity movie);
}
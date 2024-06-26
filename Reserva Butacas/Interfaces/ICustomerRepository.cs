using System.Collections.Generic;
using System.Threading.Tasks;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Interfaces;
    public interface ICustomerRepository
    {
        Task<DatabaseEntities.CustomerEntity> GetByIdAsync(int id);
        Task<List<DatabaseEntities.CustomerEntity>> GetAllAsync();
        Task AddAsync(DatabaseEntities.CustomerEntity customer);
        Task UpdateAsync(DatabaseEntities.CustomerEntity customer);
        Task DeleteAsync(DatabaseEntities.CustomerEntity customer);
    }
;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reserva_Butacas.Data;
using Reserva_Butacas.Interfaces;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Repositories;

    public class BillboardRepository : IBillboardRepository
    {
        private readonly Context _context;

        public BillboardRepository(Context context)
        {
            _context = context;
        }

        public async Task<DatabaseEntities.BillboardEntity> GetById(int id)
        {
            try
            {
                return await _context.Billboards.FindAsync(id);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");

                throw; 
            }
        }

        public async Task<List<DatabaseEntities.BillboardEntity>> GetAll()
        {
            return await _context.Billboards.ToListAsync();
        }

        public async Task Add(DatabaseEntities.BillboardEntity billboard)
        {
            try
            {
                _context.Billboards.Add(billboard);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");

                throw; 
            }
        }

        public async Task Update(DatabaseEntities.BillboardEntity billboard)
        {
            _context.Billboards.Update(billboard);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var billboard = await _context.Billboards.FindAsync(id);
            if (billboard != null)
            {
                _context.Billboards.Remove(billboard);
                await _context.SaveChangesAsync();
            }
        }
        
        
        public async Task<List<DatabaseEntities.BillboardEntity>> GetByDate(DateTime date)
        {
            return await _context.Billboards
                .Where(b => b.Date.Date == date.Date) 
                .ToListAsync();
        }
    }

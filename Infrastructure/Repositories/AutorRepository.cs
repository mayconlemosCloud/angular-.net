using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AutorRepository : IBaseRepository<Autor>
    {
        private readonly MyContext _context;
        public AutorRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Autor>> GetAllAsync()
        {
            return await _context.Autores
                .Include(a => a.LivroAutores)
                    .ThenInclude(la => la.Livro)
                .ToListAsync();
        }

        public async Task<Autor?> GetByIdAsync(int id)
        {
           
            return await _context.Autores
                .Include(a => a.LivroAutores)
                    .ThenInclude(la => la.Livro)
                .FirstOrDefaultAsync(a => a.CodAu == id);
        }

        public async Task AddAsync(Autor entity)
        {
            await _context.Autores.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Autor entity)
        {
            _context.Autores.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int entity)
        {
            var autor = await _context.Autores.FindAsync(entity);
            if (autor != null)
            {
                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AssuntoRepository : IBaseRepository<Assunto>
    {
        private readonly MyContext _context;
        public AssuntoRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Assunto>> GetAllAsync()
        {
            return await _context.Assuntos
                .Include(a => a.LivroAssuntos)
                    .ThenInclude(la => la.Livro)
                .ToListAsync();
        }

        public async Task<Assunto?> GetByIdAsync(int id)
        {
            // Inclui livros ao buscar assunto por id
            return await _context.Assuntos
                .Include(a => a.LivroAssuntos)
                    .ThenInclude(la => la.Livro)
                .FirstOrDefaultAsync(a => a.CodAs == id);
        }

        public async Task AddAsync(Assunto entity)
        {
            await _context.Assuntos.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Assunto entity)
        {
            _context.Assuntos.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int entity)
        {
            var assunto = await _context.Assuntos.FindAsync(entity);
            if (assunto != null)
            {
                _context.Assuntos.Remove(assunto);
                await _context.SaveChangesAsync();
            }
        }
    }
}

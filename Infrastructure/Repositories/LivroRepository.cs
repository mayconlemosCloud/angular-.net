using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LivroRepository : IBaseRepository<Livro>
    {
        private readonly MyContext _context;

        public LivroRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Livro>> GetAllAsync()
        {
            return await _context.Livros
                .Include(l => l.LivroAutores)
                    .ThenInclude(la => la.Autor)
                .ToListAsync();
        }

        public async Task<Livro?> GetByIdAsync(int id)
        {
            // Inclui autores ao buscar livro por id
            return await _context.Livros
                .Include(l => l.LivroAutores)
                    .ThenInclude(la => la.Autor)
                .FirstOrDefaultAsync(l => l.Codl == id);
        }

        public async Task AddAsync(Livro entity)
        {
            await _context.Livros.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Livro entity)
        {
            _context.Livros.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int entity)
        {
            var user = await _context.Livros.FindAsync(entity);
            if (user != null)
            {
                _context.Livros.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
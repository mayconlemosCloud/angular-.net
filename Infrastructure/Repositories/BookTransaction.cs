using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookTransactionRepository : IBaseRepository<BookTransaction>
    {
        private readonly MyContext _context;

        public BookTransactionRepository(MyContext context)
        {
            _context = context;
        }     
        public Task AddAsync(BookTransaction entity)
        {
            _context.BookTransactions.Add(entity);
            return _context.SaveChangesAsync();

        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BookTransaction>> GetAllAsync()
        {
            return await _context.BookTransactions.ToListAsync();
        }

        public Task<BookTransaction?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(BookTransaction entity)
        {
            throw new NotImplementedException();
        }
    }

}
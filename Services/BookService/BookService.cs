using AutoMapper;
using BookStore.Data;
using BookStore.DTOs;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStore.Services
{
    public class BookService : IBookService
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(BookStoreContext context, IMapper mapper, ILogger<BookService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BookReadDto>> GetAllAsync()
        {
            var books = await _context.Books.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<BookReadDto>>(books);
        }

        public async Task<BookReadDto?> GetByIdAsync(int id)
        {
            var book = await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            return book == null ? null : _mapper.Map<BookReadDto>(book);
        }

        public async Task<BookReadDto> CreateAsync(BookCreateDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created new book with ID {BookId}", book.Id);

            return _mapper.Map<BookReadDto>(book);
        }

        public async Task<bool> UpdateAsync(int id, BookUpdateDto dto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return false;

            _mapper.Map(dto, book);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated book with ID {BookId}", id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted book with ID {BookId}", id);
            return true;
        }
    }
}

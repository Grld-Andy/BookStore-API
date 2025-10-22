using AutoMapper;
using BookStore.Data;
using BookStore.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public readonly BookStoreContext _context;
        public readonly IMapper _mapper;

        public BooksController(BookStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetBooks()
        {
            var books = await _context.Books.AsNoTracking().ToListAsync();
            var result = _mapper.Map<IEnumerable<BookReadDto>>(books);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            
            if (book == null)
                return NotFound(new {Message = $"Book with id {id} not found"});

            return Ok(_mapper.Map<BookReadDto>(book));
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook([FromBody] BookCreateDto newBook)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            if (newBook == null)
                return BadRequest();

            var book = _mapper.Map<Book>(newBook);

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<BookReadDto>(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBook(int id, BookCreateDto bookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound(new {Message = $"Book with id {id} not found"});

            _mapper.Map(bookDto, book);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound(new {Message = $"Book with id {id} not found"});

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}


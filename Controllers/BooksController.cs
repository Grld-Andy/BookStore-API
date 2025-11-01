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
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        private readonly IRedisCacheService _cache;

        public BooksController(BookStoreContext context, IMapper mapper, IRedisCacheService cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetBooks()
        {
            var userId = Request.Headers["UserId"];

            var cacheKey = $"books_{userId}";
            var books = _cache.GetData<IEnumerable<Book>>(cacheKey);
            if(books is null)
            {
                books = await _context.Books.AsNoTracking().ToListAsync();
                _cache.SetData("books", books);
            }
            var result = _mapper.Map<IEnumerable<BookReadDto>>(books);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var userId = Request.Headers["UserId"];

            var cacheKey = $"book_{id}_{userId}";
            var book = _cache.GetData<Book>(cacheKey);

            if (book is null)
            {
                book = await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
                _cache.SetData(cacheKey, book);
            }
            
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


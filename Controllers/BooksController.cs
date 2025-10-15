using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.Data;
using BookStore.DTOs;
using AutoMapper;

namespace BookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly MongoDbService _mongoService;
    private readonly IMapper _mapper;

    public BooksController(MongoDbService mongoService, IMapper mapper)
    {
        _mongoService = mongoService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookReadDto>>> GetBooks()
    {
        var books = await _mongoService.GetAsync();
        return Ok(_mapper.Map<List<BookReadDto>>(books));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookReadDto>> GetBookById(string id)
    {
        var book = await _mongoService.GetAsync(id);
        if(book == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<BookReadDto>(book));
    }

    [HttpPost]
    public async Task<ActionResult<BookReadDto>> AddBook(BookCreateDto bookDto)
    {
        var newBook = _mapper.Map<Book>(bookDto);
        if(newBook == null)
            return BadRequest();

        await _mongoService.CreateAsync(newBook);
        return CreatedAtAction(nameof(GetBookById), new {id = newBook.Id}, newBook);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(string id, BookCreateDto bookDto)
    {
        var existingBook = await _mongoService.GetAsync(id);
        if(existingBook == null)
            return NotFound();
        
        _mapper.Map(bookDto, existingBook);
        await _mongoService.UpdateAsync(id, existingBook);
        return NoContent(); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(string id)
    {
        var book = await _mongoService.GetAsync(id);
        if(book == null)
            return NotFound();

        await _mongoService.RemoveAsync(id);
        return NoContent();
    }
}

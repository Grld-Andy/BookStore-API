using BookStore.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookReadDto>> GetAllAsync();
        Task<BookReadDto?> GetByIdAsync(int id);
        Task<BookReadDto> CreateAsync(BookCreateDto dto);
        Task<bool> UpdateAsync(int id, BookUpdateDto dto);
        Task<bool> DeleteAsync(int id);

    }
}



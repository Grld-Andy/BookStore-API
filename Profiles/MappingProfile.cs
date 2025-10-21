using AutoMapper;
using BookStore.DTOs;

namespace BookStore.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookReadDto>();
        CreateMap<BookCreateDto, Book>();
    }
}
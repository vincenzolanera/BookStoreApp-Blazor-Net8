using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Authors;
using BookStoreApp.API.Models.Books;
using BookStoreApp.API.Models.User;

namespace BookStoreApp.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Author, AuthorCreateDto>();
            CreateMap<Author, AuthorReadOnlyDto>();
            CreateMap<AuthorUpdateDto, Author>();

            CreateMap<Book, BookReadOnlyDto>()
                .ForMember(e => e.AuthorId, d => d.MapFrom(map => map.Author.Id))
                .ForMember(e => e.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"));
            CreateMap<Book, BookDetailDto>()
                .ForMember(e => e.AuthorId, d => d.MapFrom(map => map.Author.Id))
                .ForMember(e => e.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"));
            CreateMap<BookUpdateDto, Book>();
            CreateMap<BookCreateDto, Book>();

            CreateMap<UserDto, ApiUser>()
                .ForMember(e => e.UserName, d => d.MapFrom(map => map.Email));
;
        }
    }
}
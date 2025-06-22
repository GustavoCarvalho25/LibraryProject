using Application.Commands.Books;
using Application.Models;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookViewModel>()
            .ForMember(b => b.ISBN, b => b.MapFrom(book => book.ISBN.Value));
        
        CreateMap<AddBookCommand, Book>()
            .ConstructUsing(cmd => new Book(cmd.Title, cmd.Author, cmd.ISBN, cmd.PublicationYear));
            
        CreateMap<UpdateBookCommand, Book>()
            .ConstructUsing(cmd => new Book(cmd.Title, cmd.Author, cmd.ISBN, cmd.PublicationYear));
            
        CreateMap<Loan, LoanViewModel>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title));
    }
}
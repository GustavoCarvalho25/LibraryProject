using Application.Commands.Books.AddBookCommand;
using Application.Commands.Books.UpdateBookCommand;
using Application.Commands.Users.AddUserCommand;
using Application.Commands.Users.UpdateUserCommand;
using Application.Models;
using Application.ViewModels;
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
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.LoanDate.AddDays(14)));
            
        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.ActiveLoans, opt => opt.MapFrom(src => 
                src.Loans != null ? src.Loans.Count(l => l.ReturnDate == null) : 0));
            
        CreateMap<AddUserCommand, User>()
            .ConstructUsing(cmd => new User(cmd.Name, cmd.Email));
            
        CreateMap<UpdateUserCommand, User>()
            .ConstructUsing(cmd => new User(cmd.Name, cmd.Email));
    }
}
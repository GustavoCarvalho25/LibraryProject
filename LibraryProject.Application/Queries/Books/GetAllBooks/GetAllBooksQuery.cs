using Application.Models;
using MediatR;

namespace Application.Queries.Books.GetAllBooks;

public class GetAllBooksQuery : IRequest<ResultViewModel<IEnumerable<BookViewModel>>>
{
}
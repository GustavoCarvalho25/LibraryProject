using Application.Commands.Books.AddBookCommand;
using Application.Commands.Books.RemoveBookCommand;
using Application.Commands.Books.UpdateBookCommand;
using Application.Models;
using AutoMapper;
using Core.Repository;
using MediatR;

namespace Application.Handlers.Book;

public class BookCommandHandler : 
    IRequestHandler<AddBookCommand, ResultViewModel<BookViewModel>>,
    IRequestHandler<UpdateBookCommand, ResultViewModel<BookViewModel>>,
    IRequestHandler<RemoveBookCommand, ResultViewModel>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public BookCommandHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<ResultViewModel<BookViewModel>> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var book = _mapper.Map<Core.Entities.Book>(request);
        
        var result = await _bookRepository.Add(book);
        
        if (result is null)
        {
            return ResultViewModel<BookViewModel>.Error("Failed to add book.");
        }

        return ResultViewModel<BookViewModel>.Success(_mapper.Map<BookViewModel>(result));
    }
    
    public async Task<ResultViewModel<BookViewModel>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var existingBook = await _bookRepository.GetById(request.Id);
        
        if (existingBook is null)
        {
            return ResultViewModel<BookViewModel>.Error($"Book with ID {request.Id} not found.");
        }
        
        var updatedBook = _mapper.Map<Core.Entities.Book>(request);
        
        updatedBook.Id = existingBook.Id;
        
        var updateSuccess = await _bookRepository.Update(updatedBook);
        
        if (!updateSuccess)
        {
            return ResultViewModel<BookViewModel>.Error("Failed to update book.");
        }
        
        var updatedBookFromDb = await _bookRepository.GetById(updatedBook.Id);
        return ResultViewModel<BookViewModel>.Success(_mapper.Map<BookViewModel>(updatedBookFromDb));
    }
    
    public async Task<ResultViewModel> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
    {
        var existingBook = await _bookRepository.GetById(request.Id);
        
        if (existingBook is null)
        {
            return ResultViewModel.Error($"Book with ID {request.Id} not found.");
        }
        
        if (!existingBook.IsAvailable)
        {
            return ResultViewModel.Error("Cannot remove a book that is currently loaned.");
        }
        
        var removeSuccess = await _bookRepository.Remove(existingBook);
        
        if (!removeSuccess)
        {
            return ResultViewModel.Error("Failed to remove book.");
        }
        
        return ResultViewModel.Success();
    }
}
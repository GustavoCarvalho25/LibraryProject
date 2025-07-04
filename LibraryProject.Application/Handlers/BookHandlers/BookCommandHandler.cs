using Application.Commands.Books.AddBookCommand;
using Application.Commands.Books.RemoveBookCommand;
using Application.Commands.Books.UpdateBookCommand;
using Application.Models;
using AutoMapper;
using Core.Repository;
using MediatR;
using Core.Entities;

namespace Application.Handlers.BookHandlers;

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
        var book = _mapper.Map<Book>(request);
        
        var result = await _bookRepository.Add(book);
        
        // TODO: isso não tem como ser nulo nunca. Alterar o repository para retornar o saveChanges! Está em ciclo
        if (result is null) 
            return ResultViewModel<BookViewModel>.Error("Failed to add entity");

        return ResultViewModel<BookViewModel>.Success(_mapper.Map<BookViewModel>(result));
    }
    
    public async Task<ResultViewModel<BookViewModel>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(request.Id);
        
        if (book is null)
            return ResultViewModel<BookViewModel>.Error($"Book with ID {request.Id} not found");
        
        book.Update(
            request.Title,
            request.Author,
            request.ISBN,
            request.PublicationYear
        );
        
        var updateSuccess = await _bookRepository.Update(book);
        
        if (!updateSuccess)
            return ResultViewModel<BookViewModel>.Error("Failed to update entity");
        
        return ResultViewModel<BookViewModel>.Success(_mapper.Map<BookViewModel>(book));
    }
    
    public async Task<ResultViewModel> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetById(request.Id);
        
        if (book is null)
            return ResultViewModel.Error($"Entity with ID {request.Id} not found");
        
        if (!book.IsAvailable)
            return ResultViewModel.Error("Cannot remove a book that is currently loaned");
        
        book.Remove();
        
        var removeSuccess = await _bookRepository.Update(book);
        
        if (!removeSuccess)
            return ResultViewModel.Error("Failed to remove entity");
        
        return ResultViewModel.Success();
    }
}
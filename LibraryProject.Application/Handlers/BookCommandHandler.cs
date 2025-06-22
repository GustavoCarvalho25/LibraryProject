using Application.Commands.Books;
using Application.Models;
using AutoMapper;
using Core.Entities;
using Core.Repository;
using MediatR;

namespace Application.Handlers;

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
        
        if (result is null)
        {
            return ResultViewModel<BookViewModel>.Error("Failed to add book.");
        }

        return ResultViewModel<BookViewModel>.Success(_mapper.Map<BookViewModel>(result));
    }
    
    public async Task<ResultViewModel<BookViewModel>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var existingBook = await _bookRepository.GetById(request.Id);
        
        if (existingBook == null)
        {
            return ResultViewModel<BookViewModel>.Error($"Book with ID {request.Id} not found.");
        }
        
        // Atualizar as propriedades do livro existente
        var updatedBook = new Book(
            request.Title,
            request.Author,
            request.ISBN,
            request.PublicationYear,
            existingBook.IsAvailable
        );
        
        // Manter o mesmo ID
        updatedBook.Id = existingBook.Id;
        
        var result = await _bookRepository.Update(updatedBook);
        
        if (result is null)
        {
            return ResultViewModel<BookViewModel>.Error("Failed to update book.");
        }
        
        return ResultViewModel<BookViewModel>.Success(_mapper.Map<BookViewModel>(result));
    }
    
    public async Task<ResultViewModel> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
    {
        var existingBook = await _bookRepository.GetById(request.Id);
        
        if (existingBook == null)
        {
            return ResultViewModel.Error($"Book with ID {request.Id} not found.");
        }
        
        // Verificar se o livro pode ser removido (não está emprestado)
        if (!existingBook.IsAvailable)
        {
            return ResultViewModel.Error("Cannot remove a book that is currently loaned.");
        }
        
        await _bookRepository.Remove(existingBook);
        
        return ResultViewModel.Success();
    }
}
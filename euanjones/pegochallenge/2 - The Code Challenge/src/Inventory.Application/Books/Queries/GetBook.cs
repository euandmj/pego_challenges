using Inventory.Books;
using Inventory.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Books.Queries;

public static class GetBook
{
	public record GetBookRequest(long Id) : IRequest<Book>;

	internal sealed class GetBookRequestHandler : IRequestHandler<GetBookRequest, Book>
	{
		private readonly InventoryDbContext _context;

		public GetBookRequestHandler(InventoryDbContext context)
		{
			_context = context;
		}

		public async Task<Book> Handle(GetBookRequest request, CancellationToken cancellationToken)
		{
			var book = await _context.Books.AsNoTracking()
				.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
				?? throw new BookNotFoundException(request.Id);

			return new Book
			{
				Id = book.Id,
				Author = book.Author,
				Title = book.Title,
				PublishDate = book.PublishDate,
				Quantity = book.Quantity
			};
		}
	}
}

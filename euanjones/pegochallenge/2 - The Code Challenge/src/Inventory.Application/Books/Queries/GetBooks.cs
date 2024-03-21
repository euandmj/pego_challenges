using Inventory.Books;
using Inventory.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Books.Queries;

public static class GetBooks
{
	public record GetBooksRequest(int Offset, int PageSize) : IRequest<IEnumerable<Book>>;

	internal sealed class GetBooksRequestHandler : IRequestHandler<GetBooksRequest, IEnumerable<Book>>
	{
		private readonly InventoryDbContext _context;

		public GetBooksRequestHandler(InventoryDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Book>> Handle(GetBooksRequest request, CancellationToken cancellationToken)
		{
			var books = await _context.Books.AsNoTracking()
				.Skip((request.Offset - 1) * request.PageSize)
				.Take(request.PageSize)
				.ToListAsync(cancellationToken);

			return books.Select(book => new Book
			{
				Id = book.Id,
				Author = book.Author,
				Title = book.Title,
				PublishDate = book.PublishDate,
				Quantity = book.Quantity
			});
		}
	}
}

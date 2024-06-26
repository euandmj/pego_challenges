﻿using Inventory.Application.Books.Commands;
using Inventory.Application.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Books;

public static partial class AddBookEndpoints
{
	public static IEndpointRouteBuilder AddPublicApi(this IEndpointRouteBuilder builder)
	{
		builder.MapGet("/book/{id}", async (long id, CancellationToken cancellationToken, 
			[FromServices] IMediator mediator) =>
		{
			var book = await mediator.Send(new GetBook.GetBookRequest(id), cancellationToken);
			return Results.Ok(book);
		}).CacheOutput().WithOpenApi();

		builder.MapGet("/books", async (CancellationToken cancellationToken, 
			[FromServices] IMediator mediator, int offset = 1, int pageSize = 10) =>
		{
			var books = await mediator.Send(new GetBooks.GetBooksRequest(offset, pageSize), cancellationToken);
			return Results.Ok(books);
		}).CacheOutput().WithOpenApi();

		return builder;
	}

	public static IEndpointRouteBuilder AddAdminApi(this  IEndpointRouteBuilder builder)
	{
		var adminGroup = builder.MapGroup("/admin/books")
			.WithName("Books Admin")
			.CacheOutput()
			//.RequireAuthorization(p => p.RequireClaim("admin"))
			.WithOpenApi();

		adminGroup.MapPost(string.Empty, async (CreateBookRequest request,
			CancellationToken cancellationToken, [FromServices] IMediator mediator) =>
		{
			var response = await mediator.Send(new CreateBook.CreateBookRequest(
				request.Title, request.Author, request.Published, request.Quantity), cancellationToken);
			return Results.Ok(response.Id);
		}).WithName("Create Book");

		adminGroup.MapPut("/{id}", async (long id, UpdateBookRequest request,
			CancellationToken cancellationToken, [FromServices] IMediator mediator) =>
		{
			await mediator.Send(new UpdateBook.UpdateBookRequest(
				id, request.Title, request.Author, request.Published, request.Quantity), cancellationToken);
			return Results.NoContent();
		}).WithName("Update Book");

		adminGroup.MapDelete("/{id}", async (long id, CancellationToken cancellationToken,
			[FromServices] IMediator mediator) =>
		{
			await mediator.Send(new DeleteBook.DeleteBookRequest(id), cancellationToken);
			return Results.NoContent();
		}).WithName("Delete Book");

		return builder;
	}



}

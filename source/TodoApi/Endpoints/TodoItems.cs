using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.TodoItems;
using TodoApi.Application.Todos;
using TodoApi.Domain.Todos;
using TodoApi.Dtos;

namespace TodoApi.Endpoints
{
    internal static class TodoItems
    {
        internal static WebApplication MapTodoItemEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/todos/{todoId}")
                .WithTags("Todo Item")
                .WithOpenApi()
                .WithMetadata()
                .AddEndpointFilter(async (ctx, next) =>
                {
                    var stopwatch = Stopwatch.StartNew();
                    var result = await next(ctx);
                    stopwatch.Stop();
                    var elapsed = stopwatch.ElapsedMilliseconds;
                    var response = ctx.HttpContext.Response;
                    response.Headers.Add("X-Response-Time", $"{elapsed} milliseconds");

                    return result;
                });

            group.MapGet("/todo-items/", GetAllTodoItems)
                .WithMetadata(new SwaggerOperationAttribute("Get all todo item for a todo"))
                .Produces<TodoItemDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
            group.MapPost("/todo-items/", AddTodoItems)
                .WithMetadata(new SwaggerOperationAttribute("Add new todo item for a todo"))
                .Produces<TodoItemDto>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
            group.MapPut("/todo-items/{id}", UpdateTodoItem)
                .WithMetadata(new SwaggerOperationAttribute("Update todo item for a todo"))
                .Produces<TodoItemDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
            group.MapDelete("/todo-items/{id}", DeleteTodoItem)
                .WithMetadata(new SwaggerOperationAttribute("Delete todo item for a todo"))
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
            group.MapDelete("/todo-items/", DeleteAllTodoItems)
                .WithMetadata(new SwaggerOperationAttribute("Delete all todo items"))
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError);

            return app;
        }
        static async Task<IResult> GetAllTodoItems(int todoId, [AsParameters] TodoItemSearchCriteria searchCriteria, IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new TodoItemSearchRequest(todoId, searchCriteria));
                if (result != null && result.Any())
                {
                    return TypedResults.Ok(result);
                }
                return TypedResults.NotFound();
            }
            catch (Exception)
            {
                return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        static async Task<IResult> AddTodoItems(int todoId, CreateTodoInput input, IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new CreateTodoItemRequest(todoId, input));

                return TypedResults.Created($"/todos/{todoId}", result);
            }
            catch (Exception)
            {
                return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        static async Task<IResult> UpdateTodoItem(int todoId, CreateTodoInput input, IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new CreateTodoItemRequest(todoId, input));

                return TypedResults.Created($"/todos/{todoId}", result);
            }
            catch (Exception)
            {
                return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        static async Task<IResult> DeleteTodoItem(int todoId, int todoItemId, IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new DeleteTodoItemRequest(todoId, todoItemId));

                return TypedResults.Ok();
            }
            catch (Exception)
            {
                return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        static async Task<IResult> DeleteAllTodoItems(int todoId, int todoItemId, IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new DeleteAllTodoItemsRequest(todoId));

                return TypedResults.Ok();
            }
            catch (Exception)
            {
                return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
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

            group.MapGet("/todo-items", GetAllTodoItems);
            group.MapPost("/todo-items/add", AddTodoItems);

            return app;
        }
        static async Task<IResult> GetAllTodoItems(int todoId, [AsParameters] TodoItemSearchCriteria searchCriteria, IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new TodoItemSearchRequest(todoId, searchCriteria));

                return TypedResults.Ok(result);
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

                return TypedResults.Created($"/todos/{todoId}", todoId);
            }
            catch (Exception)
            {
                return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

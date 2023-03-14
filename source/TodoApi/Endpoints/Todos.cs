﻿using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;
using System.Net;
using TodoApi.Application.Common.Exceptions;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Todos;
using TodoApi.Domain.Todos;
using TodoApi.Dtos;
using TodoApi.Infrastructure.Middleware;

namespace TodoApi.Endpoints
{
    public static class Todos
    {

        internal static WebApplication MapTodoEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/todos")
                .WithTags("Todo")
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
            group.MapGet("/", SearchTodos)
                .WithMetadata(new SwaggerOperationAttribute("Get and filter todos"))
                .Produces<List<TodoDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .ProducesValidationProblem();
            group.MapPost("/", CreateTodo)
                .WithMetadata(new SwaggerOperationAttribute("Create a new todo"))
                .Produces<TodoDto>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .ProducesValidationProblem();
            group.MapGet("/{id}", GetTodo);
            group.MapPut("/{id}", UpdateTodo);
            group.MapDelete("/{id}", DeleteTodo);

            return app;
        }

        #region Todo
        static async Task<IResult> UpdateTodo(int id, UpdateTodoRequest request, IMediator mediator)
        {
            try
            {
                if (id == request.Id)
                {
                    var result = await mediator.Send(request);
                    return TypedResults.Created($"todos/{result.Id}", result);
                }
                return TypedResults.UnprocessableEntity(request);
            }
            catch (Exception)
            {
                return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        static async Task<IResult> SearchTodos([AsParameters] TodoSearchCriteria searchCriteria, IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new SearchTodoRequest(searchCriteria));
                if (result.Count < 1)
                {
                    return TypedResults.NoContent();
                }

                return TypedResults.Ok(result);
            }
            catch (Exception)
            {
                return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        static async Task<IResult> CreateTodo(CreateTodoRequest request, IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(request);
                return TypedResults.Created($"todos/{result.Id}", result);
            }
            catch (Exception)
            {
                return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        static async Task<IResult> DeleteTodo(int todoId, IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new DeleteTodoRequest(todoId));
                return TypedResults.Ok(result);
            }
            catch (Exception)
            {
                return TypedResults.UnprocessableEntity();
            }
        }
        static async Task<IResult> GetTodo(int todoId, IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new GetTodoRequest(todoId));
                return TypedResults.Ok(result);
            }
            catch (Exception)
            {
                return TypedResults.UnprocessableEntity();
            }
        }
        #endregion


    }
}

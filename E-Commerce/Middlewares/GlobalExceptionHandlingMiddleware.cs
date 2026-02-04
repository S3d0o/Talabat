using Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.ErrorModels;
using System.Net;

namespace E_Commerce.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
      
        public async Task InvokeAsync(HttpContext context) // context has all info about HTTP request and response
        {
            try
            {
                await _next(context); // Call the next middleware in the pipeline
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                    await HandleExceptionAsync(context);
            }
            catch(Exception ex)
            {
                _logger.LogError($"something went wrong in {context.Request.Path}, the error massage => {ex.Message} ");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = $"The end point with url {context.Request.Path} not found."
            };
            await context.Response.WriteAsJsonAsync(response);

        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // 1] Set the response status code and content type
            //context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // 2] Create a response object with error details
            var response = new ErrorDetails
            {
                ErrorMessage = ex.Message
            };
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnAuthorizedException => StatusCodes.Status401Unauthorized,
                ValidationException validationException=> HandleValidationException(validationException,response),
                (_) => StatusCodes.Status500InternalServerError
            };

            //change content type to json
            context.Response.ContentType = "application/json";

            response.StatusCode = context.Response.StatusCode;

            // 3] Write the response as JSON
            await context.Response.WriteAsJsonAsync(response);
        }

        private int HandleValidationException(ValidationException validationException, ErrorDetails response)
        {
            response.Errors = validationException.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}

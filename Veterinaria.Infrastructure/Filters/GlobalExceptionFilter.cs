using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.CustomEntities;
using Veterinaria.Core.Exceptions;

namespace Veterinaria.Infrastructure.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var path = context.HttpContext.Request.Path;

            int statusCode = GetStatusCode(exception);
            var errorResponse = CreateErrorResponse(exception, path, statusCode);

            LogException(exception, statusCode, path);

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;
        }

        private int GetStatusCode(Exception ex)
        {
            return ex switch
            {
                BusinessException bex => bex.StatusCode,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                NotImplementedException => StatusCodes.Status501NotImplemented,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        private ErrorResponse CreateErrorResponse(Exception ex, string path, int statusCode)
        {
            var response = new ErrorResponse
            {
                Type = ex.GetType().Name,
                Message = ex.Message,
                Path = path
            };

            if (ex is BusinessException bex && !string.IsNullOrEmpty(bex.ErrorCode))
                response.ErrorCode = bex.ErrorCode;

            return response;
        }

        private void LogException(Exception ex, int statusCode, string path)
        {
            var msg = $"[{statusCode}] Error en {path}: {ex.Message}";
            if (statusCode >= 500)
                _logger.LogError(ex, msg);
            else
                _logger.LogWarning(ex, msg);
        }
    }
}

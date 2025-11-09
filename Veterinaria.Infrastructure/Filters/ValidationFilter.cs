using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Infrastructure.Validators;

namespace Veterinaria.Infrastructure.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IValidationService _validationService;
        private readonly IServiceProvider _serviceProvider;

        public ValidationFilter(IValidationService validationService, IServiceProvider serviceProvider)
        {
            _validationService = validationService;
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                var type = argument.GetType();
                var validatorType = typeof(IValidator<>).MakeGenericType(type);
                var validator = _serviceProvider.GetService(validatorType);
                if (validator == null) continue;

                var method = typeof(IValidationService).GetMethod("ValidateAsync")!;
                var genericMethod = method.MakeGenericMethod(type);
                var validationTask = (Task<ValidationResult>)genericMethod.Invoke(_validationService, new[] { argument })!;
                var validationResult = await validationTask;

                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(new { Errors = validationResult.Errors });
                    return;
                }
            }

            await next();
        }
    }
}

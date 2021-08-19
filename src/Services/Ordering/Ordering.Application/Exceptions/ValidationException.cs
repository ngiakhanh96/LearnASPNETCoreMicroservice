using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures.GroupBy(
                    e => e.PropertyName,
                    e => e.ErrorMessage)
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.ToArray());
        }

        public override string Message => string.Join(", ", Errors
            .SelectMany(x => x.Value));

        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
    }
}

using FluentValidation;

namespace FOT.Application.UseCase.Orders.Queries.GetOrdersByFilter;

/// <summary>
/// Validator for <see cref="GetOrdersByFilterQuery"/>.
/// </summary>
public class GetOrdersByFilterQueryValidator : AbstractValidator<GetOrdersByFilterQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetOrdersByFilterQueryValidator"/>.
    /// </summary>
    public GetOrdersByFilterQueryValidator()
    {
        RuleFor(q => q.CreatedTo)
            .Must((q, createdTo) => createdTo >= q.CreatedFrom)
            .When(q => q.CreatedTo.HasValue && q.CreatedFrom.HasValue);

        RuleFor(q => q.UpdatedTo)
            .Must((q, updatedTo) => updatedTo >= q.UpdatedFrom)
            .When(q => q.UpdatedTo.HasValue && q.UpdatedFrom.HasValue);

        RuleFor(q => q.Limit)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.Offset)
            .GreaterThanOrEqualTo(0)
            .When(q => q.Offset.HasValue);

        RuleFor(q => q.FreeText)
            .MaximumLength(200);

        RuleForEach(q => q.OrderStatuses).IsInEnum();
    }
}
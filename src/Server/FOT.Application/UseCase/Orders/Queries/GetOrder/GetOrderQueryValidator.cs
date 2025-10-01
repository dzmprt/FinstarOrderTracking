using FluentValidation;

namespace FOT.Application.UseCase.Orders.Queries.GetOrder;

/// <summary>
/// Validator for <see cref="GetOrderQuery"/>.
/// </summary>
public class GetOrderQueryValidator : AbstractValidator<GetOrderQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetOrderQueryValidator"/>.
    /// </summary>
    public GetOrderQueryValidator()
    {
        RuleFor(cmd => cmd.OrderNumber)
            .NotEqual(Guid.Empty);
    }
}
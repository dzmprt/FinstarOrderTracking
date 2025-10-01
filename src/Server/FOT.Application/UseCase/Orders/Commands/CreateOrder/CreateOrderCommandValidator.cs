using FluentValidation;
using FOT.Domain.Orders;

namespace FOT.Application.UseCase.Orders.Commands.CreateOrder;

/// <summary>
/// Validator for <see cref="CreateOrderCommand"/>.
/// </summary>
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderCommandValidator"/>.
    /// </summary>
    public CreateOrderCommandValidator()
    {
        RuleFor(cmd => cmd.Description)
            .NotEmpty()
            .Length(Order.MinDescriptionLength, Order.MaxDescriptionLength);
    }
}
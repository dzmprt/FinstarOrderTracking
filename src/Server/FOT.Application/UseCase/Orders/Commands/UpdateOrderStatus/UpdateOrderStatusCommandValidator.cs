using FluentValidation;

namespace FOT.Application.UseCase.Orders.Commands.UpdateOrderStatus;

/// <summary>
/// Validator for <see cref="UpdateOrderStatusCommand"/>.
/// </summary>
public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateOrderStatusCommandValidator"/>.
    /// </summary>
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(cmd => cmd.OrderNumber)
            .NotEqual(Guid.Empty);

        RuleFor(cmd => cmd.NewStatus).IsInEnum();
    }
}
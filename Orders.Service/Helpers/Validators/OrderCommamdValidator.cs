using FluentValidation;
using Orders.Service.Commands;

namespace Orders.Service.Helpers.Validators
{
    public class OrderCommamdValidator
    {
        public class CheckoutOrderCommamdValidator : AbstractValidator<CheckoutOrderCommand>
        {
            public CheckoutOrderCommamdValidator()
            {
                // User identity
                RuleFor(o => o.UserName)
                    .NotEmpty().WithMessage("UserName is required.");

                RuleFor(o => o.FirstName)
                    .NotEmpty().WithMessage("FirstName is required.");

                RuleFor(o => o.LastName)
                    .NotEmpty().WithMessage("LastName is required.");

                RuleFor(o => o.EmailAddress)
                    .NotEmpty().WithMessage("EmailAddress is required.")
                    .EmailAddress().WithMessage("EmailAddress is not a valid email address.");

                // Order details
                RuleFor(o => o.TotalPrice)
                    .NotNull().WithMessage("TotalPrice is required.")
                    .GreaterThan(0).WithMessage("TotalPrice must be greater than zero.");

                // Address validation
                RuleFor(o => o.AddressLine)
                    .NotEmpty().WithMessage("AddressLine is required.");

                RuleFor(o => o.Country)
                    .NotEmpty().WithMessage("Country is required.");

                RuleFor(o => o.State)
                    .NotEmpty().WithMessage("State is required.");

                RuleFor(o => o.ZipCode)
                    .NotEmpty().WithMessage("ZipCode is required.")
                    .Matches(@"^\d{6}$").WithMessage("ZipCode must be 6 digits.");

                // Payment details
                RuleFor(o => o.PaymentMethod)
                    .NotEmpty().WithMessage("PaymentMethod is required.")
                    .Must(method => method == 1 || method == 2 || method == 3 || method == 4)
                    .WithMessage("PaymentMethod must be CreditCard, DebitCard, NetBanking or UPI.");

                RuleFor(o => o.CardName)
                    .NotEmpty().WithMessage("CardName is required.");

                RuleFor(o => o.CardNumber)
                    .NotEmpty().WithMessage("CardNumber is required.")
                    .CreditCard().WithMessage("CardNumber must be a valid credit card number.");

                RuleFor(o => o.CVV)
                    .NotEmpty().WithMessage("CVV is required.")
                    .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits.");


            }

        }

        public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
        {
            public UpdateOrderCommandValidator()
            {
                // Id must always be present for updates
                RuleFor(o => o.Id)
                    .GreaterThan(0).WithMessage("Id must be greater than zero.");

                // User identity
                RuleFor(o => o.UserName)
                    .NotEmpty().WithMessage("UserName is required.");

                RuleFor(o => o.FirstName)
                    .NotEmpty().WithMessage("FirstName is required.");

                RuleFor(o => o.LastName)
                    .NotEmpty().WithMessage("LastName is required.");

                RuleFor(o => o.EmailAddress)
                    .NotEmpty().WithMessage("EmailAddress is required.")
                    .EmailAddress().WithMessage("EmailAddress is not a valid email address.");

                // Order details
                RuleFor(o => o.TotalPrice)
                    .NotNull().WithMessage("TotalPrice is required.")
                    .GreaterThan(0).WithMessage("TotalPrice must be greater than zero.");

                // Address validation
                RuleFor(o => o.AddressLine)
                    .NotEmpty().WithMessage("AddressLine is required.");

                RuleFor(o => o.Country)
                    .NotEmpty().WithMessage("Country is required.");

                RuleFor(o => o.State)
                    .NotEmpty().WithMessage("State is required.");

                RuleFor(o => o.ZipCode)
                    .NotEmpty().WithMessage("ZipCode is required.")
                    .Matches(@"^\d{5,6}$").WithMessage("ZipCode must be 5 or 6 digits.");

                // Payment details
                RuleFor(o => o.PaymentMethod)
                    .NotEmpty().WithMessage("PaymentMethod is required.")
                    .Must(method => method == 1 || method == 2 || method == 3 || method == 4)
                    .WithMessage("PaymentMethod must be CreditCard, DebitCard, or UPI.");

                // Conditional rules: only validate card details if PaymentMethod is card-based
                When(o => o.PaymentMethod == 1 || o.PaymentMethod == 2, () =>
                {
                    RuleFor(o => o.CardName)
                        .NotEmpty().WithMessage("CardName is required.");

                    RuleFor(o => o.CardNumber)
                        .NotEmpty().WithMessage("CardNumber is required.")
                        .CreditCard().WithMessage("CardNumber must be a valid credit card number.");

                    RuleFor(o => o.CVV)
                        .NotEmpty().WithMessage("CVV is required.")
                        .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits.");

                    RuleFor(o => o.ExpirationDate)
                        .NotEmpty().WithMessage("ExpirationDate is required.")
                        .Must(date => date.HasValue && date.Value > DateTime.Now)
                        .WithMessage("ExpirationDate must be in the future.");
                });
            }
        }

        public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
        {
            public DeleteOrderCommandValidator()
            {
                // Id must always be present for updates
                RuleFor(o => o.Id)
                    .GreaterThan(0).WithMessage("Id must be greater than zero.");

            }
        }
    }
}
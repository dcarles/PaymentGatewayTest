using System;
using System.Linq;
using FluentValidation;
using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Validators
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            RuleFor(request => request.CardNumber).NotNull();
            RuleFor(request => request.CardNumber).NotEmpty();
            RuleFor(request => request.CardNumber).CreditCard().WithMessage("Card number is not in correct format");
            RuleFor(request => request.Cvv).NotNull();
            RuleFor(request => request.Cvv).NotEmpty();

            When(request => request.CardNumber != null && (request.CardNumber.StartsWith("34")
                            || request.CardNumber.StartsWith("37")), () =>
            {
                RuleFor(request => request.Cvv).Length(4).WithMessage("Card Security code must be 4 digits");

            }).Otherwise(() =>
            {
                RuleFor(request => request.Cvv).Length(3).WithMessage("Card Security code must be 3 digits");

            });

            RuleFor(request => request.ExpiryMonth).NotEqual(0).WithMessage("Expiry Month should have a value");
            RuleFor(request => request.ExpiryMonth).InclusiveBetween(1, 12).WithMessage("Invalid expiry month"); // should there be a more clear message?

            RuleFor(request => request.ExpiryYear).NotEqual(0).WithMessage("Expiry Year should have a value");
            RuleFor(request => request.ExpiryYear).GreaterThanOrEqualTo(DateTime.Today.Year).WithMessage("Expiry year is in past");

            // Year and Month Checked Combined
            When(request =>
                    request.ExpiryYear == DateTime.Today.Year,
                () =>
                {
                    RuleFor(request => request.ExpiryMonth).GreaterThanOrEqualTo(DateTime.Today.Month)
                        .WithMessage("Expiry Date cannot be in past");
                });


            RuleFor(request => request.Amount).GreaterThanOrEqualTo(0).WithMessage("Requested amount cannot be negative");

            RuleFor(request => request.Currency).NotNull().WithMessage("Currency cannot be null");
            RuleFor(request => request.Currency).NotEmpty().WithMessage("Currency cannot be empty");
            RuleFor(request => request.Currency).Length(3).WithMessage("Currency should be 3 letters");
            RuleFor(x => x.Currency).Custom((currency, context) =>
            {
                if (currency != null && currency.Any(char.IsDigit))
                {
                    context.AddFailure("Currency should only contains letters");
                }
            });
        }
    }
}

using FluentValidation.TestHelper;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Validators;
using System;
using Xunit;

namespace PaymentGateway.Api.Tests.Validators
{
    public class PaymentRequestValidatorTests : IDisposable
    {
        private PaymentRequestValidator _validator;

        /// <summary>
        /// Setup validator to be tested
        /// </summary>
        public PaymentRequestValidatorTests()
        {
            _validator = new PaymentRequestValidator();
        }

        #region Card Number 
        [Fact]
        public void CardNumber_IsNull_ShouldHaveError()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.CardNumber, null as string);
        }

        [Fact]
        public void CardNumber_IsEmpty_ShouldHaveError()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.CardNumber, string.Empty);
        }

        [Fact]
        public void CardNumber_IsNotValid_ShouldHaveError()
        {
            _validator.ShouldNotHaveValidationErrorFor(request => request.CardNumber, "1234567890123452"); // simple
            _validator.ShouldNotHaveValidationErrorFor(request => request.CardNumber, "1234 5678 9012 3452"); // With spaces
            _validator.ShouldNotHaveValidationErrorFor(request => request.CardNumber, "1234-5678-9012-3452");  // With Dashes
            _validator.ShouldHaveValidationErrorFor(request => request.CardNumber, "123456789012345");  // 15 digits
        }

        #endregion

        #region Cvv 
        [Fact]
        public void Cvv_IsNull_ShouldHaveError()
        {
            var paymentRequest = new PaymentRequest() { CardNumber = "1234567890123452" };
            _validator.ShouldHaveValidationErrorFor(request => request.Cvv, paymentRequest);
        }
        [Fact]
        public void Cvv_IsEmpty_ShouldHaveError()
        {
            var paymentRequest = new PaymentRequest() { CardNumber = "1234567890123452" };
            _validator.ShouldHaveValidationErrorFor(request => request.Cvv, paymentRequest);
        }

        [Fact]
        public void Cvv_Is3DigitsForAmex_ShouldHaveError()
        {
            var paymentRequest = new PaymentRequest() { CardNumber = "3734567890123452", Cvv = "123" };
            _validator.ShouldHaveValidationErrorFor(request => request.Cvv, paymentRequest);
        }

        [Fact]
        public void Cvv_Is4DigitsForNonAmex_ShouldHaveError()
        {
            var paymentRequest = new PaymentRequest() { CardNumber = "4534567890123452", Cvv = "1235" };
            _validator.ShouldHaveValidationErrorFor(request => request.Cvv, paymentRequest);
        }

        [Fact]
        public void Cvv_Is4DigitsForAmex_ShouldNotHaveError()
        {
            var paymentRequest = new PaymentRequest() { CardNumber = "3456789012345223", Cvv = "1235" };
            _validator.ShouldNotHaveValidationErrorFor(request => request.Cvv, paymentRequest);
        }

        [Fact]
        public void Cvv_Is3DigitsForNonAmex_ShouldNotHaveError()
        {
            var paymentRequest = new PaymentRequest() { CardNumber = "5456789012345223", Cvv = "123" };
            _validator.ShouldNotHaveValidationErrorFor(request => request.Cvv, paymentRequest);
        }
        #endregion

        #region Expiry Tests
        [Fact]
        public void ExpiryMonth_Is0_ShouldHaveError()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.ExpiryMonth, 0);
        }

        [Fact]
        public void ExpiryMonth_IsNotValid_ShouldHaveError()
        {
            _validator.ShouldNotHaveValidationErrorFor(request => request.ExpiryMonth, 1);
            _validator.ShouldNotHaveValidationErrorFor(request => request.ExpiryMonth, 5);
            _validator.ShouldNotHaveValidationErrorFor(request => request.ExpiryMonth, 12);
            _validator.ShouldHaveValidationErrorFor(request => request.ExpiryMonth, 13);
            _validator.ShouldHaveValidationErrorFor(request => request.ExpiryMonth, -1);
        }

        [Fact]
        public void ExpiryYear_Is0_ShouldHaveError()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.ExpiryYear, 0);
        }

        [Fact]
        public void ExpiryYear_IsInPast_ShouldHaveError()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.ExpiryYear, 2018);
            _validator.ShouldNotHaveValidationErrorFor(request => request.ExpiryYear, DateTime.Today.Year);

        }

        [Fact]
        public void ExpiryMonth_IsInPast_ShouldHaveError()
        {
            var paymentRequest = new PaymentRequest() { ExpiryYear = DateTime.Today.Year, ExpiryMonth = 1 };
            _validator.ShouldHaveValidationErrorFor(request => request.ExpiryMonth, paymentRequest);
        }
        #endregion

        #region Amount

        [Fact]
        public void Amount_IsNegative_ShouldHaveError()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.Amount, -1);
        }


        #endregion

        #region Currency

        [Fact]
        public void Currency_IsNull_ShouldHaveError()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.Currency, null as string);
        }
        [Fact]
        public void Currency_IsEmpty_ShouldHaveError()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.Currency, string.Empty);
        }

        [Fact]
        public void Currency_LongerThan3Character_ShouldHaveError()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.Currency, "EURR");
            _validator.ShouldNotHaveValidationErrorFor(request => request.Currency, "EUR");

        }

        [Fact]
        public void Currency_HasNumericCharacter_ShouldHaveError()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.Currency, "3UR");
            _validator.ShouldNotHaveValidationErrorFor(request => request.Currency, "EUR");

        }


        #endregion

        public void Dispose()
        {
            _validator = null;
        }
    }
}

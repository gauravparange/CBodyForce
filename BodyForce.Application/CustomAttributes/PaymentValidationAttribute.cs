using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class PaymentValidationAttribute : ValidationAttribute
    {
        private readonly string _errorMessageWhenEmpty;
        public PaymentValidationAttribute(string errorMessageWhenEmpty)
        {
            _errorMessageWhenEmpty = errorMessageWhenEmpty;
        }
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var model = (MembershipDto)validationContext.ObjectInstance;

            // If PaymentMethod is "Pending", skip validation
            if (model.PaymentMethod == "Pending")
            {
                return ValidationResult.Success;
            }

            // Validate AmountPaid and PaymentDate
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(_errorMessageWhenEmpty);
            }

            return ValidationResult.Success;
        }
    }
}

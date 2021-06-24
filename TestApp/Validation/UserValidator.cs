using System;
using TestApp.Models;

namespace TestApp.Validation
{
    public class UserValidator
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserValidator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public bool HasValidFullName(string firstName, string surName)
        {
            return !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(surName);
        }

        public bool HasValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        public bool IsUserAtLeast21YearsOld(DateTime dateOfBirth)
        {
            var now = _dateTimeProvider.DateTimeUtcNow;
            var age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            return age >= 21;
        }

        public bool HasCreditLimitAndLimitLessThan500(User user)
        {
            return user.HasCreditLimit && user.CreditLimit < 500;
        }
    }
}

using TestApp.Models;
using TestApp.Services;

namespace TestApp.CreditProviders
{
    public class ImportantClientCreditLimitProvider : ICreditLimitProvider
    {
        private readonly IUserCreditService _userCreditService;

        public ImportantClientCreditLimitProvider(IUserCreditService userCreditService)
        {
            _userCreditService = userCreditService;
        }

        public (bool HasCreditLimit, int CreditLimit) GetCreditLimits(User user)
        {
            var creditLimit = _userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
            creditLimit = creditLimit * 2;

            return (true, creditLimit);
        }

        public string NameRequirement => "ImportantClient";
    }
}

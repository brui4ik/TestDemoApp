using TestApp.Models;

namespace TestApp.CreditProviders
{
    public class VeyImportantClientCreditLimitProvider: ICreditLimitProvider
    {
        public (bool HasCreditLimit, int CreditLimit) GetCreditLimits(User user)
        {
            return (false, 0);
        }

        public string NameRequirement => "VeryImportantClient";
    }
}

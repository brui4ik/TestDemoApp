using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TestApp.Services;

namespace TestApp.CreditProviders
{
    public class CreditLimitProviderFactory
    {
        private readonly IReadOnlyDictionary<string, ICreditLimitProvider> _creditLimitProviders;

        public CreditLimitProviderFactory(IUserCreditService userCreditService)
        {
            var creditLimitProviderType = typeof(ICreditLimitProvider);
            _creditLimitProviders = creditLimitProviderType.Assembly.ExportedTypes
                .Where(_ => creditLimitProviderType.IsAssignableFrom(_) && !_.IsInterface && !_.IsAbstract)
                .Select(_ =>
                {
                    var parameterlessCtor = _.GetConstructors().SingleOrDefault(c => c.GetParameters().Length == 0);
                    return parameterlessCtor != null
                        ? Activator.CreateInstance(_)
                        : Activator.CreateInstance(_, userCreditService);
                })
                .Cast<ICreditLimitProvider>()
                .ToImmutableDictionary(_ => _.NameRequirement, _ => _);
        }

        public ICreditLimitProvider GetProviderByClientName(string clientName)
        {
            var provider = _creditLimitProviders.GetValueOrDefault(clientName);

            return provider ?? _creditLimitProviders[string.Empty];
        }
    }
}

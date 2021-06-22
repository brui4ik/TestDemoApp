using System;
using TestApp.CreditProviders;
using TestApp.DataAccess;
using TestApp.Models;
using TestApp.Repositories;
using TestApp.Validation;

namespace TestApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly CreditLimitProviderFactory _creditLimitProviderFactory;
        private readonly IUserDataAccess _userDataAccess;
        private readonly UserValidator _userValidator;

        public UserService(IClientRepository clientRepository,
            CreditLimitProviderFactory creditLimitProviderFactory,
            IUserDataAccess userDataAccess,
            UserValidator userValidator)
        {
            _clientRepository = clientRepository;
            _creditLimitProviderFactory = creditLimitProviderFactory;
            _userDataAccess = userDataAccess;
            _userValidator = userValidator;
        }

        public bool AddUser(string firstName, string surName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!UserProvidedDataIsValid(firstName, surName, email, dateOfBirth))
            {
                return false;
            }

            var client = _clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firstName,
                Surname = surName
            };

            var provider = _creditLimitProviderFactory.GetProviderByClientName(client.Name);
            var (hasCreditLimit, creditLimit) = provider.GetCreditLimits(user);

            user.HasCreditLimit = hasCreditLimit;
            user.CreditLimit = creditLimit;

            if (!_userValidator.HasCreditLimitAndLimitLessThan500(user))
            {
                return false;
            }
            
            _userDataAccess.AddUser(user);

            return true;
        }

        private bool UserProvidedDataIsValid(string firstName, string surName, string email, DateTime dob)
        {
            if (!_userValidator.HasValidFullName(firstName, surName))
            {
                return false;
            }

            if (!_userValidator.HasValidEmail(email))
            {
                return false;
            }

            if (!_userValidator.IsUserAtLeast21YearsOld(dob))
            {
                return false;
            }

            return true;
        }
    }
}
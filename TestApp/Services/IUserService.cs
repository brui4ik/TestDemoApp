using System;

namespace TestApp.Services
{
    public interface IUserService
    {
        bool AddUser(string firstName, string surName, string email, DateTime dateOfBirth, int clientId);
    }
}
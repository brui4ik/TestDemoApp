using TestApp.Models;

namespace TestApp.DataAccess
{
    public class UserDataAccessProxy: IUserDataAccess
    {
        public void AddUser(User user)
        {
            UserDataAccess.AddUser(user);
        }
    }
}

using TestApp.Models;

namespace TestApp.Repositories
{
    public interface IClientRepository
    {
        public Client GetById(int id);
    }
}

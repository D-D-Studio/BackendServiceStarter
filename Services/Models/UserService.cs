using System.Threading.Tasks;
using BackendServiceStarter.Databases;
using BackendServiceStarter.Models;
using BackendServiceStarter.Services.Crypto;
using Microsoft.EntityFrameworkCore;

namespace BackendServiceStarter.Services.Models
{
    public class UserService : ModelService<User>
    {
        private readonly IHashService _hashService;
        
        public UserService(ApplicationContext context, IHashService hashService) : base(context)
        {
            _hashService = hashService;
        }

        public override Task Create(User modelObject)
        {
            modelObject.Password = _hashService.Hash(modelObject.Password);
            
            return base.Create(modelObject);
        }

        public Task<User> FindByEmail(string email)
        {
            return _models.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
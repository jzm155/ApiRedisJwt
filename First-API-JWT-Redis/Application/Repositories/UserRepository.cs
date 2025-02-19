using First_API_JWT_Redis.Application.Domain;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace First_API_JWT_Redis.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDistributedCache _redis;

        public UserRepository(IDistributedCache redis) {
            _redis = redis;
        }
        
        public async Task<User> Add(User user)
        {
            await _redis.SetStringAsync(user.Username, JsonSerializer.Serialize(user));

            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var data = await _redis.GetStringAsync(username);

            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            var user = JsonSerializer.Deserialize<User>(data);
            return user;
        }
    }
}

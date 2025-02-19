namespace First_API_JWT_Redis.Application.Domain
{
    public class User
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        protected User() { }

        public User(string name, string username, string password)
        {
            Name = name;
            Username = username;
            Password = password;
        }
    }
}

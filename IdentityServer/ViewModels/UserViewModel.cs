namespace IdentityServer5.ViewModels
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Enabled { get; set; }

        public string RoleName { get; set; }
    }
}

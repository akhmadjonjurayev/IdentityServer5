namespace IdentityServer5.ViewModels
{
    public class ChangeUserPassword
    {
        public Guid UserId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}

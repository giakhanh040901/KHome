namespace EPIC.IdentityServer.Models
{
    public class PostLoginDto
    {
        private string _username;
        public string Username 
        { 
            get => _username; 
            set => _username = value?.Trim(); 
        }

        private string _password;
        public string Password 
        { 
            get => _password; 
            set => _password = value?.Trim(); 
        }
    }
}

using JustShortIt.Model;

namespace JustShortIt.Service; 

public class AuthenticationService {
    private User User { get; }

    public AuthenticationService(User user) {
        User = user;
    }

    public bool IsUser(string username, string password) {
        return string.Equals(User.Username, username, StringComparison.CurrentCultureIgnoreCase) &&
               User.Password == password;
    }
}
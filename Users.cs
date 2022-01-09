

using Models;

public static class Users
{
    private static User[] users = new []
    {
        new User("Regular", "User", "user@test.com", "qwerty"),
        new User("Admin", "User", "admin@test.com", "qwerty", true),
    };

    public static User[] GetUsers()
    {
        return users;
    }
    
    public static User? GetUserByEmail(string email)
    {
        return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
}
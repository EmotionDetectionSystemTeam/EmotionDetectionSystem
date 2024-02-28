public enum UserType
{
    Student,
    Teacher,
    Admin,
}

public static class UserTypeExtensions
{
    public static string GetStringValue(this UserType userType)
    {
        return userType switch
        {
            UserType.Student => "Student",
            UserType.Teacher => "Teacher",
            UserType.Admin   => "Admin",
            _                => throw new ArgumentOutOfRangeException(nameof(userType), userType, null)
        };
    }
}
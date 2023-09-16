namespace ContosoUniversity;

public static class Utils
{
    public static string GetLastChars(Guid token)
        => token.ToString()[^3..];
}

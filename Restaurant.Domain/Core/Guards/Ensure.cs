namespace Restaurant.Domain.Core.Guards
{
    public static class Ensure
    {
        public static void NotEmpty(string arg, string message, string argName)
        {
            if (string.IsNullOrWhiteSpace(arg))
                throw new ArgumentException(message, argName);
        }

        public static void NotEmpty(Guid arg, string message, string argName)
        {
            if (arg == Guid.Empty)
                throw new ArgumentException(message, argName);
        }

        public static void NotNull<T>(T arg, string message, string argName) where T : class
        {
            if (arg is null)
                throw new ArgumentException(message, argName);
        }
    }
}
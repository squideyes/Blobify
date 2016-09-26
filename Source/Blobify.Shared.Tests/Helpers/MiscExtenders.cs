namespace Blobify.Shared.Tests
{
    public static class MiscExtenders
    {
        public static bool IsTrimmed(this string value) =>
            value == value.Trim();
    }
}

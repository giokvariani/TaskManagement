namespace TaskManagement.API.StaticHelper
{
    public static class StaticHelper
    {
        public static Dictionary<string, string[]> HttpVerbsMap = new()
        {
            { "Update", new[] { "PUT", "PATCH" } }
        };
    }
}

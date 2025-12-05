using System.Reflection;

namespace EmployeeInformations
{
    public static class AsemblyInfoReader
    {
        public static string ApplicationVersion
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                if (version is { }) return $"v{version.Major}.{version.Minor}.{version.Build}.{version.MinorRevision}";
                return null;
            }
        }
    }
}

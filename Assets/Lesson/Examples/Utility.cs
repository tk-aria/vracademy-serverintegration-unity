
namespace VRAcademy.HttpBasic
{
    public static partial class Utility
    {
        public const string HostName = "http://localhost:8080";

        public const string LARGE_FILE_URL = "https://cdimage.ubuntu.com/releases/20.04/release/ubuntu-20.04.2-live-server-arm64.isopon";

    }

    [System.Serializable]
    public class User
    {
        public string name;
        public string email;
    }
}

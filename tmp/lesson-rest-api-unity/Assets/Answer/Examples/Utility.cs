
namespace VRAcademy.HttpBasic
{
    public static partial class Utility
    {
        public const string HostName = "http://localhost:5678";
        public const string LARGE_FILE_URL = "https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.exe";
        //public const string LARGE_FILE_URL = "https://releases.ubuntu.com/20.04.3/ubuntu-20.04.3-live-server-amd64.iso";
    }

    [System.Serializable]
    public class User
    {
        public string name;
        public string email;
    }
}

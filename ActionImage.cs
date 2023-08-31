using System.Drawing;

namespace WindownformInstallerService
{
    public static class ActionImage
    {
        public static string iconPlay = "E:\\C#_demo\\ConsoleApp_demo\\WindownformInstallerService\\Resources\\play.png";
        public static string iconStop = "E:\\C#_demo\\ConsoleApp_demo\\WindownformInstallerService\\Resources\\stop.png";
        public static string iconSettingOff = "E:\\C#_demo\\ConsoleApp_demo\\WindownformInstallerService\\Resources\\settingoff.png";
        public static Image SetImage(string path)
        {
            Image icon = Image.FromFile(path);
            Image resize = new Bitmap(icon, 15, 15);
            return resize;
        }
    }
}




using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace INIHandler
{
    public class IniHandler
    {
        public string path;
        public IniHandler(string INIPath)
        {
            this.path = INIPath;
        }

        public string IniReadValue(string Key, string Section = "Config")
        {
            StringBuilder retVal = new StringBuilder((int)byte.MaxValue);
            IniHandler.GetPrivateProfileString(Section, Key, "", retVal, (int)byte.MaxValue, this.path);
            return retVal.ToString();
        }
        public void IniWriteValue(string Key, string Value, string Section = "Config")
        {
            IniHandler.WritePrivateProfileString(Section, Key, Value, this.path);
        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);
        public List<Items> GetKeys(string category)
        {
            byte[] numArray = new byte[2048];
            IniHandler.GetPrivateProfileSection(category, numArray, 2048, this.path);
            string[] strArray = Encoding.ASCII.GetString(numArray).Trim(new char[1]).Split(new char[1]);
            List<Items> list = new List<Items>();
            if (strArray[0] != "")
            {
                foreach (string str in strArray)
                    list.Add(new Items()
                    {
                        Key = str.Split('=')[0],
                        Value = str.Split('=')[1]
                    });
            }
            return list;
        }
        public List<string> GetKeysString(string category)
        {
            byte[] numArray = new byte[2048];
            IniHandler.GetPrivateProfileSection(category, numArray, 2048, this.path);
            string[] strArray = Encoding.ASCII.GetString(numArray).Trim(new char[1]).Split(new char[1]);
            List<string> list = new List<string>();
            if (strArray[0] != "")
            {
                foreach (string str in strArray)
                    list.Add(str.Split('=')[0]);
            }
            return list;
        }
    }
    public class Items
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

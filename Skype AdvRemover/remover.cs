using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Skype_AdvRemover
{
    public class SARemover
    {

        private List<SkypeProfile> _skype_profiles = new List<SkypeProfile>();
        public List<SkypeProfile> SkypeProfiles
        {
            get { return _skype_profiles; }
        }
        public bool SkypeRunning { get; private set; }
        public Process PID { get; private set; }
        public string path { get; private set; }
        private void GetProfileData()
        {
        }
        public SARemover()
        {
            IsSkypeRunning();
            GetSkypeInstallPath();
            GetSkypeProfiles();
        }
        public SkypeProfile GetSkypeProfile(string name)
        {
            return _skype_profiles.Find(n => n.Name == name);
        }
        private void GetSkypeProfiles()
        {
            DirectoryInfo _skypedata = new DirectoryInfo(GetAppData() + Path.DirectorySeparatorChar + "Skype");
            foreach (var item in _skypedata.EnumerateDirectories())
            {
                if (File.Exists(item.FullName + Path.DirectorySeparatorChar + "config.xml"))
                {
                    _skype_profiles.Add(new SkypeProfile(item.Name));
                }
            }
        }

        private void GetSkypeInstallPath()
        {
            if (SkypeRunning)
                path = PID.MainModule.FileName;
            else
            {
                try
                {
                    DirectoryInfo ProgramFiles = new DirectoryInfo(ProgramFilesDir() + @"\Skype");
                    path = ProgramFiles.EnumerateFiles("Skype.exe", SearchOption.AllDirectories).First().FullName;
                    
                }

                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        private void IsSkypeRunning()
        {
            Process[] procs = Process.GetProcessesByName("Skype");
            if (procs[0] != null)
            {
                SkypeRunning = true;
                PID = procs[0];
            }
            else
                SkypeRunning = false;
        }
        public static string GetAppData()
        {
            return Environment.GetEnvironmentVariable("Appdata");
        }
        public static string ProgramFilesDir()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        public void Restart()
        {
            PID.Kill();

            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = path;
            process.Start();
        }
    }

    public class SkypeProfile
    {
        public string Name { get; set; }
        public SkypeProfile(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }

        public void RemoveAdv()
        {
            ChangeConfig();
            ForbidAdvSite();
        }

        private void ForbidAdvSite()
        {
            RegistryKey key = Registry.CurrentUser;
            if (key.GetValue(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains\skype.com\apps") == null)
            {
                key = key.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains\skype.com\apps");
                key.SetValue("*", 4, RegistryValueKind.DWord);
            }
        }

        private void ChangeConfig()
        {
            var sp = Path.DirectorySeparatorChar;
            string _path = SARemover.GetAppData() + sp + "Skype" + sp + Name + sp + "config.xml";
            string adv_match = "(?<=<AdvertPlaceholder>)\\d(?=</AdvertPlaceholder>)";
            string content = String.Empty;

            using (StreamReader reader = new StreamReader(_path))

            {
                content = reader.ReadToEnd();
            }

            Match match = Regex.Match(content, adv_match);

            if (match.Success == false)
            {
                content = content.Insert(content.IndexOf("<General>") + 9, Environment.NewLine + "\t  <AdvertPlaceholder>0</AdvertPlaceholder>");
            }
            else
            {
                if (match.Value != "0")
                    content = Regex.Replace(content, adv_match, "0");
            }


            using (StreamWriter writer = new StreamWriter(_path))
            {
                writer.Write(content);
                writer.Close();
            }
        }

        public static void ReplaceInFile(
                      string filePath, string searchText, string replaceText)
        {

            var content = string.Empty;
            using (StreamReader reader = new StreamReader(filePath))
            {
                content = reader.ReadToEnd();
                reader.Close();
            }

            content = Regex.Replace(content, searchText, replaceText);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(content);
                writer.Close();
            }
        }

    }
}

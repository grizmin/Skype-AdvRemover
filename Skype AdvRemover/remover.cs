using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skype_AdvRemover
{
    public class SARemover
    {

        public List<string> SkypeProfiles { get; set; }
        private void GetProfileData()
        {

        }
        private string FindSkypeProfilePath()
        {
            return "a";

        }

        public void ChangeAdvProperty(string profileName)
        {

        }

        public List<string> Restart()
        {
            List<string> res = new List<string>();

            Process[] procs = Process.GetProcessesByName("Skype");
            procs[0].Kill();

            return res;
        }
    }
}

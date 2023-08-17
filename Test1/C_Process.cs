using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Test1
{
    public class C_Process
    {
        Process process;
        private string _filename = string.Empty;
        
        public C_Process(string filename)
        {
            _filename = filename;
        }

        public void Run()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = _filename;
                process = Process.Start(startInfo);
            }
            catch (Exception ex)
            {

            }
        }
    }
}

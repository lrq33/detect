using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace testpointer
{
    internal class Program
    {
        static async Task Main()
        {
            string processName = "chrome";

            while (true)
            {
                await PrintTabTitlesAsync(processName);
                await Task.Delay(9000); 
            }
        }

        private static async Task PrintTabTitlesAsync(string processName)
        {
            Process[] chromeProcesses = Process.GetProcessesByName(processName);

            foreach (Process process in chromeProcesses)
            {
                IntPtr hwnd = process.MainWindowHandle;
                string title = await GetWindowTitleAsync(hwnd);
                Console.WriteLine($"Tab Title ({process.Id}): {title}");
            }
        }

        private static Task<string> GetWindowTitleAsync(IntPtr hWnd)
        {
            return Task.Run(() =>
            {
                StringBuilder title = new StringBuilder(256);
                GetWindowText(hWnd, title, title.Capacity);
                return title.ToString();
            });
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    }
}

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Yournamespace
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            string flagFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Obs_settings.txt");

            if (!File.Exists(flagFilePath))
            {
                MessageBox.Show("Error: Unauthorized access. You didn't pass the face verification.", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(0);
            }

            // Read the flag content
            string fileContent = File.ReadAllText(flagFilePath).Trim();

            if (fileContent == "Denied")
            {
                MessageBox.Show("Error: Access Denied. You failed face verification. go fuck yourself if your here to crack", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(0);
            }

            if (fileContent == "verified")
            {
                MessageBox.Show("Face verification successful. Launching application...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }

            Environment.Exit(0);
        }
    }
}

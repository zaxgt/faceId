Hi please give credit if you fork this or use it for another project im just a 14yr making ts so it would be well appreciated 
and if you need help during any prosess or the instalation/setup to get your project started reach out to me on discord zaxandmilo 


there is a second step
PLEASE READ THIS IT WONT WORK WITHOUT 

you need to add this code to youre main program that you want the face.id to verify 

EXAMPLE WINFORMS
IN PROGRAM.CS

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


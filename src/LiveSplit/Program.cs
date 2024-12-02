using System;
using System.IO;
using System.Windows.Forms;

using LiveSplit.View;

namespace LiveSplit;

internal static class Program
{
    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
#if !DEBUG
        Application.ThreadException += (sender, e) =>
        {
            Options.Log.Error(e.Exception);
            MessageBox.Show($"LiveSplit has crashed due to the following reason:\n\n{e.Exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        };
#endif

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Environment.CurrentDirectory = Path.GetDirectoryName(Application.ExecutablePath);

#if !DEBUG
        Options.FiletypeRegistryHelper.RegisterFileFormatsIfNotAlreadyRegistered();
#endif

        string splitsPath = null;
        string layoutPath = null;

        for (int i = 0; i < args.Length - 1; i += 2)
        {
            switch (args[i])
            {
                case "-s": splitsPath = args[i + 1]; break;
                case "-l": layoutPath = args[i + 1]; break;
            }
        }

        Application.Run(new TimerForm(splitsPath, layoutPath));
    }
}

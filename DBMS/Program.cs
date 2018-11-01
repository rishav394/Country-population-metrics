using System;
using System.Windows.Forms;

namespace DBMS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new OptionForm());
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"An exception has occurred....");
                Console.ResetColor();

                if (!OptionForm.IsSoftwareInstalled("Mongo"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(
                        @"Looks like MongoDB is not installed. Please install it manually.....");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(e);
                }

                Environment.Exit(1);
            }
        }
    }
}

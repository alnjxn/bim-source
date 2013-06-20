using System;
using System.IO;
using System.Reflection;

namespace BIMSource.SPWriter.Updater
{
    class Program
    {
        private static string sourcePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string dirWin7 = Environment.GetEnvironmentVariable("UserProfile") + "\\AppData\\Roaming\\";
        private static string dirXP = Environment.GetEnvironmentVariable("UserProfile") + "\\Application Data\\";

        public static void Main()
        {
            // Test for Win7
            if (Directory.Exists(dirWin7))
            {
                doCopy(dirWin7);
                return;
            }
            // Test for XP
            if (Directory.Exists(dirXP))
            {
                doCopy(dirXP);
                return;
            }
            // Warn on Failure

        }
        /// <summary>
        /// Copy the Files
        /// </summary>
        /// <param name="destination"></param>
        /// <remarks></remarks>
        private static void doCopy(string destination)
        {
            // Addin path
            string pathAddin = destination + "Autodesk\\Revit\\Addins\\2014";
            // Get Files
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fiAddin = di.GetFiles("*.addin");
            FileInfo[] fiDll = di.GetFiles("*.dll");
            FileInfo[] fiPng = di.GetFiles("*.png");


            foreach (FileInfo fiAddinNext in fiAddin)
            {
                try
                {
                    fiAddinNext.CopyTo(pathAddin + "\\" + fiAddinNext.Name, true);
                }
                catch (Exception)
                {
                    // Quiet Fail
                }
            }
            foreach (FileInfo fiDllNext in fiDll)
            {
                try
                {
                    fiDllNext.CopyTo(pathAddin + "\\" + fiDllNext.Name, true);
                }
                catch (Exception)
                {
                    // Quiet Fail
                }
            }
            foreach (FileInfo fiPngNext in fiPng)
            {
                try
                {
                    fiPngNext.CopyTo(pathAddin + "\\" + fiPngNext.Name, true);
                }
                catch (Exception)
                {
                    // Quiet Fail
                }
            }
        }
    }
}

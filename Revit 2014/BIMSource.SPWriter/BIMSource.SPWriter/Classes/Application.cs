using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BIMSource.SPWriter
{
    /// <summary>
    /// Revit 2014 API Application Class
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Application : IExternalApplication
    {
        // ExternalCommands assembly path
        static string AddInPath = typeof(Application).Assembly.Location;

        // Button icons directory
        static string ButtonIconsFolder = Path.GetDirectoryName(AddInPath);

        //public static UIControlledApplication _cachedUiCtrApp;

        /// <summary>
        /// Fires off when Revit Session Starts
        /// </summary>
        /// <param name="application">An object that is passed to the external application which contains the controlled application.</param>
        /// <returns>Return the status of the external application. A result of Succeeded means that the external application successfully started. Cancelled can be used to signify that the user cancelled the external operation at some point. If false is returned then Revit should inform the user that the external application failed to load and the release the internal reference.</returns>
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // create customer Ribbon Items
                CreateRibbonPanel(application);

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ribbon");

                return Autodesk.Revit.UI.Result.Failed;
            }
        }

        /// <summary>
        /// Fires off when Revit Session Ends
        /// </summary>
        /// <param name="application">An object that is passed to the external application which contains the controlled application.</param>
        /// <returns>Return the status of the external application. A result of Succeeded means that the external application successfully shutdown. Cancelled can be used to signify that the user cancelled the external operation at some point. If false is returned then the Revit user should be warned of the failure of the external application to shut down correctly.</returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            try
            {

                //TODO: add you code below.


                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return Result.Failed;
            }
        }

        private RibbonPanel CreateRibbonPanel(UIControlledApplication application)
        {
            //System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            // create a Ribbon panel which contains three stackable buttons and one single push button.
            string firstPanelName = "BIM Source";
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(firstPanelName);

            //Create First button:
            PushButtonData pbDataAddParameterToFamily = new PushButtonData("AddParameterToFamily", "Bind Parameters \nTo Family",
                AddInPath, "BIMSource.NewSPWriter.Commands");
            PushButton pbAddParameterToFamily = ribbonPanel.AddItem(pbDataAddParameterToFamily) as PushButton;
            pbAddParameterToFamily.ToolTip = "Bind Shared Parameters From External File to Family";
            //pbAddParameterToFamily.LargeImage = GetEmbeddedImage(myAssembly, "BIMSource.SPWriter.32.png");
            //pbAddParameterToFamily.Image = GetEmbeddedImage(myAssembly, "BIMSource.SPWriter.16.png");
            pbAddParameterToFamily.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "BIMSource.SPWriter.32.png"), UriKind.Absolute));
            pbAddParameterToFamily.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "BIMSource.SPWriter.16.png"), UriKind.Absolute));

            return ribbonPanel;
        }
        
        private ImageSource GetEmbeddedImage(System.Reflection.Assembly app, string imageName)
        {
            System.IO.Stream file = app.GetManifestResourceStream("BIMSource.NewSPWriter." + imageName);
         PngBitmapDecoder bd = new PngBitmapDecoder(file, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

         return bd.Frames[0];
       }
    }
}

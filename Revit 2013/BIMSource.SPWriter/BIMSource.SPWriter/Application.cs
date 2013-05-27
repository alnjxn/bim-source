using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace BIMSource.SPWriter
{
  /// <summary>
  /// Revit 2013 API Application Class
  /// </summary>
  [Transaction(TransactionMode.Manual)]
  class Application : IExternalApplication
  {
    /// <summary>
    /// Fires off when Revit Session Starts
    /// </summary>
    /// <param name="application">An object that is passed to the external application which contains the controlled application.</param>
    /// <returns>Return the status of the external application. A result of Succeeded means that the external application successfully started. Cancelled can be used to signify that the user cancelled the external operation at some point. If false is returned then Revit should inform the user that the external application failed to load and the release the internal reference.</returns>
    public Result OnStartup(UIControlledApplication application)
    {
      try
      {
        // Add your code here

        // Return Success
        return Result.Succeeded;

      }
      catch
      {
        // Return Failure
        return Result.Failed;

      }
    }

    /// <summary>
    /// Fires off when Revit Session Ends
    /// </summary>
    /// <param name="application">An object that is passed to the external application which contains the controlled application.</param>
    /// <returns>Return the status of the external application. A result of Succeeded means that the external application successfully shutdown. Cancelled can be used to signify that the user cancelled the external operation at some point. If false is returned then the Revit user should be warned of the failure of the external application to shut down correctly.</returns>
    public Result OnShutdown(UIControlledApplication application)
    {
      // Return Success
      return Result.Succeeded;
    }
  }
}

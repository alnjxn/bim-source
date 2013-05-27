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
  /// Revit 2013 Command Class
  /// </summary>
  /// <remarks></remarks>
  [Transaction(TransactionMode.Manual)]
  public class Commands : IExternalCommand
  {
    // the active Revit application
    private Autodesk.Revit.UI.UIApplication m_app;

    /// <summary>
    /// Command Entry Point
    /// </summary>
    /// <param name="commandData">Input argument providing access to the Revit application and documents</param>
    /// <param name="message">Return message to the user in case of error or cancel</param>
    /// <param name="elements">Return argument to highlight elements on the graphics screen if Result is not Succeeded.</param>
    /// <returns>Cancelled, Failed or Succeeded</returns>
    public Result Execute(ExternalCommandData commandData,
                            ref string message,
                            ElementSet elements)
    {
      m_app = commandData.Application;

      try
      {
        bool succeeded = AddParameters();

        if (succeeded)
        {
          MessageBox.Show("Done. Binding Shared Parameters Succeeded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return Autodesk.Revit.UI.Result.Succeeded;               
        }
        else
        {
          MessageBox.Show("Failed", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return Autodesk.Revit.UI.Result.Failed;
        }
      }
      catch (Exception ex)
      {
        // Failure Message
        message = ex.Message;
        return Result.Failed;
      }
    }

    private bool AddParameters()
    {
      Document doc = m_app.ActiveUIDocument.Document;
      if (null == doc)
      {
        MessageBox.Show("There's no available document.");
        return false;
      }

      if (!doc.IsFamilyDocument)
      {
        MessageBox.Show("The active document is not a family document.");
        return false;
      }

      ParameterAssigner pa = new ParameterAssigner(m_app.Application, doc);
      // the parameters to be added are defined and recorded in a text file, read them from that file and load to memory
      bool succeeded = pa.LoadSharedParameterFile();
      if (!succeeded)
      {
        return false;
      }

      Transaction t = new Transaction(doc, "Bind Shared Parameters");
      t.Start();
      succeeded = pa.BindSharedParameters();
      if (succeeded)
      {
        t.Commit();
        return true;
      }
      else
      {
        t.RollBack();
        return false;
      }
    }
  }
}

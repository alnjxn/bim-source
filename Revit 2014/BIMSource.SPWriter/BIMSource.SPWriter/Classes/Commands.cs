using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Windows.Forms;

namespace BIMSource.SPWriter
{

    /// <summary>
    /// Revit 2014 Command Class
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

            Document doc = m_app.ActiveUIDocument.Document;
            if (null == doc)
            {
                MessageBox.Show("There's no available document.");
                return Result.Cancelled;
            }

            if (!doc.IsFamilyDocument)
            {
                MessageBox.Show("The active document is not a family document.");
                return Result.Cancelled;
            }

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
                    //MessageBox.Show("Failed", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Autodesk.Revit.ApplicationServices.Application app = m_app.Application;
            ParameterAssigner pa = new ParameterAssigner(app, doc);

            MainWindow mw = new MainWindow();
            mw.ShowDialog();
            if (mw.DialogResult == true)
            {
                string filepath = ParameterSettings.filePath;
                bool isinstance = ParameterSettings.isInstance;

                Transaction t = new Transaction(doc, "Bind Shared Parameters");
                t.Start();
                bool succeeded = pa.BindSharedParameters(filepath, isinstance);
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
            else
            {
                return false;
            }
        }
    }
}
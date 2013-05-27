using System;
using System.Text;
using System.Linq;
using System.Xml;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace BIMSource.SPWriter
{
  class ParameterAssigner
  {
    private Autodesk.Revit.ApplicationServices.Application m_app;
    private FamilyManager m_manager = null;

    private DefinitionFile m_sharedFile;
    private string m_sharedFilePath = string.Empty;

    public ParameterAssigner(Autodesk.Revit.ApplicationServices.Application app, Document doc)
    {
        m_app = app;
        m_manager = doc.FamilyManager;
    }

    public bool LoadSharedParameterFile()
    {
      string myDocsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      OpenFileDialog ofd = new OpenFileDialog();

      ofd.InitialDirectory = myDocsFolder;
      ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
      ofd.FilterIndex = 1;
      ofd.RestoreDirectory = true;
      ofd.Title = "Please Select the Shared Parameter File";

      if (ofd.ShowDialog() == DialogResult.OK)
      {
        m_sharedFilePath = ofd.FileName;
        if (!File.Exists(m_sharedFilePath))
        {
          return true;
        }

        m_app.SharedParametersFilename = m_sharedFilePath;
        try
        {
          m_sharedFile = m_app.OpenSharedParameterFile();
        }
        catch (System.Exception e)
        {
          MessageBox.Show(e.Message);
          return false;
        }
        return true;
      }
      else
      {
        return false;
      }
    }

    public bool BindSharedParameters()
    {
      if (File.Exists(m_sharedFilePath) &&
          null == m_sharedFile)
      {
        MessageBox.Show("SharedParameter.txt has an invalid format.");
        return false;
      }

      foreach (DefinitionGroup group in m_sharedFile.Groups)
      {
        foreach (ExternalDefinition def in group.Definitions)
        {
          // check whether the parameter already exists in the document
          FamilyParameter param = m_manager.get_Parameter(def.Name);
          if (null != param)
          {
            continue;
          }

          BuiltInParameterGroup bpg = BuiltInParameterGroup.INVALID;
          try
          {
            if (def.OwnerGroup.Name == "Dimensions")
            {
              bpg = BuiltInParameterGroup.PG_GEOMETRY;
            }
            else if (def.OwnerGroup.Name == "Electrical")
            {
              bpg = BuiltInParameterGroup.PG_ELECTRICAL;
            }
            else if (def.OwnerGroup.Name == "Mechanical")
            {
              bpg = BuiltInParameterGroup.PG_MECHANICAL;
            }
            else if (def.OwnerGroup.Name == "Identity Data")
            {
              bpg = BuiltInParameterGroup.PG_IDENTITY_DATA;
            }
            else if (def.OwnerGroup.Name == "Electrical - Loads")
            {
              bpg = BuiltInParameterGroup.PG_ELECTRICAL_LOADS;
            }
            else if (def.OwnerGroup.Name == "Mechanical - Air Flow")
            {
              bpg = BuiltInParameterGroup.PG_MECHANICAL_AIRFLOW;
            }
            else if (def.OwnerGroup.Name == "Energy Analysis")
            {
              bpg = BuiltInParameterGroup.PG_ENERGY_ANALYSIS;
            }
            else if (def.OwnerGroup.Name == "Mechanical - Loads")
            {
              bpg = BuiltInParameterGroup.PG_MECHANICAL_LOADS;
            }
            else if (def.OwnerGroup.Name == "Structural")
            {
              bpg = BuiltInParameterGroup.PG_STRUCTURAL;
            }
            else if (def.OwnerGroup.Name == "Plumbing")
            {
              bpg = BuiltInParameterGroup.PG_PLUMBING;
            }
            else if (def.OwnerGroup.Name == "Green Building Properties")
            {
              bpg = BuiltInParameterGroup.PG_GREEN_BUILDING;
            }
            else if (def.OwnerGroup.Name == "Materials and Finishes")
            {
              bpg = BuiltInParameterGroup.PG_MATERIALS;
            }
            else if (def.OwnerGroup.Name == "Other")
            {
              bpg = BuiltInParameterGroup.INVALID;
            }
            else if (def.OwnerGroup.Name == "Construction")
            {
              bpg = BuiltInParameterGroup.PG_CONSTRUCTION;
            }
            else
            {
              bpg = BuiltInParameterGroup.INVALID;
            }

            m_manager.AddParameter(def, bpg, true);

          }
          catch (System.Exception e)
          {
            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
          }
        }
      }
      return true;
    }
  }
}

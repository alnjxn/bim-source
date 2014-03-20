using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BIMSource.SPWriter
{
    class ParameterAssigner
    {
        private Autodesk.Revit.ApplicationServices.Application m_app;
        private FamilyManager m_manager = null;

        public ParameterAssigner(Autodesk.Revit.ApplicationServices.Application app, Document doc)
        {
            m_app = app;
            m_manager = doc.FamilyManager;
        }

        public bool BindSharedParameters(string filePath, bool isInstance)
        {
            string filepath = filePath;
            bool isinstance = isInstance;
            DefinitionFile file = null;

            m_app.SharedParametersFilename = filepath;

            try
            {
                file = m_app.OpenSharedParameterFile();
            }
            catch
            {
                MessageBox.Show("File has an invalid format.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            if (File.Exists(filepath) &&
                null == file)
            {
                MessageBox.Show("File has an invalid format.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            } 

            foreach (DefinitionGroup group in file.Groups)
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
                        switch (def.OwnerGroup.Name)
                        {
                            case "Dimensions":
                                bpg = BuiltInParameterGroup.PG_GEOMETRY;
                                break;
                            case "Graphics":
                                bpg = BuiltInParameterGroup.PG_GRAPHICS;
                                break;
                            case "Model Properties":
                                bpg = BuiltInParameterGroup.PG_ADSK_MODEL_PROPERTIES;
                                break;
                            case "Overall Legend":
                                bpg = BuiltInParameterGroup.PG_OVERALL_LEGEND;
                                break;
                            case "Electrical":
                                bpg = BuiltInParameterGroup.PG_ELECTRICAL;
                                break;
                            case "Mechanical":
                                bpg = BuiltInParameterGroup.PG_MECHANICAL;
                                break;
                            case "Electrical - Lighting":
                                bpg = BuiltInParameterGroup.PG_ELECTRICAL_LIGHTING;
                                break;
                            case "Identity Data":
                                bpg = BuiltInParameterGroup.PG_IDENTITY_DATA;
                                break;
                            case "Data":
                                bpg = BuiltInParameterGroup.PG_DATA;
                                break;
                            case "Electrical - Loads":
                                bpg = BuiltInParameterGroup.PG_ELECTRICAL_LOADS;
                                break;
                            case "Mechanical - Air Flow":
                                bpg = BuiltInParameterGroup.PG_MECHANICAL_AIRFLOW;
                                break;
                            case "Energy Analysis":
                                bpg = BuiltInParameterGroup.PG_ENERGY_ANALYSIS;
                                break;
                            case "Photometrics":
                                bpg = BuiltInParameterGroup.PG_LIGHT_PHOTOMETRICS;
                                break;
                            case "Mechanical - Loads":
                                bpg = BuiltInParameterGroup.PG_MECHANICAL_LOADS;
                                break;
                            case "Structural":
                                bpg = BuiltInParameterGroup.PG_STRUCTURAL;
                                break;
                            case "Plumbing":
                                bpg = BuiltInParameterGroup.PG_PLUMBING;
                                break;
                            case "Green Building Properties":
                                bpg = BuiltInParameterGroup.PG_GREEN_BUILDING;
                                break;
                            case "Materials and Finishes":
                                bpg = BuiltInParameterGroup.PG_MATERIALS;
                                break;
                            case "Other":
                                bpg = BuiltInParameterGroup.INVALID;
                                break;
                            case "Construction":
                                bpg = BuiltInParameterGroup.PG_CONSTRUCTION;
                                break;  
                            case "Phasing":
                                bpg = BuiltInParameterGroup.PG_PHASING;
                                break;
                            default:
                                bpg = BuiltInParameterGroup.INVALID;
                                break;
                        }

                        m_manager.AddParameter(def, bpg, isinstance);
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

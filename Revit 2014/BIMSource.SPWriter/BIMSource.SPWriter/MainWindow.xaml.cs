using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Autodesk.Revit.DB;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BIMSource.SPWriter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_OpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            this.TextBox_FileName.Text = LoadFile();
            if (this.TextBox_FileName.Text != null)
            {
                this.Button_Go.IsEnabled = true;
            }
        }

        private string LoadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            
            openFileDialog.InitialDirectory = directory;
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "Please Select the Shared Parameter File";

            string filename = null;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = openFileDialog.FileName;
                if(File.Exists(filename))
                {
                    return filename;
                }
                else
                {
                    return null;
                }
            }
            return filename;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Button_Go_Click(object sender, RoutedEventArgs e)
        {
            ParameterSettings.filePath = this.TextBox_FileName.Text;
            if (this.RadioButton_Instance.IsChecked == true)
            {
                ParameterSettings.isInstance = true;
            }
            else
            {
                ParameterSettings.isInstance = false;
            }
            DialogResult = true;
        }
    }
}

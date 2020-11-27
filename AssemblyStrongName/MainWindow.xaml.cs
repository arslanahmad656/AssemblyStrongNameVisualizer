using Microsoft.Win32;
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
using System.Reflection;
using System.IO;

namespace AssemblyStrongName
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

        private void Btn_Browse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filePath = GetFilePath();
                var (name, version, culture, key, fullName) = GetAssemblyInformation(filePath);
                Txt_Output.Text = $"Name: {name}{Environment.NewLine}";
                Txt_Output.Text += $"Version: {version}{Environment.NewLine}";
                Txt_Output.Text += $"Culture: {culture}{Environment.NewLine}";
                Txt_Output.Text += $"Public Key: {key}{Environment.NewLine}";
                Txt_Output.Text += $"Full Name: {fullName}{Environment.NewLine}";
                Txt_Output.Text += $"Path: {filePath}{Environment.NewLine}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetFilePath()
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Assembly Files (*.exe, *.dll)|*.exe;*.dll|All Files|*.*"
            };

            if (fileDialog.ShowDialog() == true)
            {
                return fileDialog.FileName;
            }

            return "";
        }

        private (string name, Version version, string culture, string key, string fullName) GetAssemblyInformation(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"File ${file} does not exist.");
            }

            var assembly = Assembly.ReflectionOnlyLoadFrom(file);
            var name = assembly.GetName();
            var pKey = BitConverter.ToString(name.GetPublicKeyToken());
            return (name.Name, name.Version, name.CultureName, pKey, name.FullName);
        }
    }
}

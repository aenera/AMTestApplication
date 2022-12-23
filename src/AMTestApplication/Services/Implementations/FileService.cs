using AMTestApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMTestApplication.Services.Implementations
{
    public class FileService : IFileService
    {

        public string SelectFile()
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".csv"; 
            dialog.Filter = "Text documents (.csv)|*.csv"; 
            dialog.Multiselect = false;

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            if (result != null && result == true)
                return dialog.FileName;

            return "";
        }
    }
}

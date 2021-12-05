using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MeadowSolar
{
    internal class FileWindowHandler
    {
        private JsonTextReader reader;

        public string file { get; set; }
        public string JsonFile { get; set; }

        public void SelectFile()
        {
            //bool fileSelected = false;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JSON Files (*.json)|*.json";

            if (openFileDialog1.ShowDialog() == true)
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                fs.Close();
                file = openFileDialog1.FileName;
            }
        }

        public void deSerializer()
        {
            JsonFile = File.ReadAllText(file);
            System.Diagnostics.Debug.WriteLine(JsonFile);
        }
    }

    //public class Values
    //{
    //    public
    //}
}
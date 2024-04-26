using SearchApp.Models;
using System.IO;
using System.Xml.Serialization;

namespace SearchApp.Services
{
    class MainModelSerializer
    {
        private readonly string _filePath;

        public MainModelSerializer(string filePath)
        {
            _filePath = filePath;
        }

        public void Serialize(MainModel mainModel)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MainModel));
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                serializer.Serialize(writer, mainModel);
            }
        }

        public MainModel Deserialize()
        {
            if (!File.Exists(_filePath))
            {
                return new MainModel(); 
            }

            XmlSerializer serializer = new XmlSerializer(typeof(MainModel));
            using (StreamReader reader = new StreamReader(_filePath))
            {
                return (MainModel)serializer.Deserialize(reader);
            }
        }
    }
}

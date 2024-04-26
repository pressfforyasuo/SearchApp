using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SearchApp.Models
{
    public class DirectoryNode
    {
        public string Name { get; set; }
        public bool Show {  get; set; }

        public ObservableCollection<ObservableCollection<DirectoryNode>> Subdirectories { get; set; }

        public DirectoryNode() { }

        public DirectoryNode(string name)
        {
            Name = name;
            Subdirectories = new ObservableCollection<ObservableCollection<DirectoryNode>>();
        }
    }
}

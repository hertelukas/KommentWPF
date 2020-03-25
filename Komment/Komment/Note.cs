using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Komment
{
    public class Note
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string LastEdited { get; set; }
        public List<string> Folders = new List<string>();
        public string _id { get; set; }

        public Note(string _title)
        {
            Title = _title;
        }
    }
}

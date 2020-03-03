using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komment
{
    public class Note
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime LastEdited { get; set; }
        public string[] Folders;
        public string _id { get; set; }

        public Note(string _title)
        {
            Title = _title;
        }
    }
}

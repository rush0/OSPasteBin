using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPasteBin.BusinessObjects
{
    public class PasteNote
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Post { get; set; }
        public string Language { get; set; }
        public string UserName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPasteBin.BusinessObjects
{
    public interface IPasteBinDAL
    {
        PasteNote GetPasteNote(int id);
        IEnumerable<PasteNote> GetAllPasteNotes();
        IEnumerable<PasteNote> GetRecentPasteNotes(DateTime starting);
        bool RemovePasteNote(int id);
    }
}

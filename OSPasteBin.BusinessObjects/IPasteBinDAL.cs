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
        PasteNote AddPostNote(PasteNote newNote); 
        IEnumerable<PasteNote> GetAllPasteNotes();
        IEnumerable<PasteNote> GetPasteNotesWithKeyWord(string keyword);
        IEnumerable<PasteNote> GetRecentPasteNotes(DateTime starting);
        IEnumerable<PasteNote> GetPasteNotesForUser(string username);
        bool RemovePasteNote(int id);
        
    }
}

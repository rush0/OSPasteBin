using OSPasteBin.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPasteBin.DAL
{
    public class PasteBinSqlDAL : IPasteBinDAL
    {
        private string _connectionString;

        #region CTOR
        public PasteBinSqlDAL(string connectionString)
        {
        }
        #endregion

        public PasteNote GetPasteNote(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PasteNote> GetAllPasteNotes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PasteNote> GetRecentPasteNotes(DateTime starting)
        {
            throw new NotImplementedException();
        }

        public bool RemovePasteNote(int id)
        {
            throw new NotImplementedException();
        }
    }
}

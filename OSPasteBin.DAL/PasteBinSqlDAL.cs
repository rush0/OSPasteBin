using OSPasteBin.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPasteBin.DAL
{
    public class PasteBinSqlDAL : IPasteBinDAL
    {
        private SQLDAL _sqlDal;

        private const string _getPasteNoteParameterName = "";
        private const string _removePasteNoteParameterName = "";
        private const string _getRecentPasteNotesParameterName = "";

        #region CTOR
        public PasteBinSqlDAL(string connectionString)
        {
            _sqlDal = new SQLDAL(connectionString);
        }
        #endregion

        public PasteNote GetPasteNote(int id)
        {
            PasteNote note = null;
            List<SqlParameter> procedureParameters = new List<SqlParameter>();
            procedureParameters.Add(new SqlParameter("@Id", id));

            using(IDataReader reader = _sqlDal.ExecuteQuery(_getPasteNoteParameterName, procedureParameters))
	        {
                while (reader.Read())
	            {
                    note = new PasteNote{
                         Id = (int)reader[0],
                         Title = (string)reader[1],
                         Description = (string)reader[2],
                         Language = (string)reader[3],
                         Post = (string)reader[4],
                         UserId = (int)reader[5]
                    };
	            }
	        }

            return note;
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

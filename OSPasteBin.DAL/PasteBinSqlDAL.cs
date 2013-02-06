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

        private const string _getPasteNoteProcedureName = "GetPasteNote";
        private const string _addPasteNoteProcedureName = "AddPasteNote";
        private const string _removePasteNoteProcedureName = "RemovePasteNote";
        private const string _getRecentPasteNotesProcedureName = "GetRecentPasteNotes";
        private const string _getPasteNotesForUserProcedureName = "GetUserPasteNotesS";
        private string _getAllPasteNotesProcedureName = "GetAllPasteNotes";


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

            using (IDataReader reader = _sqlDal.ExecuteQuery(_getPasteNoteProcedureName, procedureParameters))
            {
                while (reader.Read())
                {
                    note = new PasteNote
                    {
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
            List<PasteNote> notes = new List<PasteNote>();

            using (IDataReader reader = _sqlDal.ExecuteQuery(_getAllPasteNotesProcedureName))
            {
                while (reader.Read())
                {
                    notes.Add(new PasteNote
                    {
                        Id = (int)reader[0],
                        Title = (string)reader[1],
                        Description = (string)reader[2],
                        Language = (string)reader[3],
                        Post = (string)reader[4],
                        UserId = (int)reader[5]
                    });
                }
            }

            return notes;
        }

        public IEnumerable<PasteNote> GetRecentPasteNotes(DateTime starting)
        {
            throw new NotImplementedException();
        }

        public bool RemovePasteNote(int id)
        {
            bool success = false;
            if (id != null && id >= 0)
            {
                List<SqlParameter> procedureParameters = new List<SqlParameter>();
                procedureParameters.Add(new SqlParameter("@Id", id));

                using (IDataReader reader = _sqlDal.ExecuteQuery(_removePasteNoteProcedureName, procedureParameters))
                {
                    success = reader.RecordsAffected > 0;
                }
            }
            return success;
        }


        public PasteNote AddPostNote(PasteNote newNote)
        {
            PasteNote note = null;
            List<SqlParameter> procedureParameters = new List<SqlParameter>();
            procedureParameters.Add(new SqlParameter("@Title", newNote.Title));
            procedureParameters.Add(new SqlParameter("@Description", newNote.Description));
            procedureParameters.Add(new SqlParameter("@Post", newNote.Post));
            procedureParameters.Add(new SqlParameter("@Language", newNote.Language));

            using (IDataReader reader = _sqlDal.ExecuteQuery(_addPasteNoteProcedureName, procedureParameters))
            {
                while (reader.Read())
                {
                    note = new PasteNote
                    {
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
    }
}

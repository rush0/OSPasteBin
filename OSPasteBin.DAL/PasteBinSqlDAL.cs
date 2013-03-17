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
    /// <summary>
    /// MSSQL Data Access Layer for PasteBin Application.
    /// </summary>
    public class PasteBinSqlDAL : IPasteBinDAL
    {

        #region DBColumns
        private const string _columnId = "Id";
        private const string _columnTitle = "Title";
        private const string _columnDescription = "Description";
        private const string _columnLanguage = "Language";
        private const string _columnPost = "Post";
        private const string _columnUsername = "Username";
        private const string _columnCreated = "Created";
        private const string _columnTagName = "Title";
        private const string _columnNoteTagId = "NoteId";
        private const string _columnNoteTagName = "Tag";

        #endregion

        #region Fields - Private
        private SQLDAL _sqlDal;
        private const string _noteTagsTableName = "NoteTags";
        private const string _getPasteNoteProcedureName = "GetPasteNote";
        private const string _addPasteNoteProcedureName = "AddPasteNote";
        private const string _removePasteNoteProcedureName = "RemovePasteNote";
        private const string _getRecentPasteNotesProcedureName = "GetRecentPasteNotes";
        private const string _getPasteNotesForUserProcedureName = "GetUserPasteNotes";
        private const string _getAllPasteNotesProcedureName = "GetAllPasteNotes";
        private const string _getPasteNotesWithKeywordProcedureName = "GetKeywordPasteNotes";
        private const string _getAllTagsProcedureName = "GetTags";
        private const string _getTagsForNoteProcedureName = "GetTagsByNoteId";
        private const string _addTagsForNoteProcedureName = "AddTagsForNote";
        #endregion

        #region CTOR
        public PasteBinSqlDAL(string connectionString)
        {
            _sqlDal = new SQLDAL(connectionString);
        }
        #endregion

        #region Methods - Public

        /// <summary>
        /// Get a <see cref="PasteNote"/> by its id.
        /// </summary>
        /// <param name="id"><see cref="PasteNote"/> id</param>
        /// <returns><see cref="PasteNote"/> with specified id</returns>
        public PasteNote GetPasteNote(int id)
        {
            PasteNote note = null;
            List<SqlParameter> procedureParameters = new List<SqlParameter>();
            procedureParameters.Add(new SqlParameter("@Id", id));

            using (IDataReader reader = _sqlDal.ExecuteQuery(_getPasteNoteProcedureName, procedureParameters))
            {
                while (reader.Read())
                {
                    note = GetPasteNoteFromReader(reader);
                }
            }

            return note;
        }

        /// <summary>
        /// Returns a list of <see cref="PasteNote"/>s created by logged-in user.
        /// </summary>
        /// <param name="username">Logged in user's name</param>
        /// <returns>List of User's <see cref="PasteNote"/>s</returns>
        public IEnumerable<PasteNote> GetPasteNotesForUser(string username)
        {
            List<PasteNote> notes = new List<PasteNote>();
            List<SqlParameter> procedureParameters = new List<SqlParameter>();
            procedureParameters.Add(new SqlParameter("@Username", username));

            using (IDataReader reader = _sqlDal.ExecuteQuery(_getPasteNotesForUserProcedureName, procedureParameters))
            {
                while (reader.Read())
                {
                    notes.Add(GetPasteNoteFromReader(reader));
                }
            }

            return notes;
        }

        /// <summary>
        /// Returns all PasteNotes in the Database Table.
        /// </summary>
        /// <returns>List of PasteNotes</returns>
        public IEnumerable<PasteNote> GetAllPasteNotes()
        {
            List<PasteNote> notes = new List<PasteNote>();

            using (IDataReader reader = _sqlDal.ExecuteQuery(_getAllPasteNotesProcedureName))
            {
                while (reader.Read())
                {
                    notes.Add(GetPasteNoteFromReader(reader));
                }
            }

            return notes;
        }

        /// <summary>
        /// Get <see cref="PasteNote"/>s created after the specified date.
        /// </summary>
        /// <param name="starting">Start Date</param>
        /// <returns>A List of <see cref="PasteNote"/>s</returns>
        public IEnumerable<PasteNote> GetRecentPasteNotes(DateTime starting)
        {
            List<PasteNote> notes = new List<PasteNote>();

            List<SqlParameter> procedureParameters = new List<SqlParameter>();
            procedureParameters.Add(new SqlParameter("@StartDate", starting));

            using (IDataReader reader = _sqlDal.ExecuteQuery(_getRecentPasteNotesProcedureName, procedureParameters))
            {
                while (reader.Read())
                {
                    notes.Add(GetPasteNoteFromReader(reader));
                }
            }

            return notes;
        }

        /// <summary>
        /// Remove <see cref="PasteNote"/> with specified id.
        /// </summary>
        /// <param name="id"><see cref="PasteNote"/> id</param>
        /// <returns>True if a note was successfully deleted.</returns>
        public bool RemovePasteNote(int id)
        {
            bool success = false;
            if (id >= 0)
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

        /// <summary>
        /// Add new <see cref="PasteNote"/> to DataBase
        /// </summary>
        /// <param name="newNote"><see cref="PasteNote"/> to submit</param>
        /// <returns>Returns submitted <see cref="PasteNote"/></returns>
        public PasteNote AddPasteNote(PasteNote newNote)
        {
            PasteNote note = null;
            List<SqlParameter> procedureParameters = new List<SqlParameter>();
            procedureParameters.Add(new SqlParameter("@Title", newNote.Title));
            procedureParameters.Add(new SqlParameter("@Description", newNote.Description));
            procedureParameters.Add(new SqlParameter("@Post", newNote.Post));
            procedureParameters.Add(new SqlParameter("@Language", newNote.Language));
            procedureParameters.Add(new SqlParameter("@UserName", newNote.UserName));

            using (IDataReader reader = _sqlDal.ExecuteQuery(_addPasteNoteProcedureName, procedureParameters))
            {
                while (reader.Read())
                {
                    note = GetPasteNoteFromReader(reader);
                }
            }

            return note;
        }

        /// <summary>
        /// Retrieves <see cref="PasteNote"/>s containing keyword in title, description or tags.
        /// </summary>
        /// <param name="keyword">Keyword to search for</param>
        /// <returns>List of <see cref="PasteNote"/>s containing keyword.</returns>
        public IEnumerable<PasteNote> GetPasteNotesWithKeyWord(string keyword)
        {
            List<PasteNote> notes = new List<PasteNote>();
            List<SqlParameter> procedureParameters = new List<SqlParameter>();
            procedureParameters.Add(new SqlParameter("@Keyword", keyword));

            using (IDataReader reader = _sqlDal.ExecuteQuery(_getPasteNotesWithKeywordProcedureName, procedureParameters))
            {
                while (reader.Read())
                {
                    notes.Add(GetPasteNoteFromReader(reader));
                }
            }

            return notes;
        }

        // Tags

        /// <summary>
        /// Returns a list of <see cref="Tag"/>s.
        /// </summary>
        /// <returns>List of User's <see cref="PasteNote"/>s</returns>
        public IEnumerable<Tag> GetTags()
        {
            List<Tag> tags = new List<Tag>();

            using (IDataReader reader = _sqlDal.ExecuteQuery(_getAllTagsProcedureName))
            {
                while (reader.Read())
                {
                    tags.Add(new Tag { Name = reader.GetString(reader.GetOrdinal(_columnTagName)) });
                }
            }

            return tags;
        }

        /// <summary>
        /// Returns a list of <see cref="Tag"/>s.
        /// </summary>
        /// <returns>List of User's <see cref="PasteNote"/>s</returns>
        public IEnumerable<Tag> GetTagsForNote(int noteId)
        {
            List<Tag> tags = new List<Tag>();
            List<SqlParameter> procedureParameters = new List<SqlParameter>();
            procedureParameters.Add(new SqlParameter("@NoteId", noteId));

            using (IDataReader reader = _sqlDal.ExecuteQuery(_getTagsForNoteProcedureName, procedureParameters))
            {
                while (reader.Read())
                {
                    tags.Add(new Tag { Name = reader.GetString(reader.GetOrdinal(_columnNoteTagName)) });
                }
            }

            return tags;
        }

        /// <summary>
        /// Submit Tags for Note Id
        /// This will batch upload several rows to the NoteTag Table.
        /// </summary>
        /// <param name="noteId"><see cref="PasteNote"/> Id associated with Tags</param>
        /// <returns>Returns true if the tags were successfull inserted into the database.</returns>
        public bool AddTagsForNote(int noteId, List<Tag> noteTags)
        {

            return
                _sqlDal.ExecuteBulkCopy(
                            ConstructTagDataTable(noteId, noteTags),
                            _noteTagsTableName);
        }

        #endregion

        #region Utils

        /// <summary>
        /// Generate a new PasteNote using a Sql Reader
        /// Utility because this shouldn't be part of the Object Constructor
        /// </summary>
        /// <param name="reader">Sql Data Reader</param>
        /// <returns>A new <see cref="PasteNote"/></returns>
        private PasteNote GetPasteNoteFromReader(IDataReader reader)
        {
            int noteId = Convert.ToInt32(reader[reader.GetOrdinal(_columnId)]);
            reader.SafeGetString(4);
            return
                new PasteNote
                {
                    Id = noteId,
                    Title = reader.SafeGetString(reader.GetOrdinal(_columnTitle)),
                    Description = reader.SafeGetString(reader.GetOrdinal(_columnDescription)),
                    Language = reader.SafeGetString(reader.GetOrdinal(_columnLanguage)),
                    Post = reader.SafeGetString(reader.GetOrdinal(_columnPost)),
                    UserName = reader.SafeGetString(reader.GetOrdinal(_columnUsername)),
                    Tags = GetTagsForNote(noteId).ToList<Tag>()
                };
        }

        /// <summary>
        /// Converts a List of <see cref="Tag"/>s to a DataTable
        /// DataTable columns are "ID" and "Name"
        /// </summary>
        /// <param name="noteId"><see cref="PasteNote"/>Id</param>
        /// <param name="noteTags">List of <see cref="Tag"/>s</param>
        /// <returns></returns>
        private DataTable ConstructTagDataTable(int noteId, List<Tag> noteTags)
        {
            DataTable noteTagsTable = new DataTable();
            noteTagsTable.Columns.Add(_columnNoteTagId, typeof(int));
            noteTagsTable.Columns.Add(_columnNoteTagName, typeof(string));

            foreach (Tag noteTag in noteTags)
            {
                DataRow row = noteTagsTable.NewRow();
                row[_columnNoteTagId] = noteId;
                row[_columnNoteTagName] = noteTag.Name;
                noteTagsTable.Rows.Add(row);
            }

            return noteTagsTable;
        }

        #endregion



    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using OSPasteBin.BusinessObjects;
using OSPasteBin.DAL;
using System.Configuration;
using OSPasteBin.Models.ViewModels;


namespace OSPasteBin.Controllers
{
    /// <summary>
    /// Process all PasteNote Actions
    /// </summary>
    public class PasteNoteController : Controller
    {

        private IPasteBinDAL _dal;
        private const string _connectionStringName = "OSPasteBinConnection";

        #region CTOR

        public PasteNoteController()
        {
            _dal = new PasteBinSqlDAL(ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString);
        }
        #endregion

        #region Actions - GET
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"><see cref="PasteNote"/> id.</param>
        /// <param name="mode">View Mode. Raw for raw text, omit for normal display.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Notes(int id, string mode)
        {

            NoteViewModel noteViewModel = new NoteViewModel
            {
                 PasteNote = _dal.GetPasteNote(id),
                 NoteTags = _dal.GetTagsForNote(id).ToList<Tag>()
            };


            if (noteViewModel.PasteNote == null)
                return View("NotFound");

            if (String.Equals(mode, "Raw", StringComparison.CurrentCultureIgnoreCase))
                return View("Raw", noteViewModel.PasteNote);


            return View("Note", noteViewModel);
        }

        /// <summary>
        /// Get logged in user's notes
        /// </summary>
        /// <returns>A view of a list of the logged in user's notes.</returns>
        [HttpGet]
        public ActionResult MyNotes()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;
                var userNotes = _dal.GetPasteNotesForUser(userName);
                return View("UserNotes", userNotes);
            }
            else
                return RedirectToAction("Login", "Account");

        }

        /// <summary>
        /// Returns <see cref="PasteNote"/> Tagged with KeyWord
        /// </summary>
        /// <param name="keyword">Search Keyword</param>
        /// <returns>View - Listing of Tagged <see cref="PasteNote"/>s</returns>
        [HttpGet]
        public ActionResult TaggedNotes(string keyword)
        {
            ViewBag.keyword = keyword;
            return View(_dal.GetPasteNotesWithKeyWord(keyword));
        }

        /// <summary>
        /// New Note Entry Form
        /// </summary>
        [HttpGet]
        public ActionResult New()
        {
            return View();
        }
        #endregion

        #region Actions - POST
        /// <summary>
        /// Submits a new Note to the DAL
        /// </summary>
        /// <param name="pasteNote"><see cref="PasteNote"/> to be submitted.</param>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(PasteNote pasteNote)
        {
            // Encode Post text
            pasteNote.Post = HttpUtility.HtmlEncode(pasteNote.Post);

            // User
            pasteNote.UserName = (User.Identity.IsAuthenticated) ? User.Identity.Name : string.Empty;

            PasteNote newNote = _dal.AddPasteNote(pasteNote);

            // Tags 
            // Pushed to a separate table
            if (!String.IsNullOrEmpty(Request["tags"]))
                _dal.AddTagsForNote(newNote.Id, ConstructNoteTags(Request["tags"]));

            return RedirectToAction("Notes", new { id = newNote.Id });
        }
        #endregion

        #region Utils

        /// <summary>
        /// Generate a List of <see cref="Tag"/>s from a delimitted string.
        /// </summary>
        /// <param name="tags">delimitted string of tags</param>
        /// <returns>A List Collection of <see cref="Tag"/>s</returns>
        private List<Tag> ConstructNoteTags(string tags)
        {
            string delimitter = ";";

            List<Tag> noteTags = new List<Tag>();

            if (tags.Contains(delimitter))
            {
                // Remove trailing ';'
                if (tags.EndsWith(delimitter))
                    tags = tags.Substring(0, tags.LastIndexOf(delimitter));

                var tagsCollection = tags.Split(delimitter[0]).Distinct(); // Prevent Duplicates

                foreach (string tag in tagsCollection)
                    noteTags.Add(new Tag { Name = tag });
            }
            else
                noteTags.Add(new Tag { Name = tags });

            return noteTags;
        }
        #endregion
    }
}

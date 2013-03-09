using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using OSPasteBin.BusinessObjects;
using OSPasteBin.DAL;
using System.Configuration;


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
            PasteNote pasteNote = _dal.GetPasteNote(id);


            if (pasteNote == null)
                return View("NotFound");

            if (String.Equals(mode, "Raw", StringComparison.CurrentCultureIgnoreCase))
                return View("Raw", pasteNote);


            return View("Note", pasteNote);
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
            pasteNote.Post = HttpUtility.HtmlEncode(pasteNote.Post);

            if (User.Identity.IsAuthenticated)
                pasteNote.UserName = User.Identity.Name;
            else
                pasteNote.UserName = string.Empty;

            if (String.IsNullOrEmpty(pasteNote.Title))
                pasteNote.Title = string.Empty;

            if (String.IsNullOrEmpty(pasteNote.Description))
                pasteNote.Description = string.Empty;

            PasteNote newNote = _dal.AddPostNote(pasteNote);
            return RedirectToAction("Notes", new { id = newNote.Id });
        } 
        #endregion
    }
}

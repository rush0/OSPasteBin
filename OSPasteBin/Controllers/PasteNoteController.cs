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


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

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

        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(PasteNote pasteNote)
        {
            pasteNote.Post = HttpUtility.HtmlEncode(pasteNote.Post);

            if (User.Identity.IsAuthenticated)
                pasteNote.UserName = User.Identity.Name;
            else
                pasteNote.UserName = string.Empty;

            PasteNote newNote = _dal.AddPostNote(pasteNote);
            return RedirectToAction("Notes", new { id = newNote.Id });
        }
    }
}

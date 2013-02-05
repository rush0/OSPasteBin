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
        private const string _connectionStringName = "";
        
        #region CTOR

        public PasteNoteController()
        {
            _dal = new PasteBinSqlDAL( ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString );
        }
        #endregion


        [HttpGet]
        public ActionResult Index(int id)
        {
            var pasteNote = _dal.GetPasteNote(id);
            return View(pasteNote);
        }

        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(PasteNote pasteNote)
        {
            if (ModelState.IsValid)
                RedirectToAction("Index", new { id = pasteNote.Id });

            return View();
        }

    }
}

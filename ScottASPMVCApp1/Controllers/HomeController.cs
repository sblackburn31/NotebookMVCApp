using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Data.Entity;
using System.Net;
using ScottASPMVCApp1.Models;

namespace ScottASPMVCApp1.Controllers
{
    public class HomeController : Controller
    {
        private DataRepository dataRepository = null;

        public HomeController()
            : this(new DataRepository())
        {
        }

        public HomeController(DataRepository dr)
        {
            this.dataRepository = dr;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                // If someone is signed in redirect to Notebook
                return RedirectToAction("Notebook");
            }
            else
            {
                // No one has signed in
                // Situation 4 show the welcome (i.e. index) view.
                return View("Index");
            }
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View("About");
        }

        [AllowAnonymous]
        public ActionResult HelpPage()
        {
            ViewBag.Message = "The Help page.";

            return View("HelpPage");
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View("Contact");
        }

        [Authorize]
        public ActionResult Notebook()
        {
            string emailName = User.Identity.Name;

            NoteBook aNotebook = dataRepository.NotebookRepository.GetNotebookByEmail(emailName);
            ViewBag.UserTitle = emailName.Substring(0,1).ToUpper() + emailName.Substring(1, emailName.IndexOf('@')-1) + "'s";
            // Situation 1 - Newly signed up user with no defined notebook
            // a new notebook record is required, create a new notebook and
            // show it (it contains no Notes)
            if (aNotebook == null)
            {
                dataRepository.NotebookRepository.CreateNotebook(emailName);
                dataRepository.NotebookRepository.Save();
                aNotebook = dataRepository.NotebookRepository.GetNotebookByEmail(emailName);

                return View("Notebook", aNotebook);
            }
            else
            {
                // User has an existing notebook
                // Check to see if the the notebook has a Current Note open
                long? noteId = aNotebook.CurrentNote;
                if (noteId == null)
                {
                    // The current note is null which means that there is no current note open
                    // Situation 2, Show existing notebook
                    return View("Notebook", aNotebook);
                }
                else
                {
                    // bring up current note
                    Note currentNote = dataRepository.NoteRepository.GetNoteById((long)noteId, User.Identity.Name);
                    // If the currentNote is not found then bring up the notebook list
                    if (currentNote == null)
                    {
                        // This should never be seen, because it means that a referenced note does not exist.
                        // However, the recovery for this situation is to display the notebook
                        return View("Notebook", aNotebook);
                    }
                    // the note was found edit the note
                    // Situation 3 Bring up the current note in the EditNote view
                    return RedirectToAction("EditNote", new { id = noteId });
                }

            }
        }

        public ActionResult CloseNote()
        {
            string curUser = User.Identity.Name;
            NoteBook aNotebook = dataRepository.NotebookRepository.GetNotebookByEmail(curUser); 

            // Set the notebook's current note to nothing
            aNotebook.CurrentNote = null;

            dataRepository.NotebookRepository.UpdateNotebook(aNotebook.Email, aNotebook);
            dataRepository.NotebookRepository.Save();

            return RedirectToAction("Notebook");

        }

        [Authorize]
        public ActionResult EditNote(long? id)
        {
            string userEmail = User.Identity.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = dataRepository.NoteRepository.GetNoteById((long)id, User.Identity.Name); 
            if (note == null)
            {
                return HttpNotFound();
            }

            NoteBook aNotebook = dataRepository.NotebookRepository.GetNotebookByEmail(userEmail); 
            if (aNotebook.CurrentNote == null || aNotebook.CurrentNote != id ) 
            {
                aNotebook.CurrentNote = id;
                dataRepository.NotebookRepository.UpdateNotebook(userEmail, aNotebook);
                dataRepository.NotebookRepository.Save();
            }
            return View("EditNote", note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, 
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditNote([Bind(Include = "Subject,NoteText, NoteId")] Note note)
        {
            if (ModelState.IsValid)
            {
                // Update Note
                Note aNote = dataRepository.NoteRepository.GetNoteById(note.NoteId);
                aNote.Subject = note.Subject;
                aNote.NoteText = note.NoteText.Substring(0, Math.Min(note.NoteText.Length, 500));
                aNote.UpdateDate = DateTime.Now;
                
                dataRepository.NoteRepository.UpdateNote(note.NoteId, aNote);
                dataRepository.NoteRepository.Save();

                return View("EditNote", aNote);
            }
           
            return View("EditNote", note);
        }

        // GET: Notes/Details/5
        [Authorize]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = dataRepository.NoteRepository.GetNoteById((long)id, User.Identity.Name); 
            if (note == null)
            {
                return HttpNotFound();
            }
            return View("Details",note);
        }

        // GET: Notes/Create
        [Authorize]
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NoteId,Subject,NoteText")] Note note)
        {
            
            if (ModelState.IsValid)
            {
                NoteBook aNotebook = dataRepository.NotebookRepository.GetNotebookByEmail(User.Identity.Name);

                note.NoteBookId = aNotebook.NotebookId;
                note.CreateDate = DateTime.Now;
                note.UpdateDate = DateTime.Now;
                dataRepository.NoteRepository.AddNote(note);
                dataRepository.NoteRepository.Save();

                aNotebook.CurrentNote = note.NoteId;
                dataRepository.NotebookRepository.UpdateNotebook(aNotebook.Email, aNotebook );
                dataRepository.NotebookRepository.Save();

                return RedirectToAction("Notebook");
            }

            return View("Create",note);
        }

        // GET: Notes/Delete/5
        [Authorize]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Note note = dataRepository.NoteRepository.GetNoteById((long)id, User.Identity.Name); 
            if (note == null)
            {
                return HttpNotFound();
            }
            return View("Delete", note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {

            Note note = dataRepository.NoteRepository.GetNoteById(id);
            dataRepository.NoteRepository.DeleteNote (note);
            dataRepository.NoteRepository.Save();

            return RedirectToAction("Notebook");
        }

    }
}
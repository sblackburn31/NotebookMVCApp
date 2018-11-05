/// <project>Notebook App</project>
/// <version>1.0.0</version>
/// <author>David Scott Blackburn</author>
/// <summary>
/// This is the primary controller for the application.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Data.Entity;
using System.Net;
using NotebookMVCApp.Models;

namespace NotebookMVCApp.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// dataRepository is how the notebook data is accessed.  From here the notebook and note repositories can be accessed.  How these classes are 
        /// implemented is completely hidden from the controller.
        /// </summary>
        private DataRepository dataRepository = null;

        /// <summary>
        /// This is how the datarepository is injected into the controller object.
        /// </summary>
        public HomeController()
            : this(new DataRepository())
        {
        }

        /// <summary>
        /// This is how the datarepository is injected into the controller object.
        /// </summary>
        public HomeController(DataRepository dr)
        {
            this.dataRepository = dr;
        }

        /// <summary>
        /// Index: this is the initial action taken when the application is brought up by the system. If the user is not yet logged in
        /// the base welcome screen is displayed.  If the user previously logged in with the Remember Me option selected, then the note they were
        /// last editing or their notebook is displayed.
        /// </summary>
        /// <returns>Returns an Action result, that is a view if the Index screen or a redirection to the Notebook Action</returns>
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
            ViewBag.Message = "The application description page.";

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
            ViewBag.Message = "The contact page.";
            return View("Contact");
        }


        /// <summary>
        /// Notebook - this page shows the notes belonging to the user's notebook. 
        /// If the user was editing a note and exited the app, then the edited note will come up; otherwise the list of notes is displayed.
        /// From the Notebook page the user may edit, delete or view one of the previous notes or create a new one.
        /// If this is the first time the user has logged in then a new notebook is created.
        /// User must be Logged In in order to be authorized to use this action.
        /// </summary>
        /// <returns>
        /// The Notebook view is viewed, unless the user exited a note they were editing in the prevoius session, in that case the note is brought up
        /// in the edit note screen.
        /// </returns>
        [Authorize]
        public ActionResult Notebook()
        {
            string emailName = User.Identity.Name;

            NoteBook aNotebook = dataRepository.NotebookRepository.GetNotebookByEmail(emailName);
            // Display the user's base name for the name of the notebook.
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

        /// <summary>
        /// This action is reached by Edit note's CANCEL button.
        /// This action nulls out the Notebook's current Note property.  Current Note, when set to a valid note id will cause notebook action
        /// to bring up a note with the Edit Note action.
        /// </summary>
        /// <returns>Always reditects to the Notebook</returns>
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

        /// <summary>
        /// EditNote
        /// This Action is reached from:
        /// Notebook redirection
        /// Create Note's successful save Post
        /// Selected as a list item from the Notebook
        /// In order to Edit a Note the user must supply the note's ID and the note belong to the user who is currently logged in
        /// The note is retrieved from the Note Repository and the Notebook's Current Note is set to the note's ID.
        /// </summary>
        /// <param name="id">This is the Note's ID.  The note must belong to the logged in user.</param>
        /// <returns>
        ///     View the Edit Note screen if the Note is properly returned.
        ///     If the ID is null return BadRequest
        ///     If the Note is not returned from the repository, return Not Found, this happens if the ID is bad or if the user
        ///         does not own the note.
        /// </returns>
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

        /// <summary>
        /// Post Action - Edit Note
        /// This occurs when the Edit Note save button is pressed.
        /// The Model state is checked to ensure that the note is in a valid state.  If the note is not valid then the note is redisplayed with the invalid
        /// messages displayed.  If the Model state is valid, then the changes to the note are saved to the NoteRepository.
        /// </summary>
        /// <param name="note">Note - The note with the changes that are to be saved</param>
        /// <returns>
        ///     If the model state is invalid, return to the edit note screen with the invalid note state
        ///     Otherwise save the changes and return to the edit note screen
        /// </returns>
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

        /// <summary>
        /// Details
        /// This action is reached by selection as a list item from the Notebook
        /// In order to View the Note the user must supply the note's ID and the note belong to the user who is currently logged in
        /// The note is retrieved from the Note Repository.
        /// </summary>
        /// <param name="id">This is the Note's ID.  The note must belong to the logged in user.</param>
        /// <returns>
        ///     View the Details screen if the Note is properly returned.
        ///     If the ID is null return BadRequest
        ///     If the Note is not returned from the repository, return Not Found, this happens if the ID is bad or if the user
        ///         does not own the note.
        /// </returns>
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


        /// <summary>
        /// Create
        /// This action is reached by the CREATE NOTE button from the Notebook screen
        /// This simplely calls up the Create View.
        /// From the Create View screen the user must enter a Subject and some text.  If either of those values are blank then the fields are invalid.
        /// </summary>
        /// <returns>It always returns the Create Screen</returns>
        // GET: Notes/Create
        [Authorize]
        public ActionResult Create()
        {
            return View("Create");
        }


        /// <summary>
        /// Post Action - Create 
        /// This occurs when the Save button is pressed in the Create screen.
        /// The Model state is checked to ensure that the note is in a valid state.  If the note is not valid then the note is redisplayed with the invalid
        /// messages displayed.  If the Model state is valid, then a new note is added to the NoteRepository.
        /// </summary>
        /// <param name="note">Note - The note to be added to the note repository</param>
        /// <returns>
        ///     If the model state is invalid, return to the create note screen with the invalid note state
        ///     Otherwise add the note and redirect to the edit note screen
        /// </returns>
        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details 
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


        /// <summary>
        /// Delete
        /// This action is reached by selection as a list item from the Notebook
        /// In order to remove the Note the user must supply the note's ID and the note belong to the user who is currently logged in
        /// The note is retrieved from the Note Repository.
        /// From the Delete screen the user may select the DELETE action which confirms the delete action and causes the note to be removed
        /// from the NoteRepository or return to the Note book if cancel is selected.
        /// </summary>
        /// <param name="id">This is the Note's ID.  The note must belong to the logged in user.</param>
        /// <returns>
        ///     View the Delete screen if the Note is properly returned.
        ///     If the ID is null return BadRequest
        ///     If the Note is not returned from the repository, return Not Found, this happens if the ID is bad or if the user
        ///         does not own the note.
        /// </returns>
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

        /// <summary>
        /// Post Action - Delete Confirmed
        /// This is called from the Delete screen
        /// This action removes the note from the note repository
        /// </summary>
        /// <param name="id">This is the id of the note to be deleted</param>
        /// <returns>this always redirects to the Notebook screen</returns>
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ScottASPMVCApp1;
using ScottASPMVCApp1.Controllers;
using ScottASPMVCApp1.Models;
using ScottASPMVCApp1.Tests.Models;

namespace ScottASPMVCApp1.Controllers.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        #region Test Setup
        String newlySignedUp = "NewGuy@Sample.com";
        long? badId = long.MaxValue;

        Note note1 = null;
        Note note2 = null;
        Note note3 = null;
        Note note4 = null;
        Note note5 = null;

        NoteBook notebook1 = null;
        NoteBook notebook2 = null;
        NoteBook notebook3 = null;
        NoteBook notebook4 = null;
        NoteBook notebook5 = null;

        List<Note> noteList = null;
        List<NoteBook> notebookList = null;
        DummyNotebooksRepository notebooksRepo = null;
        DummyNoteRepository noteRepo = null;
        DataRepository dataRepo = null;
        HomeController controller = null;

        public HomeControllerTest()
        {
            // Lets create some sample notebooks
            // Notebook 1 does not has a current note
            notebook1 = new NoteBook { NotebookId = 1, Email= "One@Sample.com",  CreateDate = DateTime.Now, CurrentNote = null };
            // Notebook 2 has a current note that is valid
            notebook2 = new NoteBook { NotebookId = 2, Email = "test@test.test", CreateDate = DateTime.Now, CurrentNote = 22 };
            // Notebook 3 has a current note that does not exist on the note list
            notebook3 = new NoteBook { NotebookId = 3, Email = "test2@test2.test", CreateDate = DateTime.Now, CurrentNote = 333 };
            //Notebook 4 has no assoicated notes
            notebook4 = new NoteBook { NotebookId = 4, Email = "two@sample.com", CreateDate = DateTime.Now, CurrentNote = null };
            //Notebook 5 does not exist on the notebook list and has no assoicated notes
            notebook5 = new NoteBook { NotebookId = 5, Email = "ten@sample.com", CreateDate = DateTime.Now, CurrentNote = null };


            notebookList = new List<NoteBook>
            {
                notebook1,
                notebook2,
                notebook3,
                notebook4
            };

            // Lets create some sample notes and notebooks
            note1 = new Note { NoteBookId = 1, NoteId = 10,
                        Subject = "10",
                        NoteText = "10",
                        CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            note2 = new Note
            {
                NoteBookId = 2,
                NoteId = 22,
                Subject = "22",
                NoteText = "22",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
            note3 = new Note
            {
                NoteBookId = 3,
                NoteId = 33,
                Subject = "33",
                NoteText = "33",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
            note4 = new Note
            {
                NoteBookId = 1,
                NoteId = 14,
                Subject = "14",
                NoteText = "14",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
            note5 = new Note
            {
                NoteBookId = 1,
                NoteId = 13,
                Subject = "c",
                NoteText = "d",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };

            noteList = new List<Note>
            {
                note1,
                note2,
                note3,
                note4, 
                new Note {NoteBookId = 1, NoteId = 15,
                    Subject = "15",
                    NoteText = "15",
                    CreateDate = DateTime.Now, UpdateDate = DateTime.Now },
                new Note {NoteBookId = 2, NoteId = 21,
                    Subject = "21",
                    NoteText = "21",
                    CreateDate = DateTime.Now, UpdateDate = DateTime.Now },
                new Note {NoteBookId = 3, NoteId = 16,
                    Subject = "26",
                    NoteText = "16",
                    CreateDate = DateTime.Now, UpdateDate = DateTime.Now },
                new Note {NoteBookId = 1, NoteId = 17,
                    Subject = "e",
                    NoteText = "f",
                    CreateDate = DateTime.Now, UpdateDate = DateTime.Now },
                new Note {NoteBookId = 2, NoteId = 28,
                    Subject = "g",
                    NoteText = "h",
                    CreateDate = DateTime.Now, UpdateDate = DateTime.Now },
                new Note {NoteBookId = 3, NoteId = 30,
                    Subject = "30",
                    NoteText = "30",
                    CreateDate = DateTime.Now, UpdateDate = DateTime.Now },
                new Note {NoteBookId = 1, NoteId = 19,
                    Subject = "19",
                    NoteText = "19",
                    CreateDate = DateTime.Now, UpdateDate = DateTime.Now },
                new Note {NoteBookId = 2, NoteId = 24,
                    Subject = "24",
                    NoteText = "24",
                    CreateDate = DateTime.Now, UpdateDate = DateTime.Now },
                new Note {NoteBookId = 2, NoteId = 25,
                    Subject = "25",
                    NoteText = "25",
                    CreateDate = DateTime.Now, UpdateDate = DateTime.Now },
            };

            // Lets create our dummy repository
            notebooksRepo = new DummyNotebooksRepository(notebookList);
            noteRepo = new DummyNoteRepository(noteList);

            // Let us now create the Unit of work with our dummy repository
            dataRepo = new DataRepository(notebooksRepo, noteRepo);

            // Now lets create the BooksController object to test and pass our unit of work
            controller = new HomeController(dataRepo);
            noteRepo.SetNotebook(notebookList);
        }
        #endregion

        #region Simple Actions
        [TestMethod]
        public void About()
        {
            // Arrange

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            // check for any required information it supposed to displayed.
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange

            // Act
            ViewResult result = controller.Contact() as ViewResult;


            // Assert
            Assert.AreEqual("Contact", result.ViewName);
            // Check for any information that needs to be displayed Create
        }
        #endregion

        #region Index
        [TestMethod]
        // If someone is logged onto the system, then authentication is true, redirect to the Notebook screen.
        public void IndexIsAuthenticated()
        {
            // Arrange
            SetIsAutorized(true);

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Index();

            // Assert
            Assert.AreEqual("Notebook", result.RouteValues["action"].ToString());
        }
        [TestMethod]
        // If noone is logged onto the system, then noone is autherized to use the system, show the Index screen
        public void IndexIsNotAuthenticated()
        {

            // Arrange
            SetIsAutorized(false);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Index", result.ViewName);
        }
        #endregion

        #region Notebook
        [TestMethod]
        // This tests the creation of a new notebook
        public void NotebookNewUserNewNotebook()
        {
            // Arrange
            SetTestUserName(newlySignedUp);

            // Assert
            ViewResult result = controller.Notebook() as ViewResult;

            // Assert
            Assert.AreEqual("Notebook", result.ViewName);

            // check for the newly added notebook
            NoteBook testNotebook = dataRepo.NotebookRepository.GetNotebookByEmail(newlySignedUp);
            Assert.IsNotNull(testNotebook);
            Assert.IsNull(testNotebook.CurrentNote);
            Assert.AreSame(result.Model, testNotebook);
        }

        [TestMethod]
        // This tests for an existing notebook, that does not have a current note
        public void NotebookNotebookNoCurrentNote()
        {
            // Arrange
            SetTestUserName(notebook1.Email);

            // Action
            ViewResult result = controller.Notebook() as ViewResult;

            // Assert
            Assert.AreEqual("Notebook", result.ViewName);

            // check for the newly added notebook
            NoteBook returnedNotebook = (NoteBook)result.Model;
            
            // Make sure something was returned
            Assert.IsNotNull(returnedNotebook);
            // Make sure that the current note is null, key to the test!
            Assert.IsNull(returnedNotebook.CurrentNote);
            // Make sure that the returned notebook is the same as the one
            // we initially tested for
            Assert.AreSame(returnedNotebook, notebook1);
        }


        [TestMethod]
        // This tests for a current note
        public void NotebookNotebookCurrentNote()
        {
            // Arrange
            SetTestUserName(notebook2.Email);

            // Action
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Notebook();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("EditNote", result.RouteValues["action"].ToString());
            Assert.AreEqual(notebook2.CurrentNote, result.RouteValues["id"]);
        }


        [TestMethod]
        // This tests for a condition that should never arise, that is
        // where a current note does not exist in the note list.
        // The recovery for this is to treat the current note as if it doesn't
        // exist and to continue.
        public void NotebookNotebookBadCurrentNote()
        {
            // Arrange
            SetTestUserName(notebook3.Email);

            // Action
            ViewResult result = controller.Notebook() as ViewResult;

            // Assert
            Assert.AreEqual("Notebook", result.ViewName);

            // check for the newly added notebook
            NoteBook returnedNotebook = (NoteBook)result.Model;

            // Make sure something was returned
            Assert.IsNotNull(returnedNotebook);
            // Make sure that the current note is null, key to the test!
            Assert.IsNotNull(returnedNotebook.CurrentNote);
            // Make sure that the returned notebook is the same as the one
            // we initially tested for
            Assert.AreSame(returnedNotebook, notebook3);
        }
        #endregion

        #region EditNote
        [TestMethod]
        // Edit a note with a good Id
        // The notebook's current note is set to the note id
        public void EditNoteCurrentNoteSame()
        {
            // Arrange
            long? preCurrentNote = notebook2.CurrentNote;
            SetTestUserName(notebook2.Email);

            // Act
            ViewResult result = controller.EditNote(note2.NoteId) as ViewResult;

            // Assert
            Assert.AreEqual("EditNote", result.ViewName );
            Assert.AreEqual (notebook2.CurrentNote, note2.NoteId );
        }

        [TestMethod]
        public void EditNoteCurrentNoteNull()
        {
            // Arrange
            long? preCurrentNote = notebook1.CurrentNote;
            SetTestUserName(notebook1.Email);

            // Act
            ViewResult result = controller.EditNote(note1.NoteId) as ViewResult;

            // Assert
            Assert.AreEqual("EditNote", result.ViewName);
            Assert.AreEqual(notebook1.CurrentNote, note1.NoteId);


        }


        [TestMethod]
        public void EditNoteCurrentNoteDifferent()
        {
            // Arrange
            long? preCurrentNote = notebook3.CurrentNote;
            SetTestUserName(notebook3.Email);

            // Act
            ViewResult result = controller.EditNote(note3.NoteId) as ViewResult;

            // Assert
            Assert.AreEqual("EditNote", result.ViewName);
            Assert.AreEqual(notebook3.CurrentNote, note3.NoteId);
        }

        [TestMethod]
        // Edit a note with a bad Id, that is one that does not exist
        public void EditNoteBadID()
        {
            // Arrange
            long? preCurrentNote = notebook3.CurrentNote;
            SetTestUserName(notebook3.Email);

            // Act
            ActionResult result = controller.EditNote(preCurrentNote);

            // Assert
            Assert.IsNotNull(result);
            HttpStatusCodeResult r = (HttpStatusCodeResult)result;
            Assert.AreEqual(r.StatusCode, 404);
        }

        [TestMethod]
        // Edit a note with no Id
        public void EditNoteNoID()
        {
            // Arrange
            SetTestUserName(notebook2.Email);

            // Act
            ActionResult result = controller.EditNote((long?)null);

            // Assert
            Assert.IsNotNull(result);
            HttpStatusCodeResult r = (HttpStatusCodeResult)result;
            Assert.AreEqual(r.StatusCode, 400);
        }

        [TestMethod]
        // Edit a note with valid model state
        public void EditModelStateValid()
        {
            // Arrange
            string testString = "This is a test";
            SetTestUserName(notebook1.Email);
            Note editedNote = new Note();
            editedNote.NoteBookId = note1.NoteBookId;
            editedNote.NoteId = note1.NoteId;
            editedNote.Subject = note1.Subject;
            editedNote.NoteText = testString;
            editedNote.UpdateDate = note1.UpdateDate;
            editedNote.CreateDate = note1.CreateDate;

            // Act
            ViewResult result = controller.EditNote(editedNote) as ViewResult;

            // Assert
            Assert.AreEqual("EditNote", result.ViewName);
            Note noteInList = dataRepo.NoteRepository.GetNoteById(note1.NoteId);
            Note returnedNote = (Note)result.Model;
            Assert.AreSame(returnedNote, noteInList);
            Assert.AreEqual(testString,note1.NoteText);
        }

        [TestMethod]
        // Edit a note with invalid model state
        public void EditModelStateNotValid()
        {
            // Arrange
            SetTestUserName(notebook1.Email);
            controller.ModelState.AddModelError("key", "error message");
            Note editedNote = new Note();
            editedNote.NoteBookId = note1.NoteBookId;
            editedNote.NoteId = note1.NoteId;
            editedNote.NoteText = "This is a test";
            // Act
            ViewResult result = controller.EditNote(editedNote) as ViewResult;

            // Assert
            Assert.AreEqual("EditNote", result.ViewName);
            Note noteInList = dataRepo.NoteRepository.GetNoteById(note1.NoteId);
            Note returnedNote = (Note)result.Model;
            Assert.AreNotEqual(returnedNote.NoteText, noteInList.NoteText);

        }

        [TestMethod]
        public void CloseNote()
        {
            // Arrange
            long? preCurrentNote = notebook2.CurrentNote;
            // set the current user to the notebook2 email value
            SetTestUserName(notebook2.Email);

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.CloseNote();

            // Assert
            // redirect to Notebook
            Assert.AreEqual("Notebook", result.RouteValues["action"].ToString());

            // after Users's notebook has a null value for the current note
            Assert.IsNull(notebook2.CurrentNote);
            Assert.IsNotNull(preCurrentNote);
        }
        #endregion

        #region Create

        [TestMethod]
        public void Create()
        {
            // Arrange
            long? preCurrentNote = notebook1.CurrentNote;

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod]
        public void CreateModelStateValid()
        {
            // Arrange
            long newNoteId = 77777;
            SetTestUserName(notebook1.Email);

            Note aNewNote = new Note();
            aNewNote.NoteId = newNoteId;
            aNewNote.NoteText = "This is a new note entry";
            aNewNote.Subject = "Test Note";

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Create(aNewNote);

            // Assert
            Assert.AreEqual("Notebook", result.RouteValues["action"].ToString());

            Assert.IsTrue(noteList.Contains(aNewNote));
            Assert.AreEqual(aNewNote.NoteId, notebook1.CurrentNote);
        }

        [TestMethod]
        // Edit a note with invalid model state
        public void CreateModelStateNotValid()
        {
            // Arrange
            long newNoteId = 55555;
            SetTestUserName(notebook1.Email);
            controller.ModelState.AddModelError("key", "error message");

            Note aNewNote = new Note();
            aNewNote.NoteId = newNoteId;
            aNewNote.NoteText = "This is a new note entry";
            aNewNote.Subject = "Test Note";

            // Act
            ViewResult result = controller.Create(aNewNote) as ViewResult;

            // Assert
            Assert.AreEqual("Create", result.ViewName);
            
            Note returnedNote = (Note)result.Model;
            Assert.IsFalse(noteList.Contains(aNewNote));
            Assert.AreNotEqual( aNewNote.NoteId, notebook1.CurrentNote);
        }

        #endregion

        #region Details
        [TestMethod]
        public void DetailsGoodNoteIDGoodUser()
        {
            // Arrange
            Note retrievedNote = dataRepo.NoteRepository.GetNoteById(note1.NoteId);
            SetTestUserName(notebook1.Email);

            // Act
            ViewResult result = controller.Details(note1.NoteId) as ViewResult;

            // Assert
            Assert.AreEqual("Details", result.ViewName);
            Assert.AreSame(retrievedNote, (Note)result.Model);
        }
        [TestMethod]
        public void DetailsGoodNoteIDWrongUser()
        {
            // Arrange
            Note retrievedNote = dataRepo.NoteRepository.GetNoteById(note1.NoteId);
            SetTestUserName(notebook2.Email);


            // Act
            ActionResult result = controller.Details(note1.NoteId);

            // Assert
            Assert.IsNotNull(result);
            HttpStatusCodeResult r = (HttpStatusCodeResult)result;
            Assert.AreEqual(r.StatusCode, 404);
        }

        [TestMethod]
        public void DetailsGoodNoteIDNonExistUser()
        {
            // Arrange
            Note retrievedNote = dataRepo.NoteRepository.GetNoteById(note1.NoteId);
            SetTestUserName("UNUSEDID@UNUSED.COM");

            // Act
            ActionResult result = controller.Details(note1.NoteId);

            // Assert
            Assert.IsNotNull(result);
            HttpStatusCodeResult r = (HttpStatusCodeResult)result;
            Assert.AreEqual(r.StatusCode, 404);
        }


        [TestMethod]
        // Edit a note with a bad Id, that is one that does not exist
        public void DetailsBadNoteID()
        {
            // Arrange
            long? preCurrentNote = notebook3.CurrentNote;
            SetTestUserName(notebook3.Email);

            // Act
            ActionResult result = controller.Details(preCurrentNote);

            // Assert
            Assert.IsNotNull(result);
            HttpStatusCodeResult r = (HttpStatusCodeResult)result;
            Assert.AreEqual(r.StatusCode, 404);
        }

        [TestMethod]
        // Edit a note with no Id
        public void DetailsNoNoteID()
        {
            // Arrange1
            SetTestUserName(notebook2.Email);

            // Act
            ActionResult result = controller.Details((long?)null);

            // Assert
            Assert.IsNotNull(result);
            HttpStatusCodeResult r = (HttpStatusCodeResult)result;
            Assert.AreEqual(r.StatusCode, 400);
        }


        #endregion

        #region Delete
        [TestMethod]
        public void DeleteGoodID()
        {
            // Arrange
            Note retrievedNote = dataRepo.NoteRepository.GetNoteById(note1.NoteId);
            SetTestUserName(notebook1.Email);

            // Act
            ViewResult result = controller.Delete(note1.NoteId) as ViewResult;

            // Assert
            Assert.AreEqual("Delete", result.ViewName);
            Assert.AreSame(retrievedNote, (Note)result.Model);
        }

        [TestMethod]
        // Delete a note with a bad Id, that is one that does not exist
        public void DeleteBadID()
        {
            // Arrange
            long? preCurrentNote = badId;
            SetTestUserName(notebook1.Email);

            // Act
            ActionResult result = controller.Delete(preCurrentNote);

            // Assert
            Assert.IsNotNull(result);
            HttpStatusCodeResult r = (HttpStatusCodeResult)result;
            Assert.AreEqual(r.StatusCode, 404);
        }

        [TestMethod]
        // Edit a note with no Id
        public void DeleteNoID()
        {
            // Arrange

            // Act
            ActionResult result = controller.Delete((long?)null);

            // Assert
            Assert.IsNotNull(result);
            HttpStatusCodeResult r = (HttpStatusCodeResult)result;
            Assert.AreEqual(r.StatusCode, 400);
        }

        [TestMethod]
        public void DeleteConfirmed()
        {
            // Arrange
            Note retrievedNote = dataRepo.NoteRepository.GetNoteById(note1.NoteId);

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.DeleteConfirmed(note1.NoteId);

            // Assert
            Assert.AreEqual("Notebook", result.RouteValues["action"].ToString());
            Assert.IsFalse(noteList.Contains(note1));
        }


        #endregion


        #region Supporting Functions
        private void SetTestUserName(string testUser)
        {
            var controllerContext = new Mock<ControllerContext>();
            var mock = new Mock<IPrincipal>();
            mock.SetupGet(princ => princ.Identity.Name).Returns(testUser);
            controllerContext.SetupGet(ctx => ctx.HttpContext.User).Returns(mock.Object);
            controller.ControllerContext = controllerContext.Object;
        }

        private void SetIsAutorized(bool value)
        {
            var controllerContext = new Mock<ControllerContext>();
            var mock = new Mock<IPrincipal>();
            mock.SetupGet(iP => iP.Identity.IsAuthenticated).Returns(value);
            controllerContext.SetupGet(ctx => ctx.HttpContext.User).Returns(mock.Object);
            controller.ControllerContext = controllerContext.Object;
        }

        #endregion
    }

}

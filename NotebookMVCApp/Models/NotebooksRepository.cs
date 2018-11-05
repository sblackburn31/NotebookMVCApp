/// <project>Notebook App</project>
/// <Version>1.0.0</Version>
/// <author>David Scott Blackburn</author>
/// <summary>
/// This class defines how the notebook data structure is defined.  In this case it is an interface between Entity Framework and the app.  EF connection
/// is defined by the entities class.  This class defines the methods that are exposed by INotebook interface.
/// LINQ to EF is the method used to access the stored data.
/// List<Notebook> GetNoteBooks() - Returns a list of notebooks from the entities class
/// Notebook GetNotebookByEmail(userEmail) - Returns a notebook class for the given email
/// CreateNotebook(userEmail) - Adds a notebook to the notebook list
/// UpdateNotebook(userEmail, noteBook) - Replaces the notebook item in the notebook list with the passed in noteBook
/// DeleteNotebook(userEmail, noteBook) - Removes the notebook assoicated with the user id from the notebook
/// Save() - Commits changes to the notebook list
/// </summary>
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NotebookMVCApp.Models
{
    public class NotebookRepository : INotebook
    {
        NotebooksEntities entities = null;

        public NotebookRepository(NotebooksEntities entities)
        {
            this.entities = entities;
        }

        #region INotebook Members
        public NotebookRepository()
        {
            entities = new NotebooksEntities();
        }

        public List<NoteBook> GetNoteBooks()
        {
            return entities.NoteBooks.ToList();
        }

        public NoteBook GetNotebookByEmail(string userEmail)
        {
            var qry = from n in entities.NoteBooks
                      where n.Email == userEmail
                      select n;
            return qry.FirstOrDefault(); 
        }

        public void CreateNotebook(string userEmail)
        {
            NoteBook noteBook = new NoteBook();
            noteBook.Email = userEmail;
            noteBook.CreateDate = DateTime.Now;

            entities.NoteBooks.Add(noteBook);
        }

        public void UpdateNotebook(string userEmail, NoteBook noteBook)
        {
            NoteBook n = GetNotebookByEmail(userEmail);
            n = noteBook;
        }

        public void DeleteNotebook(string userEmail)
        {
            NoteBook n = GetNotebookByEmail( userEmail);
            entities.NoteBooks.Remove(n);
        }

        public void Save()
        {
            entities.SaveChanges();
        }
        #endregion
    }
}
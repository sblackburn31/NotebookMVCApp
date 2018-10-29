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
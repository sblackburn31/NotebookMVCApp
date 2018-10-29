using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotebookMVCApp.Models;

namespace NotebookMVCApp.Tests.Models
{
    class DummyNotebooksRepository : INotebook
    {
        // Master list of books that will mimic the persitent database storage
        List<NoteBook> notebookList = null;

        public DummyNotebooksRepository(List<NoteBook> nb)
        {
            notebookList = nb;
        }

        public List<NoteBook> GetAllBooks()
        {
            return notebookList;
        }

        public NoteBook GetNotebookByEmail(string userEmail)
        {
            return notebookList.SingleOrDefault(book => book.Email == userEmail);
        }

        public void CreateNotebook(string userEmail)
        {
            NoteBook noteBook = new NoteBook();
            noteBook.Email = userEmail;
            noteBook.CreateDate = DateTime.Now;

            notebookList.Add(noteBook);
        }

        public void UpdateNotebook(string userEmail, NoteBook noteBook)
        {

            NoteBook bookToUpdate = notebookList.SingleOrDefault(b => b.Email == userEmail);
            bookToUpdate = noteBook;
        }

        public void DeleteNotebook(string userEmail)
        {
            NoteBook n = GetNotebookByEmail(userEmail);
            notebookList.Remove(n);
        }

        public void Save()
        {
            // Nothing to do here
        }
    }
}
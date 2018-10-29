using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ScottASPMVCApp1.Models
{

    public class NoteRepository : INote
    {
        NotebooksEntities entities = null;

        public NoteRepository(NotebooksEntities entities)
        {
            this.entities = entities;
        }

        public List<Note> GetAllNotes()
        {
            return entities.Notes.ToList();
        }

        public Note GetNoteById(long id)
        {
            return entities.Notes.SingleOrDefault(n => n.NoteId == id);
        }



        public Note GetNoteById(long id, string userId)
        {
            // return entities.Notes.Find(id);
            NoteBook nbk = entities.NoteBooks.SingleOrDefault(nb => nb.Email == userId) ;
            long notebookId = 0;
            if (nbk != null)
            {
                notebookId = nbk.NotebookId;
            }
            return entities.Notes.SingleOrDefault(n => n.NoteId == id && n.NoteBookId == notebookId);
        }

        public void AddNote(Note note)
        {
            entities.Notes.Add(note);
        }

        public void UpdateNote(long id, Note note)
        {
            Note n = GetNoteById(id);
            n = note;
            //entities.Entry(n).State = EntityState.Modified;
            
            
        }

        public void DeleteNote(Note note)
        {
            entities.Notes.Remove(note);
        }

        public void Save()
        {
            entities.SaveChanges();
        }
    }

}
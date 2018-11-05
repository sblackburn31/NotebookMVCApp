/// <project>Notebook App</project>
/// <Version>1.0.0</Version>
/// <author>David Scott Blackburn</author>
/// <summary>
/// This class defines how the note data structure is defined.  In this case it is an interface between Entity Framework and the app.  EF connection
/// is defined by the entities class.  This class defines the methods that are exposed by INote interface.
/// LINQ to EF is the method used to access the stored data.
/// List<Note> GetAllNotes() - Returns a list of notes from the entities class
/// Notebook GetNoteById(id) - Returns a note object for the given id
/// Notebook GetNoteById(id, email) - Returns a note object for the given id and email
/// AddNote(Note) - Adds Note to the note list
/// UpdateNote(id, note) - Replaces the note retrieved by id with the passed in note
/// DeleteNote(note) - Removes the note from the notelist
/// Save() - Commits changes to the note list
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NotebookMVCApp.Models
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
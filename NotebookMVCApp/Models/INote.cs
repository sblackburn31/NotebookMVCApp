/// <project>Notebook App</project>
/// <Version>1.0.0</Version>
/// <author>David Scott Blackburn</author>
/// <summary>
/// This interface will give define a contract for CRUD operations on Note entity
/// </summary>
using System;
using System.Collections.Generic;

namespace NotebookMVCApp.Models
{
    public interface INote
    {
        List<Note> GetAllNotes();
        Note GetNoteById(long id);
        Note GetNoteById(long id, string userId);
        void AddNote(Note note);
        void UpdateNote(long id, Note note);
        void DeleteNote(Note note);
        void Save();
    }
}

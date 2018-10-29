using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottASPMVCApp1.Models
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

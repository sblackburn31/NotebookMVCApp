using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottASPMVCApp1.Models;

namespace ScottASPMVCApp1.Tests.Models
{
    public class DummyNoteRepository : INote
    {

        // Master list of notes that will mimic the persitent database storage
        List<Note> noteList = null;

        DummyNotebooksRepository notebooksRepo = null;
        List<NoteBook> notebooks;

        public DummyNoteRepository(List<Note> notes)
        {
            this.noteList = notes;
        }

        public void SetNotebook(List<NoteBook> nb)
        {
            notebooks = nb;
        }

        public List<Note> GetAllNotes()
        {
            return noteList;
        }

        public Note GetNoteById(long id)
        {
            return noteList.SingleOrDefault(n => n.NoteId == id);
        }

        public Note GetNoteById(long id, string userid)
        {
            
            NoteBook nbk = notebooks.SingleOrDefault(nb => nb.Email == userid);
            long nbid = nbk == null ? 0 : nbk.NotebookId;
            return noteList.SingleOrDefault(n => n.NoteId == id && nbid == n.NoteBookId);
        }

        public void AddNote(Note note)
        {
            noteList.Add(note);
        }

        public void UpdateNote(long id, Note note)
        {
            Note noteToUpdate = noteList.SingleOrDefault(b => b.NoteBookId == id);
            noteToUpdate = note;
        }

        public void DeleteNote(Note note)
        {
            noteList.Remove(note);
        }

        public void Save()
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottASPMVCApp1.Models
{
    public interface INotebook
    {
        NoteBook GetNotebookByEmail(string userEmail);
        void CreateNotebook(string userEmail);
        void UpdateNotebook(string userEmail, NoteBook noteBook);
        void DeleteNotebook(string userEmail);
        void Save();
    }
}

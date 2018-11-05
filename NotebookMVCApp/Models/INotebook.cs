/// <project>Notebook App</project>
/// <Version>1.0.0</Version>
/// <author>David Scott Blackburn</author>
/// <summary>
/// This interface will give define a contract for CRUD operations on Notebook entity
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookMVCApp.Models
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

﻿/// <project>Notebook App</project>
/// <Version>1.0.0</Version>
/// <author>David Scott Blackburn</author>
/// <summary>
/// DataRepository class
/// This class is used to centralize the access to the data repositories used by the application.  This class makes it much easier to control access
/// to multiple repositories and to unit test the system.
/// For a full explanation for the thoughts behind using this method of data access control please refer to the following URL
/// https://www.codeproject.com/Articles/576681/UnderstandingplusandplusImplementingplusRepository
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotebookMVCApp.Models
{
    public class DataRepository : IDisposable
    {
        private NotebooksEntities entities = null;

        // This will be called from controller default constructor
        public DataRepository()
        {
            entities = new NotebooksEntities();
            NotebookRepository = new  NotebookRepository(entities);
            NoteRepository = new NoteRepository(entities);
        }

        // This will be created from test project and passed on to the
        // controllers parameterized constructors
        public DataRepository(INotebook nb, INote n)
        {
            NotebookRepository = nb;
            NoteRepository = n;
        }

        public INotebook NotebookRepository
        {
            get;
            private set;
        }

        public INote NoteRepository
        {
            get;
            private set;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                entities = null;
            }
        }

        ~DataRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}

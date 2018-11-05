/// <project>Notebook App</project>
/// <Version>1.0.0</Version>
/// <author>David Scott Blackburn</author>
/// <summary>
/// This class is used to define the Note and Notebook Metadata classes.  Refer to the Metadata class for a fuller explanation.
/// </summary>
using System;
using System.ComponentModel.DataAnnotations;

namespace NotebookMVCApp.Models
{
    [MetadataType(typeof(NoteMetadata))]
    public partial class Note
    {
    }

    [MetadataType(typeof(NoteBookMetadata ))]
    public partial class NoteBook
    {
    }
}
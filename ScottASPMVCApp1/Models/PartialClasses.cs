using System;
using System.ComponentModel.DataAnnotations;

namespace ScottASPMVCApp1.Models
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
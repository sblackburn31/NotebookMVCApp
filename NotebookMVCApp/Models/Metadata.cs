/// <project>Notebook App</project>
/// <Version>1.0.0</Version>
/// <author>David Scott Blackburn</author>
/// <summary>
/// The classes here are used to more fully define the Note and Notebook properties.  These definitions are used to define the validatations, and field
/// names used by the apps views.  The raw Note and Notebook classes should not be changed because they are entity framework generated classes and 
/// can be overwritten therefore those classes should not be directly modified, so the metadata classes along with the PartialClasses are used to fully 
/// describe the entity framework classes.
/// </summary>
using System;
using System.ComponentModel.DataAnnotations;

namespace NotebookMVCApp.Models
{
    public class NoteMetadata
    {
        [Required(ErrorMessage = "The note requires a subject.")]
        [StringLength(80, ErrorMessage = "Subject cannot be longer than 80 characters.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "You must enter a value.  Cancel to quit without saving.")]
        [StringLength(500, ErrorMessage = "Text cannot be larger than 500 characters.")]
        [Display(Name = "Text")]
        public string NoteText { get; set; }

        [Display(Name = "Created")]
        public System.DateTime CreateDate { get; set; }

        [Display(Name = "Updated")]
        public System.DateTime UpdateDate { get; set; }
    }

    public partial class NoteBookMetadata
    {
        [Required]
        [StringLength(256, ErrorMessage = "Email cannot be longer than 256 characters.")]
        public string Email { get; set; }

        [Display(Name = "Notebook Created On")]
        public System.DateTime CreateDate { get; set; }

    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
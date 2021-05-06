using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MitProjectAssignment.Models
{
    public class AddNotesViewModels
    {

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Note Title is too long")]
        [DisplayName("Title *")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Category *")]
        public int Category { get; set; }

        [DisplayName("Display Picture")]
        public HttpPostedFileBase DisplayPicture { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Upload Notes *")]
        public HttpPostedFileBase[] UploadNotes { get; set; }

        [DisplayName("Type")]
        public Nullable<int> NoteType { get; set; }

        [DisplayName("Number of Pages")]
        [RegularExpression("[0-9]*", ErrorMessage = "Only numeric entry allowed")]
        public int? NumberofPages { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Description *")]
        public string Description { get; set; }

        [DisplayName("Institution Name")]
        [MaxLength(200, ErrorMessage = "University information is too long")]
        public string UniversityName { get; set; }

        public Nullable<int> Country { get; set; }

        [DisplayName("Course Name")]
        [MaxLength(100, ErrorMessage = "Course name is too long")]
        public string Course { get; set; }

        [DisplayName("Course Code")]
        [MaxLength(100, ErrorMessage = "Course code is too long")]
        public string CourseCode { get; set; }

        [DisplayName("Proffesor / Lecturer")]
        [MaxLength(100, ErrorMessage = "Professor name is too long")]
        public string Professor { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public bool IsPaid { get; set; }

        [DisplayName("Sell Price *")]
        public Nullable<decimal> SellingPrice { get; set; }

        [DisplayName("Notes Preview")]
        public HttpPostedFileBase NotesPreview { get; set; }

        public IEnumerable<NoteCategories> NoteCategoryList { get; set; }

        public IEnumerable<NoteTypes> NoteTypeList { get; set; }

        public IEnumerable<Countrie> CountryList { get; set; }
    }
}
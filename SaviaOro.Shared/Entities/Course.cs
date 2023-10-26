using System.ComponentModel.DataAnnotations;

namespace SaviaOro.Shared.Entities
{
    public class Course
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        [MaxLength(75, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Title { get; set; }

        [Display(Name = "Descripción corta")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ShortDescription { get; set; }

        [Display(Name = "Descripción larga")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LargeDescription { get; set; }

        public DateTime? CreationDate { get; set; } = null;

        public DateTime? ModificationDate { get; set; } = null;

        public string Photo { get; set; }

        //TODO
        public string PhotoFullPath => string.IsNullOrEmpty(Photo)
            ? "https://api.lousoftware.eu/images/noimage.png"
            : $"https://api.lousoftware.eu/{Photo.Substring(1)}";

        public bool Active { get; set; }

        public ICollection<Content> Contents { get; set; }
    }
}

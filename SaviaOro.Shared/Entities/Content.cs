using System.ComponentModel.DataAnnotations;

namespace SaviaOro.Shared.Entities
{
    public class Content
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

        public Course Course { get; set; }

        public int CourseId { get; set; }
    }
}

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

        [Display(Name = "Título")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ShortDescription { get; set; }

        [Display(Name = "Título")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LargeDescription { get; set; }

        public string Photo { get; set; }

        public bool Active { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ProvaPortal.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "{0} obrigatório!")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "{0} deve ter no mínimo {2} e no máximo {1} caracteres.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "{0} obrigatória!")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "{0} deve ter no mínimo {2} e no máximo {1} caracteres.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}

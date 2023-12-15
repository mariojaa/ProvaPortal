using System.ComponentModel.DataAnnotations;

namespace ProvaPortal.Models.Enum
{
    public enum Periodo : int
    {
        [Display(Name = "1º")]
        Primeiro = 0,
        [Display(Name = "2º")]
        Segundo = 1,
        [Display(Name = "3º")]
        Terceiro = 2,
        [Display(Name = "4º")]
        Quarto = 3,
        [Display(Name = "5º")]
        Quinto = 4,
        [Display(Name = "6º")]
        Sexto = 5,
        [Display(Name = "7º")]
        Setimo = 6,
        [Display(Name = "8º")]
        Oitavo = 7,
        [Display(Name = "9º")]
        Nono = 8,
        [Display(Name = "10º")]
        Decimo = 9,
    }
}

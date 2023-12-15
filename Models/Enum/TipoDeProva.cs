using System.ComponentModel.DataAnnotations;

namespace ProvaPortal.Models.Enum
{
    public enum TipoDeProva : int
    {
        [Display(Name = ("Prova A"))]
        ProvaA = 0,
        [Display(Name = ("Prova B"))]
        ProvaB = 1,
        [Display(Name =("Prova C"))]
        ProvaC = 2,
        [Display(Name =("Prova D"))]
        ProvaD = 3
    }
}
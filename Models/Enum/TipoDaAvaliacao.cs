using System.ComponentModel.DataAnnotations;

namespace ProvaPortal.Models.Enum
{
    public enum TipoDaAvaliacao : int
    {
        [Display(Name = "Av 1")]
        AV1 = 0,
        [Display(Name = "Av 2")]
        AV2 = 1,
        [Display(Name = "Av 3")]
        AV3 = 2,
        [Display(Name = "Av 4")]
        AV4 = 3,
        [Display(Name = "Final")]
        Final = 4,
        [Display(Name = ("Teste"))]
        Teste = 5,
        [Display(Name =("Outra"))]
        Outra = 6
    }
}
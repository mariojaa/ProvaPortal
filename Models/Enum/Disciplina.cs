using System.ComponentModel.DataAnnotations;

namespace ProvaPortal.Models.Enum
{
    public enum Disciplina : int
    {
        [Display(Name = "Arquitetura de Redes")]
        ArquiteturaDeRedes = 0,
        [Display(Name = "Programação I")]
        Programacao1 = 1,
        [Display(Name = "Programação II")]
        Programacao2 = 2,
        [Display(Name = "Banco de Dados")]
        BancoDeDados = 3,
        [Display(Name = "Mapa Mundi")]
        MapaMundi = 4,
        [Display(Name = "Calculo I")]
        Calculo = 5,
        [Display(Name = "Calculo II")]
        Calculo2 = 6,
    }
}
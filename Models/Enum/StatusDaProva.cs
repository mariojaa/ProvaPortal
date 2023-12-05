using System.ComponentModel.DataAnnotations;

namespace ProvaPortal.Models.Enum
{
    public enum StatusDaProva : int
    {
        [Display(Name = "Enviada")]
        Enviado = 0,

        [Display(Name = "Aguardando Impressão")]
        AguardandoImpressao = 1,

        [Display(Name = "Deletada")]
        Deletado = 2,

        [Display(Name = "Recebida")]
        Recebida = 3,

        [Display(Name = "Prova Impressa")]
        Impressa = 4,
    }
}

using System.ComponentModel.DataAnnotations;

namespace ProvaPortal.Models.Enum
{
    public enum Curso : int
    {
        [Display(Name = "Sistemas de Informação")]
        Sistemas = 0,

        [Display(Name = "Engenharia de Produção")]
        EngenhariadeProducao = 1,

        [Display(Name = "Geografia")]
        Geografia = 2,

        [Display(Name = "Direito Noturno")]
        DireitoNoite = 3,

        [Display(Name = "Direito Matutino")]
        DireitoManha = 4,
    }

}

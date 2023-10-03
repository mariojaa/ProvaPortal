using System;
using System.ComponentModel.DataAnnotations;

public class ProvaModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }

    [Required]
    public string CaminhoArquivo { get; set; }

    public DateTime DataEnvio { get; set; }
}

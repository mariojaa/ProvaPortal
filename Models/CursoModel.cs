namespace ProvaPortal.Models
{
    public class CursoModel
    {
        public int Id { get; set; }
        public string NomeDoCurso { get; set; }
        public int PeriodoId { get; set; }

        // Relacionamento entre Curso e Professor
        public int ProfessorId { get; set; }
        public ProfessorModel Professor { get; set; }

        // Relacionamento entre Curso e Periodo
        public List<PeriodoModel> PeriodoModels { get; set; }

    }
}

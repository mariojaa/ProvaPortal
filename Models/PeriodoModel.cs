namespace ProvaPortal.Models
{
    public class PeriodoModel
    {
        public int Id { get; set; }
        public int Periodo { get; set; }
        public int DisciplinaId { get; set; }

        // Relacionamento entre Periodo e Curso
        public int CursoModelId { get; set; }
        public CursoModel Curso { get; set; }

        // Relacionamento entre Periodo e Disciplina
        public List<DisciplinaModel> DisciplinaModels { get; set; }
    }
}
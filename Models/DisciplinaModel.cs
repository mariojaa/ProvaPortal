namespace ProvaPortal.Models
{
    public class DisciplinaModel
    {
        public int Id { get; set; }
        public string DisciplinaProfessor { get; set; }
        public int PeriodoModelId { get; set; }

        // Relacionamento entre Disciplina e Periodo
        public PeriodoModel Periodo { get; set; }
    }
}
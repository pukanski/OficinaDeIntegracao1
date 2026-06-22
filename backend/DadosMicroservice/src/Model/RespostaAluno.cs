using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DadosAPI.Model.Base;

namespace DadosAPI.Model
{
    [Table("Resposta_Aluno")]
    public class RespostaAluno : BaseEntity
    {
        [Required(ErrorMessage = "O ID do aluno é obrigatório.")]
        [Column("AlunoId")]
        public long AlunoId { get; set; }

        [Required(ErrorMessage = "O ID da questão é obrigatório.")]
        [Column("QuestaoId")]
        public long QuestaoId { get; set; }

        [Required(ErrorMessage = "O ID da alternativa é obrigatório.")]
        [Column("AlternativaId")]
        public long AlternativaId { get; set; }

        [MaxLength(80)]
        [Column("Disciplina")]
        public string Disciplina { get; set; }

        [MaxLength(80)]
        [Column("Vestibular")]
        public string Vestibular { get; set; }

        [Column("AnoProva")]
        public int AnoProva { get; set; }

        [MaxLength(20)]
        [Column("Dificuldade")]
        public string? Dificuldade { get; set; }

        [Column("Acertou")]
        public bool Acertou { get; set; }

        [Column("RespondidoEm")]
        public DateTime RespondidoEm { get; set; } = DateTime.UtcNow;
    }
}
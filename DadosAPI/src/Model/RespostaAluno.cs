using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DadosAPI.Model.Base;

namespace DadosAPI.Model
{
    [Table("Resposta_Aluno")]
    public class RespostaAluno : BaseEntity
    {
        [Required(ErrorMessage = "O ID do aluno é obrigatório.")]
        public long AlunoId { get; set; }

        [Required(ErrorMessage = "O ID da questão é obrigatório.")]
        public long QuestaoId { get; set; }

        [Required(ErrorMessage = "O ID da alternativa é obrigatório.")]
        public long AlternativaId { get; set; }

        // Informações desnormalizadas para evitar chamadas entre microsserviços
        // no momento da consulta do dashboard
        [MaxLength(80)]
        public string Disciplina { get; set; }

        [MaxLength(80)]
        public string Vestibular { get; set; }

        public int AnoProva { get; set; }

        [MaxLength(20)]
        public string? Dificuldade { get; set; }

        public bool Acertou { get; set; }

        public DateTime RespondidoEm { get; set; } = DateTime.UtcNow;
    }
}

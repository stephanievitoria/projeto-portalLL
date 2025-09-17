using System;
using System.ComponentModel.DataAnnotations;

namespace PortalLL.Models
{
      public enum StatusLicao
    {
        Pendente,
        Aprovada,
        Reprovada
    }
    public class LicaoAprendida
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public DateTime DataHora { get; set; }

        [Required]
        public string Equipe { get; set; }

        [Required]
        public string Software { get; set; }

        [Required]
        public string Descricao { get; set; }

        public int UsuarioId { get; set; }

        public List<string> Clarificacoes { get; set; } = new();

        public StatusLicao Status { get; set; } = StatusLicao.Pendente;
    }

  
}

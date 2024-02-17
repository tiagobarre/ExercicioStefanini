using System.ComponentModel.DataAnnotations;

namespace Questao5.Domain.Entities
{
    public class MovimentacaoContasRequest
    {
       
        [Required]
        public string IdContaCorrente { get; set; }
        public string Valor { get; set; }
        
        [MaxLength(1)]
        public string Tipo { get; set; }
    }

    public class SaldoContaRequest 
    {
        [Required]
        public string IdContaCorrente { get; set; }
    }

}

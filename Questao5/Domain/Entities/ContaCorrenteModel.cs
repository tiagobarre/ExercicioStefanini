namespace Questao5.Domain.Entities
{
    public class ContaCorrenteModel
    {
        public string idcontacorrente { get; set; }
        public int numero { get; set; }
        public string nome { get; set; }
        public int ativo { get; set; }
    }

    public class MovimentoResponse 
    {
        public string idmovimento { get; set; }
        public string idcontacorrente { get; set; }
        public string datamovimento { get; set; }
        public string tipomovimento { get; set; }
        public double valor { get; set; }

    }

    public class SaldoAtualConta 
    {
        public int NumeroConta { get; set; }
        public string Nome { get; set; }
        public string DataHoraConsulta { get; set; }
        public double SaldoAtual { get; set; }
    }


}

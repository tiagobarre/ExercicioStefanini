using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Movimentacao
{
    public interface IMovimentacao
    {
        Task<ContaCorrenteModel> GetContaCorrenteByID(string idConta);
        Task<MovimentoResponse> InsertMovimento(ContaCorrenteModel obj, MovimentacaoContasRequest mov);
        Task<SaldoAtualConta> GetSAldoAtual(string idConta);
    }
}

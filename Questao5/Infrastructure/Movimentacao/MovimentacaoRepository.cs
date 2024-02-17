using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Movimentacao
{
    public class MovimentacaoRepository : IMovimentacao
    {
        private readonly DatabaseConfig databaseConfig;
        private readonly SqliteConnection _connection;

        public MovimentacaoRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
            _connection = new SqliteConnection(databaseConfig.Name);
        }

        public async Task<ContaCorrenteModel> GetContaCorrenteByID(string idConta)
        {

            return _connection.Query<ContaCorrenteModel>($"SELECT * FROM contacorrente WHERE idcontacorrente = '{idConta}'").FirstOrDefault();         

        }

        public async Task<SaldoAtualConta> GetSAldoAtual(string idConta)
        {

            return _connection.Query<SaldoAtualConta>($"select CC.numero as NumeroConta, " +
                                                      $"CC.nome as Nome, " +
                                                      $"strftime('%d/%m/%Y %H:%M:%S', datetime('now')) as DataHoraConsulta, " +
                                                      $"CASE when MV.valor is null then 0 ELSE SUM(case when MV.tipomovimento = 'D' then -MV.valor else MV.valor end) END as SaldoAtual " +
                                                      $"from contacorrente CC " +
                                                      $"left join movimento MV on CC.idcontacorrente = MV.idcontacorrente " +
                                                      $"where CC.idcontacorrente = '{idConta}'").FirstOrDefault();

        }

        public async Task<MovimentoResponse> InsertMovimento(ContaCorrenteModel obj, MovimentacaoContasRequest mov)
        {
            try
            {                

                int idMovimento = 0;

                int Maxidmovimento = _connection.Query<int>("select case when max(idmovimento) is null then 0 else max(idmovimento) END from movimento").FirstOrDefault();

                idMovimento += Maxidmovimento == 0 ? 1 : Maxidmovimento + 1;

                _connection.Execute($"INSERT INTO movimento(idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES('{idMovimento}','{obj.idcontacorrente}', '{DateTime.Now}', '{mov.Tipo}', '{mov.Valor.Replace(",",".")}')");
                                
                return await GetUltimoMovimento(obj,mov);
            }
            catch (Exception ex)
            {

                throw ex;
            }

           

        }

        public async Task<MovimentoResponse> GetUltimoMovimento(ContaCorrenteModel obj, MovimentacaoContasRequest mov)
        {
            return _connection.Query<MovimentoResponse>($"SELECT * FROM movimento WHERE idcontacorrente = '{obj.idcontacorrente}' order by datamovimento desc").FirstOrDefault();
        }
    }
}

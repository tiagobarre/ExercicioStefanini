using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Movimentacao;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaldoContasController : Controller
    {
        public IMovimentacao _movimentacao { get; set; }

        public SaldoContasController(IMovimentacao movimentacao)
        {
            _movimentacao = movimentacao;
        }

        [HttpPost]
        public async Task<ActionResult> GetSaldoConta(SaldoContaRequest request)
        {
            try
            {
                var resp = await _movimentacao.GetContaCorrenteByID(request.IdContaCorrente);

                SaldoAtualConta respSaldo = new SaldoAtualConta();

                if (resp != null)
                {
                    if (resp.ativo == 0)
                    {
                        return BadRequest("INACTIVE_ACCOUNT: Conta está desativada.");
                    }


                    respSaldo = await _movimentacao.GetSAldoAtual(request.IdContaCorrente);

                    respSaldo.SaldoAtual = Math.Round(respSaldo.SaldoAtual, 2);

                    return Ok(respSaldo);
                }
                else
                {
                    return BadRequest("INVALID_ACCOUNT: Conta não existe.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}

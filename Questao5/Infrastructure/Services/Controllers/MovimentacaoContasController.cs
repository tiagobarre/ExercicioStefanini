using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Movimentacao;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimentacaoContasController : Controller
    {
        public IMovimentacao _movimentacao { get; set; }

        public MovimentacaoContasController(IMovimentacao movimentacao)
        {
            _movimentacao = movimentacao;
        }

        [HttpPost]
        public async Task<ActionResult> MovimentacaoConta(MovimentacaoContasRequest request)
        {
            try
            {
                if(double.Parse(request.Valor.Replace(",",".")) < 1)
                {
                    return BadRequest("INVALID_VALUE: O valor não pode ser negativo e deve ser maior que 0.");
                }

                if (request.Tipo.ToUpper() == "C" || request.Tipo.ToUpper() == "D")
                {
                    var resp = await _movimentacao.GetContaCorrenteByID(request.IdContaCorrente);

                    MovimentoResponse respMovimentacao = new MovimentoResponse();

                    if (resp != null)
                    {
                        if (resp.ativo == 0)
                        {
                            return BadRequest("INACTIVE_ACCOUNT: Conta está desativada.");
                        }

                       
                        respMovimentacao = await _movimentacao.InsertMovimento(resp, request);

                        return Ok(respMovimentacao);
                    }
                    else
                    {
                        return BadRequest("INVALID_ACCOUNT: Conta não existe.");
                    }

                    
                }
                else
                {
                    return BadRequest("INVALID_TYPE: Tipo da movimentação deve ser C - ( Crédito ) ou D - ( Débito ).");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
           
        }       


    }
}

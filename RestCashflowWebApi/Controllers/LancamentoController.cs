using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestCashflowLibrary.Domain.Business;
using RestCashflowLibrary.Domain.Model.Entity;
using RestCashflowWebApi.Model;

namespace RestCashflowWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/lancamento")]
    [Produces("application/json")]
    public class LancamentoController : ControllerBase
    {
        readonly IFinancialEntryBusiness _financialEntryBusiness;

        public LancamentoController(IFinancialEntryBusiness financialEntryBusiness)
        {
            _financialEntryBusiness = financialEntryBusiness;
        }

        /// <summary>
        /// Cria um novo lançamento financeiro
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/v1/lancamento
        ///     {
        ///       "descricao": "Pagamento de conta de luz",
        ///       "banco_destino": 5,
        ///       "conta_destino": "0001",
        ///       "tipo_de_conta": 0,
        ///       "cpf_cnpj_destino": "47.972.568/0001-38",
        ///       "tipo_da_lancamento": 1,
        ///       "encargos": "R$ 5,00",
        ///       "valor_do_lancamento": "R$ 80,00",
        ///       "data_de_lancamento":"01-01-2022"
        ///     }
        ///
        /// </remarks>
        /// <param name="lancamento">Lançamento financeiro</param>
        /// <returns>O retorno de sucesso é somente o código http 202</returns>
        /// <response code="202">Lancamento financeiro adicionado com succeso</response>
        /// <response code="400">Data do lançamento não deve ser no passado</response>
        /// <response code="422">Limite do dia em pagamentos já atingido</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpPost]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<AcceptedResult> Post([FromBody] LancamentoFinanceiro lancamento)
        {
            try
            {
                await _financialEntryBusiness.AddToQueue((FinancialEntryEntity) lancamento);
                return Accepted();
            }
            catch
            {
                throw;
            }
        }
    }
}

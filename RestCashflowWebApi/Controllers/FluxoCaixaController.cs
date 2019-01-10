using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestCashflowLibrary.Domain.Business;
using System.Linq;
using RestCashflowWebApi.Model;

namespace RestCashflowWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/fluxo-caixa")]
    [Produces("application/json")]
    public class FluxoCaixaController : ControllerBase
    {
        readonly IFinancialEntryBusiness _financialEntryBusiness;

        public FluxoCaixaController(IFinancialEntryBusiness financialEntryBusiness)
        {
            _financialEntryBusiness = financialEntryBusiness;
        }

        /// <summary>
        /// Cria o fluxo de caixa do dia e próximos 30 dias
        /// </summary>
        /// <returns>Retorna uma lista com o fluxo de caixa do dia e dos próximos 30 dias</returns>
        /// <response code="200">Succeso na geração do fluxo de caixa</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<FluxoCaixa>> Get()
        {
            try
            {
                var list = await _financialEntryBusiness.GetCashflow(DateTime.Now, 30);
                return list.ToList().ConvertAll(dto => (FluxoCaixa)dto);
            }
            catch
            {
                throw;
            }
        }
    }
}

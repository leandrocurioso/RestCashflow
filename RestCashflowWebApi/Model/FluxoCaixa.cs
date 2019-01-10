using System;
using System.Collections.Generic;
using RestCashflowLibrary.Domain.Model.DataTransferObject;
using System.Linq;
using RestCashflowLibrary.Infrastructure.Utility;

namespace RestCashflowWebApi.Model
{
    public class EntradaFluxoCaixa
    {
        /// <summary>
        /// Data da conciliação
        /// </summary>
        /// <example>01-01-2022</example>
        public string data { get; set; }
        /// <summary>
        /// Valor da entrada
        /// </summary>
        /// <example>R$ 1.000,00</example>
        public string valor { get; set; }
    }

    public class SaidaFluxoCaixa
    {
        /// <summary>
        /// Data da conciliação
        /// </summary>
        /// <example>01-01-2022</example>
        public string data { get; set; }
        /// <summary>
        /// Valor da saída
        /// </summary>
        /// <example>R$ 1.000,00</example>
        public string valor { get; set; }
    }

    public class EncargoFluxoCaixa
    {
        /// <summary>
        /// Data da conciliação
        /// </summary>
        /// <example>01-01-2022</example>
        public string data { get; set; }
        /// <summary>
        /// Valor do encargo
        /// </summary>
        /// <example>R$ 1.000,00</example>
        public string valor { get; set; }
    }

    [Serializable]
    public class FluxoCaixa
    {
        /// <summary>
        /// Data
        /// </summary>
        /// <example>01-01-2022</example>
        public string data { get; set; }
        /// <summary>
        /// Lista de entradas
        /// </summary>
        public IEnumerable<EntradaFluxoCaixa> entradas { get; set; }
        /// <summary>
        /// Lista de saídas
        /// </summary>
        public IEnumerable<SaidaFluxoCaixa> saidas { get; set; }
        /// <summary>
        /// Lista de encargos
        /// </summary>
        public IEnumerable<EncargoFluxoCaixa> encargos { get; set; }
        /// <summary>
        /// Total dos lançamentos (Entrada + Saída + Encargos)
        /// </summary>
        /// <example>R$ 1.000,00</example>
        public string total { get; set; }
        /// <summary>
        /// Porcentagem de crescimento ou queda comparado com o dia anterior
        /// Caso não haja registro para o dia anterior o crescimento é encarado como 100.00%
        /// </summary>
        /// <example>0.5%</example>
        public string posicao_do_dia { get; set; }

        public static explicit operator FluxoCaixa(CashflowDataTransferObject dto)
        {
            return new FluxoCaixa()
            {
                data = dto.Date.ToString("dd-MM-yyyy"),
                entradas = dto.Entries.ToList().ConvertAll(x => new EntradaFluxoCaixa()
                {
                    data = dto.Date.ToString("dd-MM-yyyy"),
                    valor = FinancialConverter.ToRealFormat(x.Value)
                }),
                saidas = dto.Outs.ToList().ConvertAll(x => new SaidaFluxoCaixa()
                {
                    data = dto.Date.ToString("dd-MM-yyyy"),
                    valor = FinancialConverter.ToRealFormat(x.Value)
                }),
                encargos = dto.Charges.ToList().ConvertAll(x => new EncargoFluxoCaixa()
                {
                    data = dto.Date.ToString("dd-MM-yyyy"),
                    valor = FinancialConverter.ToRealFormat(x.Value)
                }),
                total = FinancialConverter.ToRealFormat(dto.Total),
                posicao_do_dia = FinancialConverter.ToStringPercentage(dto.DayPosition)
            };
        }
    }
}

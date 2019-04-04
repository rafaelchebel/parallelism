using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tasks._01.Model;
using Tasks._01.Repository;

namespace Tasks._01.Services
{
    public class ContaClienteService
    {
        ContaClienteRepository contaClienteRepository = null;

        public ContaClienteService()
        {
            contaClienteRepository = new ContaClienteRepository();
        }

        public IEnumerable<ContaCliente> GetContaClientes()
        {
            return contaClienteRepository.GetContaClientes();
        }

        public string ConsolidarMovimentacao(ContaCliente conta)
        {
            return ConsolidarMovimentacao(conta, CancellationToken.None);
        }

        public string ConsolidarMovimentacao(ContaCliente conta, CancellationToken ct)
        {
            var soma = 0m;

            foreach (var movimento in conta.Movimentacoes)
            {
                ct.ThrowIfCancellationRequested();
                soma += movimento.Valor * FatorDeMultiplicacao(movimento.Data);
            }

            ct.ThrowIfCancellationRequested();
            AtualizarInvestimentos(conta);
            return $"Cliente {conta.NomeCliente} tem saldo atualizado de R${soma.ToString("#00.00")}";
        }

        private static decimal FatorDeMultiplicacao(DateTime dataMovimento)
        {
            const decimal CTE_FATOR = 1.0000000005m;

            var diasCorridosDesdeDataMovimento = (dataMovimento - new DateTime(1900, 1, 1)).Days;
            var resultado = 1m;

            for (int i = 0; i < diasCorridosDesdeDataMovimento * 2; i++)
                resultado = resultado * CTE_FATOR;

            return resultado;
        }

        private static void AtualizarInvestimentos(ContaCliente cliente)
        {
            const decimal CTE_BONIFICACAO_MOV = 1m / (10m * 5m);
            cliente.Investimento *= CTE_BONIFICACAO_MOV;
        }

        public async Task<string[]> ConsolidarContas(IEnumerable<ContaCliente> contas, CancellationToken ct)
        {
            var tasks = contas.Select(conta =>
                Task.Factory.StartNew(() =>
                {
                    ct.ThrowIfCancellationRequested();

                    var resultadoConsolidacao = ConsolidarMovimentacao(conta, ct);

                    ct.ThrowIfCancellationRequested();
                    return resultadoConsolidacao;
                }, ct)
            );

            return await Task.WhenAll(tasks);
        }
    }
}

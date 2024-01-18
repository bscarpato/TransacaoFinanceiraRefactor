using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace TransacaoFinanceira
{
    class Program
    {

        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddScoped<IAcessoDados, AcessoDados>()
            .AddScoped<IExecutarTransacaoFinanceira, ExecutarTransacaoFinanceira>()
            .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var executor = serviceProvider.GetService<IExecutarTransacaoFinanceira>();

                var transacoes = new[] {
                                    new Transacao { CorrelationId= 1,DateTime="09/09/2023 14:15:00", ContaOrigem= 938485762, ContaDestino= 2147483649, Valor= 150},
                                    new Transacao { CorrelationId= 2,DateTime="09/09/2023 14:15:05", ContaOrigem= 2147483649, ContaDestino= 210385733, Valor= 149},
                                    new Transacao { CorrelationId= 3,DateTime="09/09/2023 14:15:29", ContaOrigem= 347586970, ContaDestino= 238596054, Valor= 1100},
                                    new Transacao { CorrelationId= 4,DateTime="09/09/2023 14:17:00", ContaOrigem= 675869708, ContaDestino= 210385733, Valor= 5300},
                                    new Transacao { CorrelationId= 5,DateTime="09/09/2023 14:18:00", ContaOrigem= 238596054, ContaDestino= 674038564, Valor= 1489},
                                    new Transacao { CorrelationId= 6,DateTime="09/09/2023 14:18:20", ContaOrigem= 573659065, ContaDestino= 563856300, Valor= 49},
                                    new Transacao { CorrelationId= 7,DateTime="09/09/2023 14:19:00", ContaOrigem= 938485762, ContaDestino= 2147483649, Valor= 44},
                                    new Transacao { CorrelationId= 8,DateTime="09/09/2023 14:19:01", ContaOrigem= 573659065, ContaDestino= 675869708, Valor= 150},
                                    // Transação entre mesma conta
                                    new Transacao { CorrelationId= 9,DateTime="09/09/2023 14:19:01", ContaOrigem= 573659065, ContaDestino= 573659065, Valor= 10},
                                    // Transação repetida
                                    new Transacao { CorrelationId= 9,DateTime="09/09/2023 14:19:01", ContaOrigem= 573659065, ContaDestino= 573659065, Valor= 10},
                                    // Transação normal
                                    new Transacao { CorrelationId= 10, DateTime="09/09/2023 14:20:00", ContaOrigem= 347586970, ContaDestino= 938485762, Valor= 200},
                                    // Transação com valor negativo
                                    new Transacao { CorrelationId= 11, DateTime="09/09/2023 14:22:00", ContaOrigem= 938485762, ContaDestino= 347586970, Valor= -100},
                                    // Transação com conta de origem inválida (não existe na lista de saldos)
                                    new Transacao { CorrelationId= 12, DateTime="09/09/2023 14:24:00", ContaOrigem= 999999999, ContaDestino= 938485762, Valor= 200},
                                    // Transação com valor zero
                                    new Transacao { CorrelationId= 13, DateTime="09/09/2023 14:25:00", ContaOrigem= 238596054, ContaDestino= 675869708, Valor= 0},
                };


                Parallel.ForEach(transacoes, item =>
                {
                    executor.Transferir(item.CorrelationId, item.ContaOrigem, item.ContaDestino, item.Valor);
                });
            }

        }
    }

}

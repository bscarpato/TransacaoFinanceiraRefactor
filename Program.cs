using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TransacaoFinanceira
{
    class Program
    {

        static void Main(string[] args)
        {
            var transacoes = new[] {
                                    new Transacao { CorrelationId= 1,DateTime="09/09/2023 14:15:00", ContaOrigem= 938485762, ContaDestino= 2147483649, Valor= 150},
                                    new Transacao { CorrelationId= 2,DateTime="09/09/2023 14:15:05", ContaOrigem= 2147483649, ContaDestino= 210385733, Valor= 149},
                                    new Transacao { CorrelationId= 3,DateTime="09/09/2023 14:15:29", ContaOrigem= 347586970, ContaDestino= 238596054, Valor= 1100},
                                    new Transacao { CorrelationId= 4,DateTime="09/09/2023 14:17:00", ContaOrigem= 675869708, ContaDestino= 210385733, Valor= 5300},
                                    new Transacao { CorrelationId= 5,DateTime="09/09/2023 14:18:00", ContaOrigem= 238596054, ContaDestino= 674038564, Valor= 1489},
                                    new Transacao { CorrelationId= 6,DateTime="09/09/2023 14:18:20", ContaOrigem= 573659065, ContaDestino= 563856300, Valor= 49},
                                    new Transacao { CorrelationId= 7,DateTime="09/09/2023 14:19:00", ContaOrigem= 938485762, ContaDestino= 2147483649, Valor= 44},
                                    new Transacao { CorrelationId= 8,DateTime="09/09/2023 14:19:01", ContaOrigem= 573659065, ContaDestino= 675869708, Valor= 150},
                                    new Transacao { CorrelationId= 9,DateTime="09/09/2023 14:19:01", ContaOrigem= 573659065, ContaDestino= 573659065, Valor= 10},
                                    new Transacao { CorrelationId= 9,DateTime="09/09/2023 14:19:01", ContaOrigem= 573659065, ContaDestino= 573659065, Valor= 10},

            };


            ExecutarTransacaoFinanceira executor = new ExecutarTransacaoFinanceira();
            Parallel.ForEach(transacoes, item =>
            {
                executor.Transferir(item.CorrelationId, item.ContaOrigem, item.ContaDestino, item.Valor);
            });

        }
    }

    class Transacao
    {
        public int CorrelationId { get; set; }
        public string DateTime { get; set; }
        public long ContaOrigem { get; set; }
        public long ContaDestino { get; set; }
        public decimal Valor { get; set; }
    }

    class ExecutarTransacaoFinanceira : AcessoDados
    {
        public void Transferir(int correlationId, long contaOrigem, long contaDestino, decimal valor)
        {
            if (contaOrigem <= 0 || contaDestino <= 0)
            {
                Console.WriteLine($"Transacao numero {correlationId} é inválida devido a contas de origem/destino inválidas.");
                return;
            }

            if (valor <= 0)
            {
                Console.WriteLine($"Transacao numero {correlationId} é inválida devido a um valor de transação negativo ou zero.");
                return;
            }

            lock (lockObj)
            {
                if (TransacaoFoiProcessada(correlationId))
                {
                    Console.WriteLine($"Transacao numero {correlationId} já foi processada anteriormente.");
                    return;
                }

                ContaSaldo contaSaldoOrigem = GetSaldoById(contaOrigem);
                if (contaSaldoOrigem == null)
                {
                    Console.WriteLine($"Transacao numero {correlationId} foi cancelada porque a conta de origem não foi encontrada.");
                    return;
                }

                if (contaSaldoOrigem.Saldo < valor)
                {
                    Console.WriteLine($"Transacao numero {correlationId} foi cancelada por falta de saldo.");
                    return;
                }

                ContaSaldo contaSaldoDestino = GetSaldoById(contaDestino);
                if (contaSaldoDestino == null)
                {
                    Console.WriteLine($"Transacao numero {correlationId} foi cancelada porque a conta de destino não foi encontrada.");
                    return;
                }

                contaSaldoOrigem.Saldo -= valor;
                contaSaldoDestino.Saldo += valor;

                AtualizarSaldo(contaSaldoOrigem);
                AtualizarSaldo(contaSaldoDestino);

                Console.WriteLine($"Transacao numero {correlationId} foi efetivada com sucesso! Novos saldos: Conta Origem: {contaSaldoOrigem.Saldo} | Conta Destino: {contaSaldoDestino.Saldo}");
            }
        }
    }
    class ContaSaldo
    {
        public long Conta { get; set; }
        public decimal Saldo { get; set; }

        public ContaSaldo(long conta, decimal saldo)
        {
            Conta = conta;
            Saldo = saldo;
        }
    }


    class AcessoDados
    {
        protected static readonly object lockObj = new();
        private List<ContaSaldo> tabelaSaldos;
        private HashSet<int> transacoesProcessadas;

        public AcessoDados()
        {
            tabelaSaldos = new List<ContaSaldo>()
            {
                new ContaSaldo(938485762, 180),
                new ContaSaldo(347586970, 1200),
                new ContaSaldo(2147483649, 0),
                new ContaSaldo(675869708, 4900),
                new ContaSaldo(238596054, 478),
                new ContaSaldo(573659065, 787),
                new ContaSaldo(210385733, 10),
                new ContaSaldo(674038564, 400),
                new ContaSaldo(563856300, 1200)
            };

            transacoesProcessadas = new HashSet<int>();

        }
        public ContaSaldo GetSaldoById(long id)
        {
            return tabelaSaldos.Find(x => x.Conta == id);
        }


        public void AtualizarSaldo(ContaSaldo contaSaldo)
        {
            try
            {
                lock (lockObj)
                {
                    tabelaSaldos.RemoveAll(x => x.Conta == contaSaldo.Conta);
                    tabelaSaldos.Add(contaSaldo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool TransacaoFoiProcessada(int correlationId)
        {
            lock (lockObj)
            {
                if (transacoesProcessadas.Contains(correlationId))
                {
                    return true;
                }
                transacoesProcessadas.Add(correlationId);
                return false;
            }
        }

    }
}

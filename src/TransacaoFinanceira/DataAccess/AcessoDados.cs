
using System.Collections.Generic;
using System;

class AcessoDados : IAcessoDados
{
    public object LockObj { get; } = new object();
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
            lock (LockObj)
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
        lock (LockObj)
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
using System;

public class ExecutarTransacaoFinanceira : IExecutarTransacaoFinanceira
{
    private readonly IAcessoDados _acessoDados;

    public ExecutarTransacaoFinanceira(IAcessoDados acessoDados)
    {
        _acessoDados = acessoDados;
    }

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

        lock (_acessoDados.LockObj)
        {
            if (_acessoDados.TransacaoFoiProcessada(correlationId))
            {
                Console.WriteLine($"Transacao numero {correlationId} já foi processada anteriormente.");
                return;
            }

            ContaSaldo contaSaldoOrigem = _acessoDados.GetSaldoById(contaOrigem);
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

            ContaSaldo contaSaldoDestino = _acessoDados.GetSaldoById(contaDestino);
            if (contaSaldoDestino == null)
            {
                Console.WriteLine($"Transacao numero {correlationId} foi cancelada porque a conta de destino não foi encontrada.");
                return;
            }

            contaSaldoOrigem.Saldo -= valor;
            contaSaldoDestino.Saldo += valor;

            _acessoDados.AtualizarSaldo(contaSaldoOrigem);
            _acessoDados.AtualizarSaldo(contaSaldoDestino);

            Console.WriteLine($"Transacao numero {correlationId} foi efetivada com sucesso! Novos saldos: Conta Origem: {contaSaldoOrigem.Saldo} | Conta Destino: {contaSaldoDestino.Saldo}");
        }
    }
}
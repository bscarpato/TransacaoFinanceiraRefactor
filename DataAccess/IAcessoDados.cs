internal interface IAcessoDados
{
    void AtualizarSaldo(ContaSaldo contaSaldo);
    ContaSaldo GetSaldoById(long id);
    bool TransacaoFoiProcessada(int correlationId);
    object LockObj { get; }
}
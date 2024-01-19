namespace TransacaoFinanceira.Tests.DataAccess
{
    public class AcessoDadosTests
    {
        [Fact]
        public void GetSaldoById_DeveRetornarCorretamente()
        {
            // Arrange
            var acessoDados = new AcessoDados();

            // Act
            var saldo = acessoDados.GetSaldoById(938485762);

            // Assert
            Assert.NotNull(saldo);
            Assert.Equal(938485762, saldo.Conta);
            Assert.Equal(180, saldo.Saldo);
        }

        [Fact]
        public void GetSaldoById_DeveRetornarNull_QuandoContaNaoExiste()
        {
            // Arrange
            var acessoDados = new AcessoDados();

            // Act
            var saldo = acessoDados.GetSaldoById(999999999);

            // Assert
            Assert.Null(saldo);
        }

        [Fact]
        public void AtualizarSaldo_DeveAtualizarCorretamente()
        {
            // Arrange
            var acessoDados = new AcessoDados();
            var novaContaSaldo = new ContaSaldo(938485762, 250);

            // Act
            acessoDados.AtualizarSaldo(novaContaSaldo);
            var saldoAtualizado = acessoDados.GetSaldoById(938485762);

            // Assert
            Assert.NotNull(saldoAtualizado);
            Assert.Equal(938485762, saldoAtualizado.Conta);
            Assert.Equal(250, saldoAtualizado.Saldo); // Verifica se o saldo foi atualizado
        }

        [Fact]
        public void TransacaoFoiProcessada_DeveRetornarTrueParaTransacoesProcessadas()
        {
            // Arrange
            var acessoDados = new AcessoDados();
            int correlationIdExistente = 1;
            acessoDados.TransacaoFoiProcessada(correlationIdExistente); // Simula que a transação foi processada

            // Act
            bool resultado = acessoDados.TransacaoFoiProcessada(correlationIdExistente);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void TransacaoFoiProcessada_DeveRetornarFalseParaNovasTransacoes()
        {
            // Arrange
            var acessoDados = new AcessoDados();
            int correlationIdNova = 999;

            // Act
            bool resultado = acessoDados.TransacaoFoiProcessada(correlationIdNova);

            // Assert
            Assert.False(resultado);
        }

    }
}

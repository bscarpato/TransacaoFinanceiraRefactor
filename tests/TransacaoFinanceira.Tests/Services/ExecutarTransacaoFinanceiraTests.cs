using Moq;

namespace TransacaoFinanceira.Tests.Services
{
    public class ExecutarTransacaoFinanceiraTests
    {
        [Fact]
        public void Transferir_DeveFalhar_QuandoContaOrigemNaoExistir()
        {
            // Arrange
            var acessoDadosMock = new Mock<IAcessoDados>();
            acessoDadosMock.Setup(ad => ad.LockObj).Returns(new object());

            long contaInexistenteId = 123;
            acessoDadosMock.Setup(ad => ad.GetSaldoById(contaInexistenteId)).Returns((ContaSaldo)null);
            var executor = new ExecutarTransacaoFinanceira(acessoDadosMock.Object);

            // Act
            executor.Transferir(1, contaInexistenteId, 456, 100);

            // Assert
            acessoDadosMock.Verify(ad => ad.GetSaldoById(contaInexistenteId), Times.Once);

        }


        [Fact]
        public void Transferir_DeveFalhar_QuandoContasInvalidas()
        {
            // Arrange
            var acessoDadosMock = new Mock<IAcessoDados>();
            acessoDadosMock.Setup(ad => ad.LockObj).Returns(new object());

            var executor = new ExecutarTransacaoFinanceira(acessoDadosMock.Object);
            int correlationId = 1;
            long contaOrigemInvalida = -1;
            long contaDestinoInvalida = -2;
            decimal valor = 100;

            // Act
            executor.Transferir(correlationId, contaOrigemInvalida, contaDestinoInvalida, valor);

            // Assert
            acessoDadosMock.Verify(ad => ad.GetSaldoById(It.IsAny<long>()), Times.Never);
            acessoDadosMock.Verify(ad => ad.AtualizarSaldo(It.IsAny<ContaSaldo>()), Times.Never);
        }


        [Fact]
        public void Transferir_DeveFalhar_QuandoValorInvalido()
        {
            // Arrange
            var acessoDadosMock = new Mock<IAcessoDados>();
            acessoDadosMock.Setup(ad => ad.LockObj).Returns(new object());

            var executor = new ExecutarTransacaoFinanceira(acessoDadosMock.Object);
            int correlationId = 1;
            long contaOrigem = 123;
            long contaDestino = 456;
            decimal valorInvalido = -100;

            // Act
            executor.Transferir(correlationId, contaOrigem, contaDestino, valorInvalido);

            // Assert
            acessoDadosMock.Verify(ad => ad.GetSaldoById(It.IsAny<long>()), Times.Never);
            acessoDadosMock.Verify(ad => ad.AtualizarSaldo(It.IsAny<ContaSaldo>()), Times.Never);
        }

        [Fact]
        public void Transferir_DeveFalhar_QuandoSaldoInsuficiente()
        {
            // Arrange
            var acessoDadosMock = new Mock<IAcessoDados>();
            acessoDadosMock.Setup(ad => ad.LockObj).Returns(new object());

            var saldoInsuficiente = new ContaSaldo(123, 50);
            acessoDadosMock.Setup(ad => ad.GetSaldoById(123)).Returns(saldoInsuficiente);
            var executor = new ExecutarTransacaoFinanceira(acessoDadosMock.Object);
            int correlationId = 3;
            long contaOrigem = 123;
            long contaDestino = 456;
            decimal valor = 100; // Maior que o saldo disponível

            // Act
            executor.Transferir(correlationId, contaOrigem, contaDestino, valor);

            // Assert
            acessoDadosMock.Verify(ad => ad.GetSaldoById(contaOrigem), Times.Once);
            acessoDadosMock.Verify(ad => ad.AtualizarSaldo(It.IsAny<ContaSaldo>()), Times.Never);
        }

        [Fact]
        public void Transferir_DeveSerBemSucedida_QuandoValida()
        {
            // Arrange
            var acessoDadosMock = new Mock<IAcessoDados>();
            acessoDadosMock.Setup(ad => ad.LockObj).Returns(new object());

            var contaOrigem = new ContaSaldo(123, 200);
            var contaDestino = new ContaSaldo(456, 100);
            acessoDadosMock.Setup(ad => ad.GetSaldoById(123)).Returns(contaOrigem);
            acessoDadosMock.Setup(ad => ad.GetSaldoById(456)).Returns(contaDestino);
            acessoDadosMock.Setup(ad => ad.TransacaoFoiProcessada(It.IsAny<int>())).Returns(false);
            var executor = new ExecutarTransacaoFinanceira(acessoDadosMock.Object);
            int correlationId = 4;
            decimal valor = 100;

            // Act
            executor.Transferir(correlationId, contaOrigem.Conta, contaDestino.Conta, valor);

            // Assert
            acessoDadosMock.Verify(ad => ad.GetSaldoById(contaOrigem.Conta), Times.Once);
            acessoDadosMock.Verify(ad => ad.GetSaldoById(contaDestino.Conta), Times.Once);
            acessoDadosMock.Verify(ad => ad.AtualizarSaldo(It.Is<ContaSaldo>(cs => cs.Conta == contaOrigem.Conta && cs.Saldo == 100)), Times.Once);
            acessoDadosMock.Verify(ad => ad.AtualizarSaldo(It.Is<ContaSaldo>(cs => cs.Conta == contaDestino.Conta && cs.Saldo == 200)), Times.Once);
        }

        [Fact]
        public void Transferir_DeveFalhar_QuandoTransacaoJaProcessada()
        {
            // Arrange
            var acessoDadosMock = new Mock<IAcessoDados>();
            acessoDadosMock.Setup(ad => ad.LockObj).Returns(new object());

            acessoDadosMock.Setup(ad => ad.TransacaoFoiProcessada(It.IsAny<int>())).Returns(true);
            var executor = new ExecutarTransacaoFinanceira(acessoDadosMock.Object);
            int correlationId = 5; // ID que supostamente já foi processado
            long contaOrigem = 123;
            long contaDestino = 456;
            decimal valor = 100;

            // Act
            executor.Transferir(correlationId, contaOrigem, contaDestino, valor);

            // Assert
            acessoDadosMock.Verify(ad => ad.TransacaoFoiProcessada(correlationId), Times.Once);
            acessoDadosMock.Verify(ad => ad.GetSaldoById(It.IsAny<long>()), Times.Never);
            acessoDadosMock.Verify(ad => ad.AtualizarSaldo(It.IsAny<ContaSaldo>()), Times.Never);
        }
    }
}

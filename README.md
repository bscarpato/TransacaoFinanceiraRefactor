# TransacaoFinanceira

Case para refatoração

Passos a implementar:

1. Corrija o que for necessario para resolver os erros de compilação.
2. Execute o programa para avaliar a saida, identifique e corrija o motivo de algumas transacoes estarem sendo canceladas mesmo com saldo positivo e outras sem saldo sendo efetivadas.
3. Aplique o code review e refatore conforme as melhores praticas(SOLID,Patterns,etc).
4. Implemente os testes unitários que julgar efetivo.
5. Crie um git hub e compartilhe o link respondendo o ultimo e-mail.

# TransacaoFinanceiraRefactor

## Refatoração do projeto TransacaoFinanceira

### Arquitetura em Camadas

O projeto foi estruturado seguindo uma Arquitetura em Camadas, promovendo a separação de responsabilidades e a modularidade.
As camadas incluem:

- Camada de Domínio (Domain): Contém as entidades centrais do negócio, como Transacao e ContaSaldo, e encapsula a lógica de negócios fundamental.
- Camada de Acesso a Dados (DataAccess): Responsável pela comunicação com mecanismos de armazenamento, gerenciando a persistência e recuperação de dados de transações e saldos.
- Camada de Serviço (Services): Orquestra o fluxo de operações de aplicação, interagindo com a camada de domínio para processar transações financeiras através da classe ExecutarTransacaoFinanceira.
- Camada de Apresentação (Presentation): Interface de interação com o usuário ou sistemas externos, onde a classe Program atua como o ponto de entrada da aplicação.

### Padrões de Design

Foram aplicados vários padrões de design para resolver problemas comuns:

- Padrão de Repositório (Repository Pattern): Aplicado na camada de acesso a dados para abstrair a complexidade das operações de banco de dados, permitindo que a lógica de negócios interaja com um repositório genérico em vez de detalhes específicos do banco de dados.
- Padrão de Serviço (Service Pattern): Utilizado na camada de serviço para encapsular a lógica de negócios, garantindo que a camada de apresentação não contenha lógica de negócios diretamente, mas delegue responsabilidades para serviços definidos.
- Injeção de Dependência (Dependency Injection): Implementada para desacoplar as classes e suas dependências, permitindo maior flexibilidade e facilitando os testes unitários. A injeção de dependência foi configurada usando o Microsoft.Extensions.DependencyInjection.

### Código e Manutenção

- Legibilidade e Manutenção: Melhoria na legibilidade e na estrutura, aplicando nomes descritivos e mantendo métodos e classes focados em uma única responsabilidade.
- Tratamento de Erros: Melhoria no tratamento de erros para garantir que o sistema se comporte de maneira previsível e informe adequadamente sobre situações excepcionais.
- Concorrência: Implementado o gerenciamento de acesso a recursos compartilhados usando lock, garantindo a execução segura em ambientes de múltiplas threads.

### Testes Unitários

O projeto foi estruturado para permitir testes isolados de componentes, assegurando que cada parte funcione corretamente de forma independente.

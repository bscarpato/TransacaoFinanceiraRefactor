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

Foram aplicados vários padrões de design para resolver problemas comuns, sendo os principais:

- Padrão de Repositório (Repository Pattern): Aplicado na camada de acesso a dados para abstrair a complexidade das operações de banco de dados, permitindo que a lógica de negócios interaja com um repositório genérico em vez de detalhes específicos do banco de dados.
- Padrão de Serviço (Service Pattern): Utilizado na camada de serviço para encapsular a lógica de negócios, garantindo que a camada de apresentação não contenha lógica de negócios diretamente, mas delegue responsabilidades para serviços definidos.
- Injeção de Dependência (Dependency Injection): Implementada para desacoplar as classes e suas dependências, permitindo maior flexibilidade e facilitando os testes unitários. A injeção de dependência foi configurada usando o Microsoft.Extensions.DependencyInjection.

### Princípios SOLID

1. Single Responsibility Principle (SRP) - Princípio da Responsabilidade Única

Classe ExecutarTransacaoFinanceira: Responsável apenas por executar a lógica de negócios das transações financeiras, como verificar a existência de contas, validar o saldo e realizar transferências.
Classe AcessoDados: Encarregada de interagir com o armazenamento de dados, como recuperar e atualizar saldos de contas e verificar transações processadas.
Ao separar a lógica de negócios da lógica de acesso a dados, garantimos que cada classe tenha uma única responsabilidade.

2. Open/Closed Principle (OCP) - Princípio Aberto/Fechado

Interfaces como IAcessoDados e IExecutarTransacaoFinanceira: Essas interfaces permitem que o código seja aberto para extensão, mas fechado para modificação. Facilita a introdução de novas maneiras de acessar dados (talvez mudando de um banco de dados em memória para um banco de dados SQL), podendo criar uma nova implementação de IAcessoDados sem alterar as classes que dependem dessa interface.

3. Liskov Substitution Principle (LSP) - Princípio da Substituição de Liskov

Ao usar interfaces como IAcessoDados, garantimos que qualquer implementação dessa interface possa ser substituída por outra sem afetar o comportamento do sistema. Isso é particularmente útil em testes, onde podemos substituir a implementação real por uma versão mock ou stub para testar o comportamento da classe ExecutarTransacaoFinanceira.

4. Interface Segregation Principle (ISP) - Princípio da Segregação de Interface

Foram criadas interfaces específicas e enxutas para o sistema. Por exemplo, IAcessoDados não deve ter métodos desnecessários que não estão relacionados ao acesso a dados. Se tivermos diferentes tipos de operações de dados, podemos dividir essa interface em interfaces menores e mais específicas.

5. Dependency Inversion Principle (DIP) - Princípio da Inversão de Dependência

Injeção de Dependência: No método Main e na classe ExecutarTransacaoFinanceira, utilizamos a injeção de dependência para fornecer as dependências (IAcessoDados). Isso reduz o acoplamento entre as classes e torna o código mais modular e testável.

### Código e Manutenção

- Legibilidade e Manutenção: Melhoria na legibilidade e na estrutura, aplicando nomes descritivos seguindo as convenções de nomenclatura da linguagem e mantendo métodos e classes focados em uma única responsabilidade.
- Tratamento de Erros: Melhoria no tratamento de erros para garantir que o sistema se comporte de maneira previsível e informe adequadamente sobre situações excepcionais.
- Concorrência: Implementado o gerenciamento de acesso a recursos compartilhados usando lock, garantindo a execução segura em ambientes de múltiplas threads.

### Testes Unitários

O projeto foi estruturado para permitir testes isolados de componentes, assegurando que cada parte funcione corretamente de forma independente.
Foram implementados testes unitários abrangentes, utilizando um framework de mocking para simular dependências externas. Esses testes cobriram casos de uso críticos, como validação diversas, processamento de transações e manutenção do estado das transações processadas, garantindo que a lógica de negócios funcionasse como esperado em diferentes cenários.

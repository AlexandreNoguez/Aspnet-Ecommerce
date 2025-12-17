dotnet ef migrations add Initial
dotnet ef migrations add AddCustomerEmail
dotnet ef database update

# Hierarquia de Dependências (de Fora para Dentro)
A estrutura típica de camadas no DDD segue uma regra de dependência unidirecional, geralmente organizada em:
1. Interface/Apresentação (Camada mais externa)
2. Aplicação
3. Domínio
4. Infraestrutura (Geralmente acoplada externamente ou com padrões específicos para inversão) 
## Regras de Conhecimento e Dependência:
- Camada de Interface/Apresentação: Conhece a Camada de Aplicação (através de Application Services ou DTOs).
- Camada de Aplicação: Conhece a Camada de Domínio e orquestra suas entidades e serviços para realizar casos de uso específicos. Define interfaces para a camada de infraestrutura (como interfaces de repositórios).
- Camada de Domínio: O coração da aplicação, contendo as regras de negócio e a lógica central. Não deve conhecer as camadas de Aplicação, Infraestrutura ou Apresentação. Seus componentes (Entidades, Objetos de Valor, Agregados, Domain Services) só dependem de outros elementos do próprio domínio.
- Camada de Infraestrutura: Implementa as interfaces definidas nas camadas internas (normalmente na camada de Domínio ou Aplicação), como a persistência de dados (bancos de dados) ou comunicação com sistemas externos. Ela depende das abstrações internas. 
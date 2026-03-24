# Prisma
---

## 📖 Sumário
1. [Introdução](#1-introdução)
2. [Descrição Geral do Sistema](#2-descrição-geral-do-sistema)
3. [Desenvolvimento do Projeto](#3-desenvolvimento-do-projeto)
4. [Requisitos do Sistema](#4-requisitos-do-sistema)
5. [Análise do Sistema](#5-análise-do-sistema)
6. [Implementação](#6-implementação)
7. [Considerações Finais](#7-considerações-finais)
8. [Bibliografia](#8-bibliografia)

---

## 1. Introdução

### Contexto
Descrever o cenário atual do negócio a ser impactado pela aplicação. Apresentar o tema do projeto, de forma clara, apresentando ao leitor a área a ser abordada, produtos ou estudos semelhantes. Deixar claro como é a rotina do negócio impactado, como o ambiente de negócio funciona, para visualizar o contexto específico onde a aplicação vai ser inserida.

[]

### Justificativa
Descrever a abordagem do projeto, de modo a comunicar seu propósito e importância a todas as pessoas envolvidas. Deve ficar claro por que os clientes e usuários finais precisam da solução. Deve-se utilizar o tempo presente para falar do problema atual e tempo futuro para falar da situação do negócio quando a nova solução for implantada.
Recomenda-se utilizar as seguintes perguntas para este capítulo:
* **Qual é o problema?** [Ineficiência e o alto custo de tempo na extração, categorização e distribução de questões de bestibulares, somadoo a falta de visibilidade sobre o desempenho real dos alunos]
* **Quem é afetado?** [Professores que são sobrecarregados pela triagem manual do material, e os alunos, que não conseguem direcionar seus estudos com base em dados reais de sua proficiência]
* **Qual o impacto?** [O impacto atual é a falta de otimização nesta parte acadêmica. Com a implantaçõa do projeto, a extração e estruturação do material de estudo será semi-automatizado, e a coordenação terá um painel analitico sobre o desempenho dos alunos no banco de questões e nas listas]

### Proposta
Descrever a solução que será implantada com o desenvolvimento do sistema. Apresentar o impacto do sistema, e como ele soluciona o problema observado.

Apresentar uma descrição em linhas gerais da solução a ser desenvolvida. Independente do que será implementado, este item visa o entendimento global do projeto.

[Uma aplicação web dividida em dois módulos principais. O módulo do professor permitirá a geração de listas de exercícios personalizadas ou para turmas inteiras, extraídas de um banco de questões automatizado, além de fornecer dashboards de análise de desempenho cruzando disciplinas macro e micro. O módulo do aluno oferecerá acesso a essas listas, ao banco geral para estudos independentes e a um painel de autodesempenho. Para o processamento dos dados será utilizado LLMs e OCRs para extrair, converter e pré-categorizar automaticamente as questões]

### Organização do Documento
Descrever como este documento está organizado.

[]
---

## 2. Descrição Geral do Sistema

### Objetivos
Apresentar de forma clara o foco do projeto, com uma descrição em linhas gerais da solução a ser desenvolvida. Deve ser descrita a delimitação da solução, que define o ponto central do projeto. Dentro de uma idéia geral do projeto, ressaltar a idéia específica efetivamente a ser desenvolvida, definindo o objetivo geral.

Para cumprir o objetivo geral é preciso delimitar metas mais específicas dentro do trabalho. São elas que, somadas, conduzirão ao desfecho do objetivo geral. Os objetivos específicos são as ações ou passos que colaboram para alcançar o objetivo geral, e também são delimitadores do escopo do trabalho, ou seja, são ações de interesse que levam ao objetivo geral, restringindo o escopo do trabalho a ser desenvolvido. Enfim, os objetivos específicos devem ser cumpridos para se chegar ao objetivo geral.

* **Geral:** O foco central do projeto e a delimitação da solução. [Desenvolver um sistema web para gestão de desempenho e ?turmas? do Prisma, com um banco de questões dinâmico alimentado por uma cadeia de processamento automatizado de extração e categorização]
* **Específicos:** Metas e ações que, somadas, permitem alcançar o objetivo geral.
  * Desenvolver um script offline para análise de layout de PDFs
  * Implementar um mecanismo de classificação de questões em hierarquias de Disciplinas, macro e micro áreas
  * Criar o ambiente web dos professores (criação de listas e análise de turmas/alunos) e do aluno (resolução de exercícios e progesso pessoal)
  * Painel para validação e correção humana dos dados extraídos pelo pipeline antes da persistência no banco de dados

### Limites e Restrições
Limitar o escopo da solução a ser desenvolvida, descrevendo as necessidades que, a princípio, podem ser consideradas da alçada da aplicação mas não serão implementadas. Apresentar restrições tecnológicas ou de projeto, como por exemplo para qual ambiente será desenvolvida a solução ou um orçamento/prazo máximo previsto. Descreva aqui todas as restrições que o software apresenta com relação a desenvolvimento, implantação, uso, ou qualquer outra situação detectada. As restrições podem ser de compatibilidade, de segurança, de ambiente, de manutenibilidade, de operacionalidade, etc.

* **Limites:** O que a aplicação **não** fará (mesmo que pareça óbvio).
  * Autoria Direta de Questões: Professores não utilizarão o sistema para digitar e criar questões do zero, os professores apenas selecionam as questões que já existem no banco
  * Interação Sincrona: Esta fora do escopo a criação de fóruns, chats em tempo real, ou transmissão de aulas
  * Integração Externa: O sistema não fará integração com sistemas acadêmicos governamentais ou plataformas externas de vestibulares
  
* **Restrições:** Limitações de tecnologia, segurança, orçamento, prazos ou compatibilidade.
  * É preciso de um alto poder computacional para o processamentos dos dados e para o uso dos modelos. Caso o hardware local não suporte, será exigido uma migração para ambientes em nuvem como o Google Colab
  * A acurácia de categorização e da extração de caracteres não será de 100%, sendo necessário a intervenção manual humana antes da persistência no banco de dados


### Descrição dos Usuários
Apresentar os atores que serão envolvidos na solução, bem como o papel de cada ator. Deve ser descrito para qual tipo de empresa se destina o sistema e os tipos de usuários que o utilizarão.

* Aluno: Acessa a plataforma web para resolver as listas designadas pelos professores e acompanha o seu desempenho pessoal nas disciplinas, macro e micro
* Professor: Acessa a plataforma web para analisar o desempenho da(s) turma(s) por meio do dashboard, identifica discrepâncias no desempenho e gera listas de exercícios personalizados e direcionados consultando o banco de questões
* Operador de Dados: Usuário responsável por operar o painel de validação, corrigindo erros na formatação e na categorização antes de aprovar a inserção final no banco de dados 

---

## 3. Desenvolvimento do Projeto

### Tecnologias e Ferramentas
Organize aqui as escolhas técnicas do projeto:

| Categoria | Ferramenta / Tecnologia |
| :--- | :--- |
| **Linguagem de Programação** | [Ex: Java, TypeScript] |
| **Banco de Dados** | [Ex: PostgreSQL] |
| **Modelagem UML** | [Ex: Astah, Lucidchart] |
| **Gerenciamento de Projeto** | [Ex: Trello, Notion] |

### Metodologia
Descrição do modelo de ciclo de vida (ex: Scrum, Kanban, Cascata) e como ele será aplicado (fases, sprints, protótipos).

---

## 4. Requisitos do Sistema

### Requisitos Funcionais (RF)
| ID | Funcionalidade | Prioridade |
| :--- | :--- | :--- |
| RF01 | [Ex: O sistema deve permitir login] | Essencial |
| RF02 | [Ex: Gerar relatório mensal] | Importante |
| RF03 | [Ex: Enviar notificação por SMS] | Desejável |

### Requisitos Não-Funcionais (RNF)
| ID | Requisito | Categoria |
| :--- | :--- | :--- |
| RNF01 | [Ex: O sistema deve responder em < 2s] | Desempenho |
| RNF02 | [Ex: Criptografia de senhas] | Segurança |

### Diagrama de Casos de Uso
![Diagrama de Casos de Uso](./docs/imagens/caso_uso.png)
*Diagrama representando as interações dos atores com o sistema.*

### Protótipos de Telas
![Protótipo de Login](./docs/imagens/prototipo_login.png)
*Descrição: Objetivo da tela e dinâmica de navegação.*

---

## 5. Análise do Sistema

### Modelo do Banco de Dados
* **Conceitual/Lógico:** [Link ou Imagem do MER]
* **Dicionário de Dados:** Detalhamento de tabelas, atributos, tipos e chaves.

### Diagramas Técnicos
* **Diagrama de Classes:** Estrutura das classes e seus relacionamentos.
* **Diagrama de Atividades:** Fluxo de tarefas e processos do sistema.

---

## 6. Implementação

### Estrutura do Código
Explicação da organização de arquivos e pacotes.
src/
 ├── controllers/    # Lógica de controle
 ├── models/         # Entidades e Banco de Dados
 ├── views/          # Interface do usuário
 └── services/       # Regras de negócio

Siga os passos abaixo para executar o projeto localmente:
1. **Requisitos:** Certifique-se de ter instalado o [Ex: Node.js / Docker / Python].
2. **Configuração:** - Clone o repositório: `git clone [URL_DO_REPO]`
   - Configure as variáveis de ambiente no arquivo `.env`.
3. **Comandos de Inicialização:**

   # Instalar dependências
   npm install 
   
   # Rodar migrações do banco
   npm run migrate 
   
   # Iniciar aplicação
   npm start

### Telas Principais
Apresente aqui os prints do sistema finalizado (não mais o protótipo).
* **Tela X:** Descrição do objetivo e como navegar por ela.
* **Tela Y:** Descrição do objetivo e como navegar por ela.

---

## 7. Considerações Finais
Nesta seção, discuta os resultados obtidos:
* **Sucessos:** O que foi atingido conforme o planejado?
* **Limitações:** O que não foi possível implementar e por quê?
* **Futuro:** Possíveis integrações e melhorias para versões posteriores.

---

## 8. Bibliografia
* Autor, Título da Obra, Edição, Local, Editora, Ano.
* Documentação técnica de [Tecnologia X].
* Links e referências externas consultadas.

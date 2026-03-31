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

[O projeto irá ser implementado no ambiente educacional do projeto de extensão do Cursinho Prisma, focado em pré-vestibular. Atualmente, a rotina de preparação de materiais, montagem de listas e o acompanhamento de rendimento dos alunos é feito de forma manual e descentralizada. Paralelamente, os alunos e a coordenação não possuem métricas automatizadas sobre o desempenho em disciplinas e matérias específicas. O sistema será implementado neste cenário para atuar como uma ponte tecnológica, trasformando PDFs de provas em um banco de dados dinâmico e categorizado, que irá alimentar módulos de desempenho e gestão de turmas]

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

[O capítulo 1 apresenta o contexto do Cursinho Prisma, a justificativa do problema a ser resolvido e a proposta de solução beaseada em IA e gestão de dados. O capitulo 2 detalha os objetivos gerais especificos do sistema, expõe as limitações técnicas impostas poelo processamento de modelos de linguagem e visão, e define os papéis dos usuários envolvidos na operação do sistema. O capitulo 3... . O capitulo 4... .]
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
Apresentar as tecnologias, ferramentas e técnicas que serão utilizadas para desenvolvimento e implantação do sistema (linguagem de programação, sistema gerenciador de banco de dados, ferramentas, etc.). Organize em tópicos (Banco de Dados, Modelagem, Gerenciamento de Projeto, etc.) e apresente as ferramentas que serão utilizadas. Não é preciso descrever detalhadamente a tecnologia/ferramenta, mas deve ficar claro o que vai ser usado no desenvolvimento do projeto.

* Motor de extração e Inteligência Artificial:
* * Linguagem: Python
* * Análise de Layout e processamento de PDF: PyMuPDF e OpenCV
  * OCR matemático: Nougat
  * Categorização Semântica: Ollama
* Aplicação web E API:
* * Linguagem backend: python com fastapi
  * Linguagem frontend: typescript com react.js/next.js
* Banco de dados e armazenamento:
* * SGBD relacional: postgreSQL
* Versionamento e gerenciamento do projeto
* * Versionamento: Git com repositório no github
  * Gerenciamento: trello? Jira? Notion?

### Metodologia
Apresentar o modelo de ciclo de vida ou processo a ser utilizado e o motivo da escolha. Descrever como o modelo vai ser aplicado na realização do projeto (quantidade de protótipos, ou fases, definição de módulos e artefatos, etc.) conforme o modelo escolhido.

[O projeto será baseado em Srum adaptado para o contexto acadêmico, com sprints semanais. O motivo dessa escolha é a incerteza técnica do pipeline de extração de PDFs. Outra abordagem poderia prejudicar o projeto inteiro caso aconteça alguma falha no módulo de extração de e inteligência Artificial.

A applicação será dividida em três frases incrementais:
1. Prova de conceito do motor de extração: 
2. Módulo de homologação: 
3. Aplicação final do professor/aluno: 
]
### Cronograma previsto
Definir o cronograma de desenvolvimento do projeto. Elaborar o cronograma por semana, definindo o responsável por cada tarefa. O cronograma deve contemplar todas as tarefas previstas no processo de desenvolvimento de software (descrito no item 3.2 Metodologia de desenvolvimento), conforme definido para o desenvolvimento do sistema.

[

| Semana | Atividade | Responsável |
|--------|-----------|-------------|
| 1      |           |             |
| 2      |           |             |
| 3      |           |             |
| 4      |           |             |
| 5      |           |             |
| 6      |           |             |
| 7      |           |             |
| 8      |           |             |
| 9      |           |             |
| 10     |           |             |
| 11     |           |             |
| 12     |           |             |
| 13     |           |             |
| 14     |           |             |
| 15     |           |             |

]
---

## 4. Requisitos do Sistema

### Requisitos Funcionais (RF)
Apresentar os requisitos funcionais, que especificam ações que o sistema deve ser capaz de executar, ou seja, as funções do sistema. Classifique as funcionalidades quanto a prioridade:
Essencial - deve ser implementado para que o sistema funcione.
Importante - sem este requisito o sistema pode funcionar, mas não da maneira esperada.
Desejável - este tipo de requisito não compromete o funcionamento do sistema.

| ID | Funcionalidade | Prioridade |
| :--- | :--- | :--- |
| RF01 | O sistema de extração deve infentificar bounding boxes e separar texto de imagens e gráficos em provas | Essencial? |
| RF02 | O sistema de extração deve identificar e transcrever equações em texto estruturado | Essencial? |
| RF03 | O sistema de extração deve categorizar as questões extraídas | Essencial? |
| RF04 | O sistema web deve prover uma interface para o operadores de dados corrigirem textos, recortes e categorias gerados pela IA | Essencial? |
| RF05 | O sistema deve permitir a inserção, edição e exclusão de questões no banco de dados | Essencial |
| RF06 | O sistema deve gerenciar autenticação e autorização para três perfis: Operador de Dados, Professor e Aluno | Essencial |
| RF07 | O professor deve ser capaz de criar turmas e vincular alunos a elas | Importante |
| RF08 | O aluno deve acessar suas listas pendentes, registrar respostas e receber gabarito automático | Importante |
| RF09 | O aluno deve acessar o repositório de questões livre, registrar respostas e receber o gabarito automático | Essencial |
| RF10 | O aluno deve visualizar suas métricas de acerto segmentadas por áreas de conhecimento e subáreas  | Essencial |
| RF11 | O professor deve visualziar relatórios de desempenho da turma e individual de alunos | Essencial |
| RF12 | O professor deve poder filtrar o banco de questões (por áreas e subáreas de conhecimento) e gerar listas vinculadas a turmas ou alunos | Importante |
| RF13 | O sistema deve permitir acesso para coordenação do crud de alunos, professores e operadores de alunos  | Essencial |

Criar aqui subitens do capítulo para descrever textualmente, com mais detalhes, as funcionalidades previstas.

[]

### Requisitos Não-Funcionais (RNF)
Descrever os requisitos não-funcionais do sistema, que especificam restrições sobre os serviços ou funções providas pelo sistema, categorizando de acordo com a característica envolvida, como: Usabilidade, Padronização, Ambiente, Compatibilidade, Recursos, etc.

| ID | Requisito | Categoria |
| :--- | :--- | :--- |
| RNF01 | O processamento de PDFs e inferência de IA não deve ocorrer no servidor web, operando em abiente isolado | Ambiente |
| RNF02 | O front-end do módulo do aluno deve ser responsivo e seguir princípios que garantem o uso no mobile, garantindo usabilidade no mobile | Usabilidade |
| RNF03 | O banco de dados deve impor restrições de chave estrangeira rígidas entre as micro e macro disciplinas para impedir a categorização para categorias inexistentes | Confiabilidade |
| RNF04 | A senha dos usuários devem ser salvas utilizando algum algoritmo de hash | Segurança |
| RNF05 | O tempo de resposta das consultas da API para renderização do dashboard não deve ultrapassar 3 segundos | Desempenho |

### Diagrama de Casos de Uso
Inclua aqui os diagramas de Casos de Uso desenvolvidos para o sistema, usando os IDs dos itens anteriores como referência quando necessário.
![Diagrama de Casos de Uso](./docs/imagens/caso_uso.png)
*Diagrama representando as interações dos atores com o sistema.*

### Protótipos de Telas
Apresentar o protótipo do sistema, que consiste na interface preliminar contendo um conjunto de funcionalidades e telas. 
![Protótipo de Login](./docs/imagens/prototipo_login.png)
*Descrição: Objetivo da tela e dinâmica de navegação.*
O protótipo é um recurso que deve ser adotado como estratégia para levantamento, detalhamento, validação de requisitos e modelagem de interface com o usuário (usabilidade).
As telas do sistema podem ser criadas na própria linguagem de desenvolvimento ou em qualquer outra ferramenta de desenho. Cada tela deve possuir uma descrição do seu funcionamento, constando pelo menos o objetivo da tela e dinâmica de navegação (de onde é chamada e que outras telas pode chamar). A descrição das telas deve registrar informações que possam ser consultadas para facilitar a implementação e a execução de testes, assim como a que requisitos funcionais se referem.
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

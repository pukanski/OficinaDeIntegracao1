# Prisma
---

## Sumário
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

O projeto irá ser implementado no ambiente educacional do projeto de extensão do Cursinho Prisma, focado em pré-vestibular. Atualmente, a rotina de preparação de materiais, montagem de listas e o acompanhamento de rendimento dos alunos é feito de forma manual e descentralizada. Paralelamente, os alunos e a coordenação não possuem métricas automatizadas sobre o desempenho em disciplinas e matérias específicas. O sistema será implementado neste cenário para atuar como uma plataforma web que reduz o atrito na criação de materiais e forcene inteligência de dados educacionais

### Justificativa

O problema central é a ineficiência e o alto custo de tempo na extração, categorização e distribução de questões de vestibulares, somado a falta de visibilidade sobre o desempenho real dos alunos. Atualmente, os professores são sobrecarregados pela triagem manual do material, e os alunos, não conseguem direcionar seus estudos com base em dados reais de sua proficiência. Essa falta de otimização dos materiais e a falta de autoanalise acaba afetando o nivelamento acadêmico e pode acabar gerando uma evasão na parte do aluno. Ao simplificar a criação de questões e automatizar a etapa de categorização, o sistema reduz o esforço do professor. Quando o sistema estiver operante, ele irá ter um banco de questões auto-organizado, que permite a geração rápida de listas e dashboards de desempenho

### Proposta

Uma aplicação web com um módulo de cadastro de questões na parte do professor, contendo uma interface otimizada onde o professor ou monitor insere o texto e imagens da questão. O sistema realiza uma chamada a um modelo de IA que analisa o texto e sugere automaticamente a classificação correta (Disciplina Macro e Micro) e o nível de dificuldade. Além deste módulo, teremos os módulos do professor, com gestão de turmas, geração dinâmica de listas de exercícios e painéis analiticos, e também do aluno, com o banco de questões, listas dos professores e a visão de autodesempenho

### Organização do Documento

O capítulo 1 apresenta o contexto do Cursinho Prisma, a justificativa do problema a ser resolvido e a proposta de solução beaseada em IA e gestão de dados. O capitulo 2 detalha os objetivos gerais especificos do sistema, expõe as limitações técnicas impostas pelo processamento da IA, e define os papéis dos usuários envolvidos na operação do sistema. O capitulo 3 detalha as tecnologicas, a metodologia ágil paralela e o cronograma. O capitulo 4 especifica os requisitos funcionais e não-funcionais que guiarão o desenvolvimento.

---

## 2. Descrição Geral do Sistema

### Objetivos

* **Geral:** Desenvolver e implantar um sistema web para gestão educacional focado em avaliação de desempenho e criação de listas de exercícios, sustentado por um banco de questões alimentado colaborativamente através de uma interface assistida por IA para automação da categorização e da dificuldade
* **Específicos:** Metas e ações que, somadas, permitem alcançar o objetivo geral.
  * Desenvolver uma interface web otimizada para inserção rápida de questões (textos e imagens manuais)
  * Integrar um modelo de IA local, como o Ollama) à API para processar textos de questões inseridas e retornar sugestões estruturadas de categorização e dificuldade
  * Desenvolver o módulo do professor, permitindo o agrupamento de questões em listas e a gestão de turmas
  * Desenvolver o módulo do aluno para resolução de exercicios
  * Implementar dashboards de proficiência baseados no histórico de resoluções

### Limites e Restrições

* **Limites:** O que a aplicação **não** fará (mesmo que pareça óbvio).
  * Limitação de extração: O sistema não realiza, atualmente, a extração autônoma, leitura de PDFs e recortes automáticos ou OCR de provas. Toda questão deve ser inserida na interface pelo usuário, seja por digitação ou copiando e colando, com upload manual de imagens pelo usuário
  * Limitação da IA: A IA atuará estritamente como um assistente de preenchimento. O usuário terá a palavra final para aceitar, alterar ou recusar a categoria e a dificuldade proposta pelo modelo antes de salvar no banco
  * Interação Sincrona: Esta fora do escopo a criação de fóruns, chats em tempo real, ou transmissão de aulas
  * Integração Externa: O sistema não fará integração com sistemas acadêmicos governamentais ou plataformas externas de vestibulares
  
* **Restrições:**
  * Pela falta de recursos a extração automática de questões diretamente de PDFs de provas não será feita nesta etapa, podendo ser uma funcionalidade futura
  * A acurácia de categorização não será de 100%, sendo necessário a intervenção manual humana antes da persistência no banco de dados
  * A API que concecta o sistema ao Ollama exige que o servidor de hospedagem tenha capacidade mínima de processamento para inferência do modelo, ou que a API de IA seja isolada em um serviço em nuvem específico


### Descrição dos Usuários
Apresentar os atores que serão envolvidos na solução, bem como o papel de cada ator. Deve ser descrito para qual tipo de empresa se destina o sistema e os tipos de usuários que o utilizarão.

* Aluno: Acessa a plataforma web para resolver as listas designadas pelos professores, treina no banco livre e acompanha o seu desempenho pessoal nas disciplinas, macro e micro
* Professor/Monitor: Acessa a plataforma web para analisar o desempenho das turmas por meio do dashboard, identifica discrepâncias no desempenho, cadastra novas questões no banco, gera listas de exercícios personalizadas e direcionadas consultando o banco de questões
* Administrador (Coordenação): Gerencia os cadastros de professores e alunos, define a árvore de macro e micro disciplinas no banco e administra o sistema

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
* * Linguagem backend: Java?.net?
  * Linguagem frontend: typescript com react.js/next.js
* Banco de dados e armazenamento:
* * SGBD relacional: postgreSQL
* Versionamento e gerenciamento do projeto
* * Versionamento: Git com repositório no github
  * Gerenciamento: trello? Jira? Notion?

### Metodologia
Apresentar o modelo de ciclo de vida ou processo a ser utilizado e o motivo da escolha. Descrever como o modelo vai ser aplicado na realização do projeto (quantidade de protótipos, ou fases, definição de módulos e artefatos, etc.) conforme o modelo escolhido.

[O projeto será baseado em Srum adaptado para o contexto acadêmico, com sprints semanais. O motivo dessa escolha é a incerteza técnica do pipeline de extração de PDFs. Outra abordagem poderia prejudicar o projeto inteiro caso aconteça alguma falha no módulo de extração de e inteligência Artificial.
Para otimizar o trabalaho, o desenvolvimento não será estritamente sequencial, sendo dividido em três frentes de trabalho paralelas que se integram ao longo do tempo:
1. Frente de Dados e IA: Focada em validar o script python, integração com nougat e o ollama (Responsáveis: Igor e Rafael)
2. Frente de Back-End e Banco de Dados: Responsável por modelar o postgre, criar as rotas da api e gerenciar a autenticação e as regras do negócio (Responsáveis: Gustavo e Luiz)
3. Frente de Front-End: Focada na construção das interfacesm, como o painel de homologação e nos módulos do professor e aluno (Responsável: Orlando)
]

### Cronograma previsto
Definir o cronograma de desenvolvimento do projeto. Elaborar o cronograma por semana, definindo o responsável por cada tarefa. O cronograma deve contemplar todas as tarefas previstas no processo de desenvolvimento de software (descrito no item 3.2 Metodologia de desenvolvimento), conforme definido para o desenvolvimento do sistema.

[

| Semana | Frente Ia e Dados| Frente Back-End | Frente Front-End |
|--------|-----------|-------------|-------------| 
| 1      |           |             |             |
| 2      |           |             |             |
| 3 (Entrega 1)|           |             |             |
| 4      |           |             |             |
| 5      |           |             |             |
| 6      |           |             |             |
| 7      |           |             |             |
| 8 (Entrega 2)|           |             |             |
| 9      |           |             |             |
| 10     |           |             |             |
| 11     |           |             |             |
| 12     |           |             |             |
| 13     |           |             |             |
| 14 (Entrega 3)|           |             |             |

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

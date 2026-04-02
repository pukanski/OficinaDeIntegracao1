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
  * Pela falta de recursos a extração automática de questões diretamente de PDFs de provas não será feita atualmente, podendo ser uma funcionalidade futura
  * A acurácia de categorização não será de 100%, sendo necessário a intervenção manual humana antes da persistência no banco de dados
  * A API que concecta o sistema ao Ollama exige que o servidor de hospedagem tenha capacidade mínima de processamento para inferência do modelo, ou que a API de IA seja isolada em um serviço em nuvem específico
  * Pela falta de recursos, o sistema irá se restringir a usar ferramentas gratuitas ou com tiers gratuitos


### Descrição dos Usuários

* Aluno: Acessa a plataforma web para resolver as listas designadas pelos professores, treina no banco livre e acompanha o seu desempenho pessoal nas disciplinas, macro e micro
* Professor/Monitor: Acessa a plataforma web para analisar o desempenho das turmas por meio do dashboard, identifica discrepâncias no desempenho, cadastra novas questões no banco, gera listas de exercícios personalizadas e direcionadas consultando o banco de questões
* Administrador (Coordenação): Gerencia os cadastros de professores e alunos, define a árvore de macro e micro disciplinas no banco e administra o sistema

---

## 3. Desenvolvimento do Projeto

### Tecnologias e Ferramentas

* Serviço de Inteligência Artificial:
  * Linguagem: Python com FastAPI, atuando como uma API interna
  * Categorização Semântica e de dificuldade: Ollama
  * Hospedagem: Ngrok
* Aplicação web E API:
  * Linguagem backend: ASP .NET
  * Linguagem frontend: Angular
  * Hospedagem front: Vercel
  * Hospedagem back-end: Render
  * Conteinerização: Docker 
* Banco de dados e armazenamento:
  * SGBD relacional: PostgreSQL
  * Armazenamento de arquivos: Supabase
  * Hospedagem: Supabase
* Gerenciamneto, qualidade e documentação:
  * Versionamento: Git com repositório no GitHub
  * Gerenciamento: Jira
  * Testes de API: Insomnia
  * Documentação de API: Swagger

### Metodologia

O projeto usará um Scrum adaptado para o contexto acadêmico com sprints semanais. O motivo dessa escolha é a incerteza técnica entre as diferentes tecnologias. Outra abordagem poderia prejudicar o projeto inteiro caso aconteça alguma falha em algum módulo específico.
Para otimizar o trabalho, o desenvolvimento não será estritamente sequencial, sendo dividido em quatro frentes de trabalho paralelas que se integram ao longo do tempo:
1. Frente Front-End (2 pessoas): Responsáveis pelos protótipos de telas, gerenciar o estado da plicação e consumir os endpoints da API ASP.NET
2. Frente Back-ENd: Responsável por modelar obanco, criar os CRUDs, implementar a segurança e pelo Swagger
3. Frente Serviço de IA: Responsável por criar a API interna que se comunica com o Ollama, afinar o prompt para garantir que a IA responda sempre no formato correto e garantindo o tempo de resposta da inferência
4. Frente de Integração e Infraestrutura: Responsável pela configuração das ferramentas, dos testes e da integração entre os diferentes frontes


### Cronograma previsto

| Semana | Frente Front-End | Frente Back-End | Frente IA | Frente Integracao e Infra |
| :--- | :--- | :--- | :--- | :--- |
| 1 | Entender o escopo e o sistema | Entender o escopo e o sistema | Entender o escopo e o sistema | Entender o escopo e o sistema |
| 2 | Criar prototipos das telas | Modelar banco de dados | Definir requisitos da IA | Configurar repositorios |
| 3 (Entrega 1) | Telas base e login | CRUD base e banco rodando | Ajustar retorno da IA em JSON | Conectar Front e Back |
| 4 | Tela de cadastro de questoes | Endpoint para receber questoes | Tratar erros de texto na IA | Criar Dockerfile e upload de imagens |
| 5 | Tela de revisao da IA | Conectar C# com Python | Testar IA com questoes reais | Conectar Back com IA |
| 6 | Telas de filtro e listas | Logica de criacao de listas | Ajustar erros e alucinacoes da IA | Validar fluxo do Front ate a IA |
| 7 | Tela de visualizacao de turmas | Consultas no banco PostgreSQL | Otimizar tempo de resposta da IA | Resolver problemas de rede e CORS |
| 8 (Entrega 2) | Corrigir erros visuais | Estabilizar a API e os dados | Congelar a versao do prompt | Testar cadastro completo com IA |
| 9 | Telas iniciais do aluno | Rotas de listas pendentes | Avaliar erros do cadastro | Preparar contas para o deploy |
| 10 | Tela de resolucao de exercicios | Endpoint para salvar respostas | Ajustes finais no texto da IA | Subir o banco PostgreSQL na nuvem |
| 11 | Tela do banco livre de questoes | Filtros de busca e rotas de chamada | Documentar como rodar a IA | Subir a API ASP.NET no Render |
| 12 | Graficos e percentual de presenca | Consultas de acertos e frequencia | Ajustar parametros do Ollama | Testar sistema na nuvem com IA local |
| 13 | Dashboard e tela de chamada | Criar indices de banco de dados | Corrigir bugs residuais da IA | Fazer testes de carga |
| 14 (Entrega 3) | Polimento final para celular | Limpeza de codigo e seguranca | Fechar escopo da IA | Entregar sistema em producao |

---

## 4. Requisitos do Sistema

### Requisitos Funcionais (RF)

| ID | Funcionalidade | Prioridade |
| :--- | :--- | :--- |
| RF01 | O sistema deve prover uma interface para inserção manual do enunciado, alternativas e gabarito, além de upload de imagem de apoio | Essencial |
| RF02 | Ao preencher o enunciado, o sistema deve acionar a IA para sugerir a disciplina, matéria e dificuldade (Fácil/Médio/Difícil) | Essencial |
| RF03 | O usuário criador deve poder aceitar, editar ou recusar as sugestões da IA antes de salvar a questão | Essencial |
| RF04 | Professores e administradores devem poder listar, editar e desativar questões já salvas no banco | Essencial |
| RF05 | O sistema deve gerenciar autenticação e autorização para três perfis: Administrador, Professor e Aluno | Essencial |
| RF06 | Professores devem poder criar turmar e adicionar alunos a elas | Importante |
| RF07 | Professores devem poder buscar questões no banco através de filtros e agrupa-lás em listas atribuídas a turmas específicas | Importante |
| RF08 | Alunos devem poder abrir listas ou o banco de livre, selecionar a alternativa e receber a correção automática no sistema | Essencial |
| RF09 | O sistema deve exibir gráficos para o aluno detalhando sua porcentagem de acertos/erros agrupados por macro e micro disciplinas | Essencial |
| RF10 | O sistema deve exibir relatórios de proficiência mostrando as maiores lacunas de aprendizado da turma e alunos individuais  | Essencial |
| RF11 | A coordenação deve poder gerenciar contas de usuários e a estrutura de disciplinas do banco | Essencial |
| RF12 | O sistema deve fornecer uma interface rápida para o professor registrar a presença ou falta dos alunos | Desejável |
| RF13 | No dashboard do aluno deve haver um indicador exibindo seu percentual de presença  | Desejável |

Criar aqui subitens do capítulo para descrever textualmente, com mais detalhes, as funcionalidades previstas.

[]

### Requisitos Não-Funcionais (RNF)
Descrever os requisitos não-funcionais do sistema, que especificam restrições sobre os serviços ou funções providas pelo sistema, categorizando de acordo com a característica envolvida, como: Usabilidade, Padronização, Ambiente, Compatibilidade, Recursos, etc.

| ID | Requisito | Categoria |
| :--- | :--- | :--- |
| RNF01 | O tempo de espera pela sugestão da IA não deve travar a tela, exibindo indicação visual de carregamento | Usabilidade |
| RNF02 | O front-end deve ser responsivo e seguir princípios que garantem o uso no mobile, sem perda de funcionalidades para telas de smartphones | Usabilidade |
| RNF03 | O banco de dados deve impor restrições de chave estrangeira rígidas entre as micro e macro disciplinas para impedir a categorização para categorias inexistentes | Confiabilidade |
| RNF04 | O sistema não deve expor a comunicação direta com a IA, o front-end deve apenas fazer requisições à API, que atuará como intermediária segura para o Ollama | Segurança |
| RNF05 | Senhas devem ser protegidas no banco utilizando algum algoritmo de hashing | Segurança |
| RNF06 | O tempo de resposta das consultas da API para a renderização do dashboard não deve ultrapassar 3 segundos | Desempenho |

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

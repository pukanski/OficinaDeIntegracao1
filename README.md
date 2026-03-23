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
Descrever o cenário atual do negócio a ser impactado pela aplicação. Apresentar o tema do projeto, a área abordada e estudos semelhantes. Como funciona a rotina atual do negócio?

### Justificativa
Propósito e importância da solução. Por que os clientes precisam disso?
* **Qual é o problema?** [Descreva aqui]
* **Quem é afetado?** [Descreva aqui]
* **Qual o impacto?** [Descreva aqui]

### Proposta
Descrição global da solução. Como o sistema resolve o problema observado e qual o seu impacto esperado?

---

## 2. Descrição Geral do Sistema

### Objetivos
* **Geral:** O foco central do projeto e a delimitação da solução.
* **Específicos:** Metas e ações que, somadas, permitem alcançar o objetivo geral.

### Limites e Restrições
* **Limites:** O que a aplicação **não** fará (mesmo que pareça óbvio).
* **Restrições:** Limitações de tecnologia, segurança, orçamento, prazos ou compatibilidade.

### Descrição dos Usuários
Atores envolvidos, seus papéis no sistema e o perfil das empresas que utilizarão a solução.

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

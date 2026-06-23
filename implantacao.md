# Guia de Implantação — Cursinho Prisma

> Este documento descreve passo a passo como configurar e executar o sistema Prisma em ambiente local.

---

## 1. Ferramentas Necessárias

Instale as ferramentas abaixo antes de prosseguir. Todas são gratuitas.

| Ferramenta | Versão mínima | Link |
|------------|--------------|------|
| **Git** | qualquer | https://git-scm.com/downloads |
| **Docker Desktop** | 4.x | https://www.docker.com/products/docker-desktop |
| **Node.js** | 20 LTS | https://nodejs.org |
| **Angular CLI** | 21.x | `npm install -g @angular/cli` |
| **Ollama** | qualquer | https://ollama.com/download |

> **Nota:** O .NET SDK e o Python **não precisam ser instalados** — eles rodam dentro dos containers Docker.

---

## 2. Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/OficinaDeIntegracao1.git
cd OficinaDeIntegracao1
```

---

## 3. Configurar o Ollama (IA Local)

O Ollama precisa rodar **antes** de subir o Docker, diretamente na máquina host. O `docker-compose.yml` já está configurado para se comunicar com ele via `host.docker.internal:11434` — não é necessário alterar nenhuma variável.

**3.1 Instalar o Ollama** pelo link da tabela acima.

**3.2 Baixar o modelo:**
```bash
ollama pull llama3.2:3b
```

**3.3 Verificar se está rodando:**
```bash
ollama list
# deve aparecer: llama3.2:3b
```

> Se o Ollama não estiver rodando quando o Docker subir, o botão "Sugerir IA" retornará erro — o restante do sistema funcionará normalmente.

---

## 4. Variáveis de Ambiente

O projeto já vem com todas as variáveis configuradas apontando para o banco e autenticação do projeto. **Não é necessário criar conta no Supabase nem configurar nada.**

### 4.1 Arquivo `.env` (Backend)

**Localização:** `OficinaDeIntegracao1/.env`

Já preenchido com as credenciais do projeto. O `docker-compose.yml` lê este arquivo automaticamente.

### 4.2 Arquivo `environment.ts` (Frontend)

**Localização:** `frontend/prisma-web/src/environments/environment.ts`

Já configurado para apontar para o gateway local na porta 5050.

> Não altere nenhum destes arquivos — os valores já estão corretos para execução local.

---

## 5. Subir o Backend (Docker)

Na raiz do projeto, execute:

```bash
docker-compose up --build
```

Aguarde até todos os containers estarem rodando. Na primeira vez o build pode levar 5-10 minutos.

**Containers que serão iniciados:**

| Container | Porta exposta | Descrição |
|-----------|--------------|-----------|
| `api-gateway` | 5050 | Ponto de entrada único do backend |
| `aluno-api` | interno | Microserviço de alunos e frequência |
| `professor-api` | interno | Microserviço de professores e admin |
| `questao-api` | interno | Microserviço de questões e provas |
| `lista-api` | interno | Microserviço de listas |
| `turma-api` | interno | Microserviço de turmas |
| `dados-api` | interno | Microserviço de desempenho |
| `ia-service` | interno | API Python de classificação (usa Ollama do host) |

**Verificar se está tudo rodando:**
```bash
docker ps
# Todos os containers devem aparecer com status "Up"
```

---

## 6. Subir o Frontend (Angular)

Em um terminal separado:

```bash
cd frontend/prisma-web
npm install
ng serve
```

Aguarde a mensagem:
```
✔ Compiled successfully.
```

O frontend estará disponível em: **http://localhost:4200**

---

## 7. Acessar o Sistema

Abra o navegador em `http://localhost:4200`.

### Credenciais de Acesso

| Perfil | E-mail | Senha |
|--------|--------|-------|
| **Administrador** | admin@oficina.com | 123456 |
| **Professor** | bruna.campos@oficina.com | 123456 |
| **Aluno** | gabriel.melo@oficina.com | 123456 |

> Selecione a aba correta (Professor / Aluno / Admin) antes de fazer login.

---

## 8. Encerrar o Sistema

```bash
# Parar os containers (mantém os dados)
docker-compose down

# Parar e remover volumes (apaga dados locais)
docker-compose down -v
```

O frontend é encerrado com `Ctrl+C` no terminal onde o `ng serve` está rodando.

---

## 9. Solução de Problemas Comuns

| Problema | Solução |
|----------|---------|
| Container não sobe / erro de build | `docker-compose up --build` para recompilar |
| IA não responde | Verificar se Ollama está rodando: `ollama list` |
| IA demora muito na primeira vez | Normal — o modelo carrega na memória |
| Erro de conexão com banco | Verificar se o `.env` está na raiz do projeto |
| `ng serve` falha | Rodar `npm install` dentro de `frontend/prisma-web` |
| Login não redireciona | Verificar se o gateway está rodando na porta 5050 |

---

## Resumo dos Comandos

```bash
# 1. Certifique-se que o Ollama está rodando
ollama list

# 2. Terminal 1 — Backend
docker-compose up --build

# 3. Terminal 2 — Frontend
cd frontend/prisma-web && npm install && ng serve

# 4. Acesse
# http://localhost:4200
```

import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { environment } from '../../../../../environments/environment';

interface UsuarioDTO { id: string | number; nome: string; email: string; tipo: 'Professor' | 'Aluno'; status: 'Ativo' | 'Inativo'; }
interface DisciplinaDTO { id: string; nome: string; macroArea: string; }

@Component({
  selector: 'app-painel-admin',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './painel-admin.html'
})
export class PainelAdminComponent implements OnInit {
  abaAtual: 'usuarios' | 'disciplinas' = 'usuarios';
  usuarioForm!: FormGroup;
  disciplinaForm!: FormGroup;

  usuarios: UsuarioDTO[] = [];
  disciplinas: DisciplinaDTO[] = [];

  carregando = false;
  mensagem = '';
  erro = '';

  private apiUrl = `${environment.gatewayUrl}/api/Admin`;

  constructor(private fb: FormBuilder, private http: HttpClient, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.usuarioForm = this.fb.group({
      nome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(6)]],
      tipo: ['Aluno', Validators.required],
      ra: [''],
      siape: [''],
      disciplina: ['']
    });

    this.disciplinaForm = this.fb.group({
      nome: ['', Validators.required],
      macroArea: ['', Validators.required]
    });

    this.carregarUsuarios();
  }

  carregarUsuarios(): void {
    this.http.get<any>(`${environment.gatewayUrl}/api/Admin/usuarios`).subscribe({
      next: (data) => {
        const profs: UsuarioDTO[] = (data.professores || []).map((p: any) => ({
          id: p.authId || p.id,
          nome: p.primeiroNome ? `${p.primeiroNome} ${p.ultimoNome}` : p.nome,
          email: p.email,
          tipo: 'Professor',
          status: 'Ativo'
        }));

        const alunos: UsuarioDTO[] = (data.alunos || []).map((a: any) => ({
          id: a.authId || a.id,
          nome: a.primeiroNome ? `${a.primeiroNome} ${a.ultimoNome}` : a.nome,
          email: a.email,
          tipo: 'Aluno',
          status: 'Ativo'
        }));

        this.usuarios = [...profs, ...alunos];

        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Erro estrutural ao carregar usuários:', err);
        this.usuarios = [];
        this.cdr.detectChanges();
      }
    });
  }

  adicionarUsuario(): void {
    if (this.usuarioForm.invalid) {
      this.usuarioForm.markAllAsTouched();
      return;
    }

    const rawValue = this.usuarioForm.value;
    const nomes = rawValue.nome.split(' ');

    const payload = {
      email: rawValue.email,
      senha: rawValue.senha,
      primeiroNome: nomes[0],
      ultimoNome: nomes.slice(1).join(' ') || ' ',
      tipo: rawValue.tipo,
      ra: rawValue.tipo === 'Aluno' ? rawValue.ra : null,
      siape: rawValue.tipo === 'Professor' ? rawValue.siape : null,
      disciplina: rawValue.tipo === 'Professor' ? rawValue.disciplina : null
    };

    this.http.post(`${environment.gatewayUrl}/api/Admin/criar-usuario`, payload).subscribe({
      next: () => {
        alert('Usuário criado com sucesso no Supabase e no PostgreSQL!');
        this.usuarioForm.reset({ tipo: 'Aluno' });
      },
      error: (err) => {
        console.error('Erro ao criar usuário', err);
        alert('Falha ao criar usuário. Verifique o console.');
      }
    });
  }

  prepararEdicao(user: any): void {
    // Esse método vai pegar os dados da linha clicada e jogar para cima no formulário
    alert(`A lógica de edição para ${user.nome} será montada aqui. Precisaremos transformar o formulário de "Adicionar" para "Atualizar".`);
    console.log('Dados para edição:', user);
  }

  excluirUsuario(user: any): void {
    if (!confirm(`Alerta Crítico: Tem certeza que deseja excluir permanentemente o ${user.tipo} ${user.nome}? Isso pode corromper históricos vinculados.`)) {
      return;
    }

    // O C# espera o ID numérico (long), garanta que está passando a propriedade correta
    const endpoint = user.tipo === 'Aluno' ? `/api/Aluno/${user.id}` : `/api/Professor/${user.id}`;

    this.http.delete(`${environment.gatewayUrl}${endpoint}`).subscribe({
      next: () => {
        alert('Usuário aniquilado do banco de dados local com sucesso.');
        this.carregarUsuarios(); // Atualiza a tabela imediatamente
      },
      error: (err) => {
        console.error('Falha de exclusão estrutural:', err);
        alert('Falha ao excluir. O usuário possui dependências (listas, respostas) amarradas a ele no banco de dados.');
      }
    });
  }

  adicionarDisciplina(): void {
    if (this.disciplinaForm.invalid) {
      this.disciplinaForm.markAllAsTouched();
      return;
    }
    const val = this.disciplinaForm.value;
    this.disciplinas.push({ id: Date.now().toString(), nome: val.nome, macroArea: val.macroArea });
    this.disciplinaForm.reset();
  }

  excluirDisciplina(id: string): void {
    if (confirm('Deseja excluir esta disciplina?')) {
      this.disciplinas = this.disciplinas.filter(d => d.id !== id);
    }
  }
}

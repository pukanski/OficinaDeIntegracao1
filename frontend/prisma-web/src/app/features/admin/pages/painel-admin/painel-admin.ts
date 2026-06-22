import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
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

  constructor(private fb: FormBuilder, private http: HttpClient) { }

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
    this.http.get<any>(`${this.apiUrl}/usuarios`).subscribe({
      next: (data) => {
        const profs: UsuarioDTO[] = (data.professores || []).map((p: any) => ({
          id: p.id, nome: p.nome, email: p.email, tipo: 'Professor', status: 'Ativo'
        }));
        const alunos: UsuarioDTO[] = (data.alunos || []).map((a: any) => ({
          id: a.id, nome: a.nome, email: a.email, tipo: 'Aluno', status: 'Ativo'
        }));
        this.usuarios = [...profs, ...alunos];
      },
      error: () => { this.usuarios = []; }
    });
  }

  adicionarUsuario(): void {
    if (this.usuarioForm.invalid) {
      this.usuarioForm.markAllAsTouched();
      return;
    }

    this.carregando = true;
    this.erro = '';
    this.mensagem = '';

    const raw = this.usuarioForm.value;
    const nomes = (raw.nome as string).trim().split(' ');

    const payload = {
      email: raw.email,
      senha: raw.senha,
      primeiroNome: nomes[0],
      ultimoNome: nomes.slice(1).join(' ') || nomes[0],
      tipo: raw.tipo,
      ra: raw.tipo === 'Aluno' ? raw.ra : null,
      siape: raw.tipo === 'Professor' ? raw.siape : null,
      disciplina: raw.tipo === 'Professor' ? raw.disciplina : null
    };

    this.http.post(`${this.apiUrl}/criar-usuario`, payload).subscribe({
      next: () => {
        this.mensagem = `${raw.tipo} criado com sucesso!`;
        this.usuarioForm.reset({ tipo: 'Aluno' });
        this.carregarUsuarios();
        this.carregando = false;
      },
      error: (err) => {
        this.erro = err.error?.error || 'Erro ao criar usuário.';
        this.carregando = false;
      }
    });
  }

  alternarStatusUsuario(id: string | number): void {
    const user = this.usuarios.find(u => u.id === id);
    if (user) {
      user.status = user.status === 'Ativo' ? 'Inativo' : 'Ativo';
    }
  }

  excluirUsuario(id: string | number): void {
    if (confirm('Tem certeza que deseja excluir este usuário?')) {
      this.usuarios = this.usuarios.filter(u => u.id !== id);
    }
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

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
  usuarioForm!: FormGroup;
  disciplinaForm!: FormGroup;

  modoEdicao: boolean = false;
  usuarioEmEdicao: any = null;

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
        const profs: any[] = (data.professores || []).map((p: any) => ({
          idNumerico: p.id, authId: p.authId, // Guardamos os IDs cruciais
          nome: p.primeiroNome ? `${p.primeiroNome} ${p.ultimoNome}` : p.nome,
          email: p.email, tipo: 'Professor', status: 'Ativo',
          siape: p.siape, disciplina: p.disciplina
        }));

        const alunos: any[] = (data.alunos || []).map((a: any) => ({
          idNumerico: a.id, authId: a.authId,
          nome: a.primeiroNome ? `${a.primeiroNome} ${a.ultimoNome}` : a.nome,
          email: a.email, tipo: 'Aluno', status: 'Ativo',
          ra: a.ra
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

  salvarUsuario(): void {
    if (this.usuarioForm.invalid) {
      this.usuarioForm.markAllAsTouched();
      return;
    }

    const rawValue = this.usuarioForm.getRawValue();
    const nomes = rawValue.nome.split(' ');
    const primeiroNome = nomes[0];
    const ultimoNome = nomes.slice(1).join(' ') || ' ';

    if (this.modoEdicao) {
      const endpoint = rawValue.tipo === 'Aluno'
        ? `/api/Aluno/${this.usuarioEmEdicao.idNumerico}`
        : `/api/Professor/atualizar_professor/${this.usuarioEmEdicao.idNumerico}`;

      const payloadEdit = {
        authId: this.usuarioEmEdicao.authId,
        email: rawValue.email,
        primeiroNome: primeiroNome,
        ultimoNome: ultimoNome,
        ra: rawValue.tipo === 'Aluno' ? rawValue.ra : null,
        siape: rawValue.tipo === 'Professor' ? rawValue.siape : null,
        disciplina: rawValue.tipo === 'Professor' ? rawValue.disciplina : null
      };

      this.http.put(`${environment.gatewayUrl}${endpoint}`, payloadEdit).subscribe({
        next: () => {
          alert('Dados do usuário atualizados no banco local com sucesso.');
          this.cancelarEdicao();
          this.carregarUsuarios();
        },
        error: (err) => console.error('Falha ao atualizar usuário:', err)
      });

    } else {
      const payloadCreate = {
        email: rawValue.email,
        senha: rawValue.senha,
        primeiroNome: primeiroNome,
        ultimoNome: ultimoNome,
        tipo: rawValue.tipo,
        ra: rawValue.tipo === 'Aluno' ? rawValue.ra : null,
        siape: rawValue.tipo === 'Professor' ? rawValue.siape : null,
        disciplina: rawValue.tipo === 'Professor' ? rawValue.disciplina : null
      };

      this.http.post(`${environment.gatewayUrl}/api/Admin/criar-usuario`, payloadCreate).subscribe({
        next: () => {
          alert('Usuário criado com sucesso no Supabase e no PostgreSQL!');
          this.usuarioForm.reset({ tipo: 'Aluno' });
          this.carregarUsuarios();
        },
        error: (err) => console.error('Erro ao criar usuário', err)
      });
    }
  }

  prepararEdicao(user: any): void {
    this.modoEdicao = true;
    this.usuarioEmEdicao = user;

    this.usuarioForm.patchValue({
      nome: user.nome,
      email: user.email,
      senha: 'Nao_editavel_aqui',
      tipo: user.tipo,
      ra: user.ra || '',
      siape: user.siape || '',
      disciplina: user.disciplina || ''
    });


    this.usuarioForm.get('email')?.disable();
    this.usuarioForm.get('senha')?.disable();
    this.usuarioForm.get('tipo')?.disable();

    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  cancelarEdicao(): void {
    this.modoEdicao = false;
    this.usuarioEmEdicao = null;
    this.usuarioForm.reset({ tipo: 'Aluno' });
    this.usuarioForm.get('email')?.enable();
    this.usuarioForm.get('senha')?.enable();
    this.usuarioForm.get('tipo')?.enable();
  }

  excluirUsuario(user: any): void {
    if (!confirm(`Alerta Crítico: Tem certeza que deseja excluir permanentemente o ${user.tipo} ${user.nome}? Isso pode corromper históricos vinculados.`)) {
      return;
    }

    const endpoint = user.tipo === 'Aluno' ? `/api/Aluno/${user.id}` : `/api/Professor/${user.id}`;

    this.http.delete(`${environment.gatewayUrl}${endpoint}`).subscribe({
      next: () => {
        alert('Usuário aniquilado do banco de dados local com sucesso.');
        this.carregarUsuarios();
      },
      error: (err) => {
        console.error('Falha de exclusão estrutural:', err);
        alert('Falha ao excluir. O usuário possui dependências (listas, respostas) amarradas a ele no banco de dados.');
      }
    });
  }
}

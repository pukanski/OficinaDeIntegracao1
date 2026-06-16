import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

interface UsuarioDTO { id: string; nome: string; email: string; tipo: 'Professor' | 'Aluno'; status: 'Ativo' | 'Inativo'; }
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

  // Mocks espelhando o payload da API
  usuarios: UsuarioDTO[] = [
    { id: 'u1', nome: 'João Silva', email: 'joao@example.com', tipo: 'Professor', status: 'Ativo' },
    { id: 'u2', nome: 'Maria Santos', email: 'maria@example.com', tipo: 'Aluno', status: 'Ativo' },
    { id: 'u3', nome: 'Carlos Oliveira', email: 'carlos@example.com', tipo: 'Professor', status: 'Inativo' }
  ];

  disciplinas: DisciplinaDTO[] = [
    { id: 'd1', nome: 'Matemática', macroArea: 'Ciências da Natureza' },
    { id: 'd2', nome: 'Física', macroArea: 'Ciências da Natureza' }
  ];

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.usuarioForm = this.fb.group({
      nome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(6)]], // <-- Campo de senha adicionado
      tipo: ['Aluno', Validators.required]
    });

    this.disciplinaForm = this.fb.group({
      nome: ['', Validators.required],
      macroArea: ['', Validators.required]
    });
  }

  // --- Ações de Usuários ---
  adicionarUsuario(): void {
    if (this.usuarioForm.invalid) {
      this.usuarioForm.markAllAsTouched();
      return;
    }
    const payload = this.usuarioForm.value;
    console.log('POST /api/admin/usuarios ->', payload);
    alert('Usuário criado. Verifique o payload no console.');
    this.usuarioForm.reset({ tipo: 'Aluno' });
  }

  alternarStatusUsuario(id: string): void {
    const user = this.usuarios.find(u => u.id === id);
    if (user) {
      user.status = user.status === 'Ativo' ? 'Inativo' : 'Ativo';
      console.log(`PUT /api/admin/usuarios/${id}/status ->`, { status: user.status });
    }
  }

  excluirUsuario(id: string): void {
    if (confirm('Tem certeza que deseja excluir este usuário?')) {
      this.usuarios = this.usuarios.filter(u => u.id !== id);
      console.log(`DELETE /api/admin/usuarios/${id}`);
    }
  }

  // --- Ações de Disciplinas ---
  adicionarDisciplina(): void {
    if (this.disciplinaForm.invalid) {
      this.disciplinaForm.markAllAsTouched();
      return;
    }
    const payload = this.disciplinaForm.value;
    console.log('POST /api/admin/disciplinas ->', payload);
    alert('Disciplina adicionada à árvore. Verifique o console.');
    this.disciplinaForm.reset();
  }

  excluirDisciplina(id: string): void {
    if (confirm('Aviso: Excluir uma disciplina pode impactar questões existentes. Deseja continuar?')) {
      this.disciplinas = this.disciplinas.filter(d => d.id !== id);
      console.log(`DELETE /api/admin/disciplinas/${id}`);
    }
  }
}
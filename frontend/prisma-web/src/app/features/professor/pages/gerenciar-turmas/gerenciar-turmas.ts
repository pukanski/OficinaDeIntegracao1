import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

interface Turma { id: number; nome: string; ano: string; turno: string; qtdAlunos: number; alunosIds: number[]; }
interface Aluno { id: number; primeiroNome: string; ultimoNome: string; email: string; ra: string; }

@Component({
  selector: 'app-gerenciar-turmas',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './gerenciar-turmas.html'
})
export class GerenciarTurmasComponent implements OnInit {
  turmaForm!: FormGroup;
  turmas: Turma[] = [];
  turmaSelecionada: Turma | null = null;
  alunosDaTurma: Aluno[] = [];
  todosAlunos: Aluno[] = [];

  criandoTurma = false;
  salvando = false;
  mensagem = '';
  erro = '';

  private api = environment.gatewayUrl;

  constructor(private fb: FormBuilder, private http: HttpClient, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.turmaForm = this.fb.group({
      nome: ['', Validators.required],
      ano: [new Date().getFullYear().toString(), Validators.required],
      turno: ['Noturno', Validators.required]
    });
    this.carregarTurmas();
    this.carregarAlunos();
  }

  carregarTurmas(): void {
    this.http.get<Turma[]>(`${this.api}/api/Turma`).subscribe({
      next: (data) => { this.turmas = data; this.cdr.detectChanges(); },
      error: () => { this.turmas = []; this.cdr.detectChanges(); }
    });
  }

  carregarAlunos(): void {
    this.http.get<Aluno[]>(`${this.api}/api/Aluno/alunos`).subscribe({
      next: (data) => { this.todosAlunos = data; this.cdr.detectChanges(); },
      error: () => { this.todosAlunos = []; this.cdr.detectChanges(); }
    });
  }

  salvarTurma(): void {
    if (this.turmaForm.invalid) { this.turmaForm.markAllAsTouched(); return; }
    this.salvando = true;
    this.erro = '';
    this.http.post<Turma>(`${this.api}/api/Turma`, this.turmaForm.value).subscribe({
      next: (t) => {
        this.turmas = [...this.turmas, t];
        this.mensagem = 'Turma criada com sucesso!';
        this.turmaForm.reset({ turno: 'Noturno', ano: new Date().getFullYear().toString() });
        this.criandoTurma = false;
        this.salvando = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.erro = err.error?.message || 'Erro ao criar turma.';
        this.salvando = false;
        this.cdr.detectChanges();
      }
    });
  }

  excluirTurma(id: number): void {
    if (!confirm('Deseja excluir esta turma?')) return;
    this.http.delete(`${this.api}/api/Turma/${id}`).subscribe({
      next: () => {
        this.turmas = this.turmas.filter(t => t.id !== id);
        if (this.turmaSelecionada?.id === id) this.turmaSelecionada = null;
        this.cdr.detectChanges();
      }
    });
  }

  selecionarTurma(turma: Turma): void {
    this.turmaSelecionada = turma;
    this.alunosDaTurma = this.todosAlunos.filter(a => turma.alunosIds?.includes(a.id));
    this.cdr.detectChanges();
  }

  alunosForaDaTurma(): Aluno[] {
    const ids = this.turmaSelecionada?.alunosIds || [];
    return this.todosAlunos.filter(a => !ids.includes(a.id));
  }

  matricularAluno(alunoId: number): void {
    if (!this.turmaSelecionada) return;
    this.http.post(`${this.api}/api/Turma/${this.turmaSelecionada.id}/matricular`, { alunoId }).subscribe({
      next: () => {
        this.turmaSelecionada!.alunosIds = [...(this.turmaSelecionada!.alunosIds || []), alunoId];
        this.alunosDaTurma = this.todosAlunos.filter(a => this.turmaSelecionada!.alunosIds.includes(a.id));
        this.turmaSelecionada!.qtdAlunos++;
        this.cdr.detectChanges();
      },
      error: (err) => { this.erro = err.error?.message || 'Erro ao matricular aluno.'; this.cdr.detectChanges(); }
    });
  }

  desmatricularAluno(alunoId: number): void {
    if (!this.turmaSelecionada) return;
    this.http.delete(`${this.api}/api/Turma/${this.turmaSelecionada.id}/desmatricular/${alunoId}`).subscribe({
      next: () => {
        this.turmaSelecionada!.alunosIds = this.turmaSelecionada!.alunosIds.filter(id => id !== alunoId);
        this.alunosDaTurma = this.alunosDaTurma.filter(a => a.id !== alunoId);
        this.turmaSelecionada!.qtdAlunos--;
        this.cdr.detectChanges();
      },
      error: (err) => { this.erro = err.error?.message || 'Erro ao desmatricular aluno.'; this.cdr.detectChanges(); }
    });
  }
}

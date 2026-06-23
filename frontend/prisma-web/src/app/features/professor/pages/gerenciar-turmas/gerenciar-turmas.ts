import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

interface Turma { id: number; nome: string; ano: string; turno: string; qtdAlunos: number; alunosIds: number[]; principal: boolean; }
interface Aluno { id: number; primeiroNome: string; ultimoNome: string; email: string; ra: string; }

@Component({
  selector: 'app-gerenciar-turmas',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './gerenciar-turmas.html'
})
export class GerenciarTurmasComponent implements OnInit {
  turmaForm!: FormGroup;
  turmas: Turma[] = [];
  turmaSelecionada: Turma | null = null;
  alunosDaTurma: Aluno[] = [];
  todosAlunos: Aluno[] = [];

  // Busca de alunos
  buscaMatricular = '';
  buscaDesmatricular = '';

  // Estado do formulário
  modoFormulario: 'criar' | 'editar' | null = null;
  salvando = false;
  mensagem = '';
  erro = '';

  private api = environment.gatewayUrl;

  constructor(private fb: FormBuilder, private http: HttpClient, private cdr: ChangeDetectorRef, private zone: NgZone) {}

  private atualizar(fn: () => void): void {
    this.zone.run(() => { fn(); this.cdr.detectChanges(); });
  }

  ngOnInit(): void {
    this.turmaForm = this.fb.group({
      nome: ['', Validators.required],
      ano: [new Date().getFullYear().toString(), Validators.required],
      turno: ['Noturno', Validators.required],
      principal: [false]
    });
    this.carregarTurmas();
    this.carregarAlunos();
  }

  carregarTurmas(): void {
    this.http.get<Turma[]>(`${this.api}/api/Turma`).subscribe({
      next: (data) => this.atualizar(() => { this.turmas = data; }),
      error: () => this.atualizar(() => { this.turmas = []; })
    });
  }

  carregarAlunos(): void {
    this.http.get<Aluno[]>(`${this.api}/api/Aluno/alunos`).subscribe({
      next: (data) => this.atualizar(() => { this.todosAlunos = data; }),
      error: () => this.atualizar(() => { this.todosAlunos = []; })
    });
  }

  abrirCriar(): void {
    this.modoFormulario = 'criar';
    this.turmaForm.reset({ turno: 'Noturno', ano: new Date().getFullYear().toString() });
    this.mensagem = '';
    this.erro = '';
  }

  abrirEditar(turma: Turma): void {
    this.modoFormulario = 'editar';
    this.turmaSelecionada = turma;
    this.turmaForm.patchValue({ nome: turma.nome, ano: turma.ano, turno: turma.turno, principal: turma.principal });
    this.mensagem = '';
    this.erro = '';
  }

  cancelarFormulario(): void {
    this.modoFormulario = null;
    this.turmaForm.reset({ turno: 'Noturno', ano: new Date().getFullYear().toString() });
  }

  salvarTurma(): void {
    if (this.turmaForm.invalid) { this.turmaForm.markAllAsTouched(); return; }
    this.salvando = true;
    this.erro = '';

    if (this.modoFormulario === 'editar' && this.turmaSelecionada) {
      this.http.put<Turma>(`${this.api}/api/Turma/${this.turmaSelecionada.id}`, this.turmaForm.value).subscribe({
        next: (t) => {
          this.atualizar(() => {
            const idx = this.turmas.findIndex(x => x.id === (this.turmaSelecionada?.id));
            if (idx >= 0) {
              // Preserva alunosIds e qtdAlunos — o PUT não retorna esses dados
              this.turmas[idx] = {
                ...this.turmas[idx],
                nome: t.nome || this.turmaForm.value.nome,
                ano: t.ano || this.turmaForm.value.ano,
                turno: t.turno || this.turmaForm.value.turno
              };
              this.turmaSelecionada = this.turmas[idx];
            }
            this.mensagem = 'Turma atualizada!';
            this.modoFormulario = null;
            this.salvando = false;
          });
        },
        error: (err) => this.atualizar(() => { this.erro = err.error?.message || 'Erro ao atualizar.'; this.salvando = false; })
      });
    } else {
      this.http.post<Turma>(`${this.api}/api/Turma`, this.turmaForm.value).subscribe({
        next: (t) => {
          this.atualizar(() => {
            this.turmas = [...this.turmas, t];
            this.mensagem = 'Turma criada!';
            this.modoFormulario = null;
            this.salvando = false;
          });
        },
        error: (err) => this.atualizar(() => { this.erro = err.error?.message || 'Erro ao criar.'; this.salvando = false; })
      });
    }
  }

  excluirTurma(id: number): void {
    if (!confirm('Deseja excluir esta turma?')) return;
    this.http.delete(`${this.api}/api/Turma/${id}`).subscribe({
      next: () => this.atualizar(() => {
        this.turmas = this.turmas.filter(t => t.id !== id);
        if (this.turmaSelecionada?.id === id) this.turmaSelecionada = null;
      })
    });
  }

  selecionarTurma(turma: Turma): void {
    this.turmaSelecionada = turma;
    this.alunosDaTurma = this.todosAlunos.filter(a => turma.alunosIds?.includes(a.id));
    this.buscaMatricular = '';
    this.buscaDesmatricular = '';
    this.modoFormulario = null;
    this.cdr.detectChanges();
  }

  get alunosForaDaTurma(): Aluno[] {
    const ids = this.turmaSelecionada?.alunosIds || [];
    const busca = this.buscaMatricular.toLowerCase();
    return this.todosAlunos.filter(a => {
      const fora = !ids.includes(a.id);
      const matchBusca = !busca ||
        `${a.primeiroNome} ${a.ultimoNome}`.toLowerCase().includes(busca) ||
        a.ra.toLowerCase().includes(busca);
      return fora && matchBusca;
    });
  }

  get alunosDaTurmaFiltrados(): Aluno[] {
    const busca = this.buscaDesmatricular.toLowerCase();
    if (!busca) return this.alunosDaTurma;
    return this.alunosDaTurma.filter(a =>
      `${a.primeiroNome} ${a.ultimoNome}`.toLowerCase().includes(busca) ||
      a.ra.toLowerCase().includes(busca)
    );
  }

  matricularAluno(alunoId: number): void {
    if (!this.turmaSelecionada) return;
    this.http.post(`${this.api}/api/Turma/${this.turmaSelecionada.id}/matricular`, { alunoId }).subscribe({
      next: () => this.atualizar(() => {
        this.turmaSelecionada!.alunosIds = [...(this.turmaSelecionada!.alunosIds || []), alunoId];
        this.alunosDaTurma = this.todosAlunos.filter(a => this.turmaSelecionada!.alunosIds.includes(a.id));
        this.turmaSelecionada!.qtdAlunos++;
        this.buscaMatricular = '';
      }),
      error: (err) => this.atualizar(() => { this.erro = err.error?.message || 'Erro ao matricular.'; })
    });
  }

  desmatricularAluno(alunoId: number): void {
    if (!this.turmaSelecionada) return;
    this.http.delete(`${this.api}/api/Turma/${this.turmaSelecionada.id}/desmatricular/${alunoId}`).subscribe({
      next: () => this.atualizar(() => {
        this.turmaSelecionada!.alunosIds = this.turmaSelecionada!.alunosIds.filter(id => id !== alunoId);
        this.alunosDaTurma = this.alunosDaTurma.filter(a => a.id !== alunoId);
        this.turmaSelecionada!.qtdAlunos--;
      }),
      error: (err) => this.atualizar(() => { this.erro = err.error?.message || 'Erro ao desmatricular.'; })
    });
  }
}

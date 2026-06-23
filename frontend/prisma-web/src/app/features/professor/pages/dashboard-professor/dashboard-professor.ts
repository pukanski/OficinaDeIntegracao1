import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

interface Turma { id: number; nome: string; ano: string; turno: string; qtdAlunos: number; alunosIds: number[]; }
interface DesempenhoAluno { percentualAcerto: number; totalRespondidas: number; }
interface DisciplinaStats { disciplina: string; totalRespondidas: number; totalAcertos: number; percentualAcerto: number; }
interface AlunoRisco { id: number; nome: string; percentual: number; }

@Component({
  selector: 'app-dashboard-professor',
  standalone: true,
  imports: [CommonModule, DecimalPipe, RouterModule],
  templateUrl: './dashboard-professor.html'
})
export class DashboardProfessorComponent implements OnInit {
  nomeProfessor = '';
  totalAlunos = 0;
  totalQuestoes = 0;
  totalListas = 0;
  mediaDesempenho = 0;

  turmas: Turma[] = [];
  turmaSelecionadaId: number | null = null;
  turmaSelecionada: Turma | null = null;

  disciplinasStats: DisciplinaStats[] = [];
  alunosRisco: AlunoRisco[] = [];
  todosAlunos: any[] = [];

  carregando = true;
  carregandoTurma = false;

  private api = environment.gatewayUrl;
  private professorId: number | null = null;

  constructor(private http: HttpClient, private cdr: ChangeDetectorRef, private zone: NgZone) {}

  private atualizar(fn: () => void): void {
    this.zone.run(() => { fn(); this.cdr.detectChanges(); });
  }

  ngOnInit(): void {
    this.http.get<any>(`${this.api}/api/Professor/me`).subscribe({
      next: (prof) => {
        this.professorId = prof.id;
        this.atualizar(() => { this.nomeProfessor = `${prof.primeiroNome} ${prof.ultimoNome}`; });
        this.carregarDados();
      }
    });
  }

  carregarDados(): void {
    // Turmas
    this.http.get<Turma[]>(`${this.api}/api/Turma`).subscribe({
      next: (turmas) => {
        this.atualizar(() => {
          this.turmas = turmas;
          const todosIds = new Set(turmas.flatMap(t => t.alunosIds || []));
          this.totalAlunos = todosIds.size;
        });
        if (turmas.length > 0) this.selecionarTurma(turmas[0].id);
      }
    });

    // Total de questões no banco
    this.http.get<any[]>(`${this.api}/api/Questao/Questoes`).subscribe({
      next: (q) => this.atualizar(() => { this.totalQuestoes = q.length; })
    });

    // Total de listas do professor
    if (this.professorId) {
      this.http.get<any[]>(`${this.api}/api/Lista/professor/${this.professorId}`).subscribe({
        next: (l) => this.atualizar(() => { this.totalListas = l.length; this.carregando = false; }),
        error: () => this.atualizar(() => { this.carregando = false; })
      });
    } else {
      this.atualizar(() => { this.carregando = false; });
    }

    // Todos os alunos para lookup de nome
    this.http.get<any[]>(`${this.api}/api/Aluno/alunos`).subscribe({
      next: (a) => this.atualizar(() => { this.todosAlunos = a; })
    });
  }

  selecionarTurma(turmaId: number): void {
    this.turmaSelecionadaId = turmaId;
    this.turmaSelecionada = this.turmas.find(t => t.id === turmaId) || null;
    this.carregandoTurma = true;
    this.disciplinasStats = [];
    this.alunosRisco = [];
    this.mediaDesempenho = 0;
    this.cdr.detectChanges();

    const alunosIds = this.turmaSelecionada?.alunosIds || [];
    if (alunosIds.length === 0) {
      this.atualizar(() => { this.carregandoTurma = false; });
      return;
    }

    // Busca desempenho de cada aluno
    const disciplinaMap = new Map<string, { acertos: number; total: number }>();
    const alunosDesempenho: { id: number; percentual: number }[] = [];
    let pendentes = alunosIds.length;
    let somaMedia = 0;

    alunosIds.forEach(alunoId => {
      this.http.get<any>(`${this.api}/api/Dados/${alunoId}/desempenho`).subscribe({
        next: (d) => {
          somaMedia += d.percentualAcerto || 0;
          alunosDesempenho.push({ id: alunoId, percentual: d.percentualAcerto || 0 });
          pendentes--;
          if (pendentes === 0) this._finalizarCarregamentoTurma(alunosDesempenho, somaMedia, alunosIds.length);
        },
        error: () => {
          alunosDesempenho.push({ id: alunoId, percentual: 0 });
          pendentes--;
          if (pendentes === 0) this._finalizarCarregamentoTurma(alunosDesempenho, somaMedia, alunosIds.length);
        }
      });
    });

    // Desempenho por disciplina de um aluno representativo
    this.http.get<any>(`${this.api}/api/Dados/${alunosIds[0]}/desempenho/disciplinas`).subscribe({
      next: (d) => this.atualizar(() => { this.disciplinasStats = d.disciplinas || []; })
    });
  }

  private _finalizarCarregamentoTurma(desempenhos: { id: number; percentual: number }[], soma: number, total: number): void {
    const media = total > 0 ? Math.round(soma / total) : 0;
    const emRisco = desempenhos
      .filter(a => a.percentual < 60)
      .map(a => {
        const aluno = this.todosAlunos.find(x => x.id === a.id);
        return { id: a.id, nome: aluno ? `${aluno.primeiroNome} ${aluno.ultimoNome}` : `Aluno #${a.id}`, percentual: a.percentual };
      })
      .sort((a, b) => a.percentual - b.percentual)
      .slice(0, 5);

    this.atualizar(() => {
      this.mediaDesempenho = media;
      this.alunosRisco = emRisco;
      this.carregandoTurma = false;
    });
  }
}

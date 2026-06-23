import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

interface ListaDTO { id: number; titulo: string; questoesIds: number[]; dataVencimento?: string; }
interface DisciplinaDesempenho { disciplina: string; totalRespondidas: number; totalAcertos: number; percentualAcerto: number; }

@Component({
  selector: 'app-dashboard-aluno',
  standalone: true,
  imports: [CommonModule, DecimalPipe],
  templateUrl: './dashboard-aluno.html'
})
export class DashboardAlunoComponent implements OnInit {
  alunoId: number | null = null;
  nomeAluno = '';

  mediaGeral = 0;
  totalRespondidas = 0;
  percentualFrequencia = 0;
  melhorDisciplina = '-';
  melhorAcerto = 0;

  disciplinas: DisciplinaDesempenho[] = [];
  listas: ListaDTO[] = [];
  carregando = true;

  private api = environment.gatewayUrl;

  constructor(private router: Router, private http: HttpClient, private cdr: ChangeDetectorRef, private zone: NgZone) {}

  private atualizar(fn: () => void): void {
    this.zone.run(() => { fn(); this.cdr.detectChanges(); });
  }

  ngOnInit(): void {
    this.http.get<any>(`${this.api}/api/Aluno/me`).subscribe({
      next: (aluno) => {
        this.alunoId = aluno.id;
        this.nomeAluno = `${aluno.primeiroNome} ${aluno.ultimoNome}`;
        this.carregarDesempenho(aluno.id);
        this.carregarListas(aluno.id);
        this.carregarFrequencia(aluno.id);
      }
    });
  }

  carregarDesempenho(alunoId: number): void {
    this.http.get<any>(`${this.api}/api/Dados/${alunoId}/desempenho`).subscribe({
      next: (d) => {
        this.atualizar(() => {
          this.mediaGeral = d.percentualAcerto || 0;
          this.totalRespondidas = d.totalRespondidas || 0;
        });
      }
    });

    this.http.get<any>(`${this.api}/api/Dados/${alunoId}/desempenho/disciplinas`).subscribe({
      next: (d) => {
        this.atualizar(() => {
          this.disciplinas = d.disciplinas || [];
          if (this.disciplinas.length > 0) {
            const melhor = this.disciplinas.reduce((a: any, b: any) => a.percentualAcerto > b.percentualAcerto ? a : b);
            this.melhorDisciplina = melhor.disciplina;
            this.melhorAcerto = melhor.percentualAcerto;
          }
          this.carregando = false;
        });
      },
      error: () => this.atualizar(() => { this.carregando = false; })
    });
  }

  carregarFrequencia(alunoId: number): void {
    // Busca turmas do aluno e calcula frequência média
    this.http.get<any[]>(`${this.api}/api/Turma`).subscribe({
      next: (turmas) => {
        const minhas = turmas.filter(t => t.alunosIds?.includes(alunoId));
        if (minhas.length === 0) return;

        let totalPercentual = 0;
        let count = 0;
        minhas.forEach(t => {
          this.http.get<any>(`${this.api}/api/Frequencia/percentual/${alunoId}/${t.id}`).subscribe({
            next: (f) => {
              totalPercentual += f.percentual || 0;
              count++;
              if (count === minhas.length) {
                this.atualizar(() => { this.percentualFrequencia = Math.round(totalPercentual / count); });
              }
            }
          });
        });
      }
    });
  }

  carregarListas(alunoId: number): void {
    this.http.get<any[]>(`${this.api}/api/Turma`).subscribe({
      next: (turmas) => {
        const minhas = turmas.filter(t => t.alunosIds?.includes(alunoId));
        const listasSet = new Map<number, ListaDTO>();
        let pendentes = minhas.length;
        if (pendentes === 0) return;

        minhas.forEach(t => {
          this.http.get<ListaDTO[]>(`${this.api}/api/Lista/turma/${t.id}`).subscribe({
            next: (ls) => {
              ls.forEach(l => listasSet.set(l.id, l));
              pendentes--;
              if (pendentes === 0) this.atualizar(() => { this.listas = Array.from(listasSet.values()); });
            },
            error: () => { pendentes--; }
          });
        });
      }
    });
  }

  iniciarLista(lista: ListaDTO): void {
    this.router.navigate(['/aluno/resolucao', lista.id], { state: { lista, alunoId: this.alunoId } });
  }

  irParaBanco(): void {
    this.router.navigate(['/aluno/banco']);
  }
}

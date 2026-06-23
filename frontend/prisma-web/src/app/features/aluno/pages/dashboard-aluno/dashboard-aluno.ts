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
    this.http.get<any[]>(`${this.api}/api/Turma`).subscribe({
      next: (turmas) => {
        const minhas = turmas.filter(t =>
          (t.alunosIds ?? t.AlunosIds ?? []).includes(alunoId)
        );
        const principais = minhas.filter(t => t.Principal === true || t.principal === true);
        const alvo = principais.length > 0 ? principais : minhas;
        if (alvo.length === 0) return;
        this._calcularFrequencia(alunoId, alvo);
      }
    });
  }

  private _calcularFrequencia(alunoId: number, turmas: any[]): void {
    const resultados: number[] = [];
    let pendentes = turmas.length;

    turmas.forEach(t => {
      const turmaId = t.id ?? t.Id ?? t.ID;
      this.http.get<any>(`${this.api}/api/Frequencia/percentual/${alunoId}/${turmaId}`).subscribe({
        next: (f) => {
          const total = f.totalAulas ?? f.TotalAulas ?? 0;
          // Só inclui na média se houver aulas registradas para essa turma
          if (total > 0) {
            resultados.push(f.percentual ?? f.Percentual ?? 0);
          }
          pendentes--;
          if (pendentes === 0) {
            const media = resultados.length > 0
              ? Math.round(resultados.reduce((a, b) => a + b, 0) / resultados.length)
              : 0;
            this.atualizar(() => { this.percentualFrequencia = media; });
          }
        },
        error: () => {
          pendentes--;
          if (pendentes === 0) this.atualizar(() => {});
        }
      });
    });
  }

  carregarListas(alunoId: number): void {
    this.http.get<any[]>(`${this.api}/api/Turma`).subscribe({
      next: (turmas) => {
        const turmaIds = turmas
          .filter(t => (t.alunosIds ?? t.AlunosIds ?? []).includes(alunoId))
          .map(t => t.id ?? t.Id ?? t.ID);

        if (turmaIds.length === 0) return;

        Promise.all(
          turmaIds.map(id =>
            this.http.get<any[]>(`${this.api}/api/Lista/turma/${id}`)
              .toPromise().catch(() => [] as any[])
          )
        ).then(resultados => {
          const listasSet = new Map<number, any>();
          (resultados as any[][]).flat().forEach((l: any) => {
            const lid = l.id ?? l.Id ?? l.ID;
            listasSet.set(lid, l);
          });
          this.atualizar(() => { this.listas = Array.from(listasSet.values()); });
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

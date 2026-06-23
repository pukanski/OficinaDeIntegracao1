import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

interface ListaDTO {
  id: number;
  titulo: string;
  dataVencimento?: string;
  questoesIds: number[];
}

@Component({
  selector: 'app-meus-exercicios',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './meus-exercicios.html'
})
export class MeusExerciciosComponent implements OnInit {
  listas: ListaDTO[] = [];
  alunoId: number | null = null;
  carregando = false;
  erro = '';

  private api = environment.gatewayUrl;

  constructor(
    private router: Router,
    private http: HttpClient,
    private cdr: ChangeDetectorRef,
    private zone: NgZone
  ) {}

  ngOnInit(): void {
    this.carregarAluno();
  }

  private atualizar(fn: () => void): void {
    this.zone.run(() => { fn(); this.cdr.detectChanges(); });
  }

  carregarAluno(): void {
    this.http.get<any>(`${this.api}/api/Aluno/me`).subscribe({
      next: (aluno) => {
        this.atualizar(() => { this.alunoId = aluno.id; });
        this.carregarListas(aluno.id);
      },
      error: () => this.atualizar(() => { this.erro = 'Não foi possível identificar o aluno.'; })
    });
  }

  carregarListas(alunoId: number): void {
    this.atualizar(() => { this.carregando = true; });

    this.http.get<any[]>(`${this.api}/api/Turma`).subscribe({
      next: (turmas) => {
        const turmaIds = turmas.filter(t => t.alunosIds?.includes(alunoId)).map(t => t.id);
        const listasSet = new Map<number, ListaDTO>();
        let pendentes = turmaIds.length;

        if (pendentes === 0) {
          this.atualizar(() => { this.carregando = false; });
          return;
        }

        turmaIds.forEach(turmaId => {
          this.http.get<ListaDTO[]>(`${this.api}/api/Lista/turma/${turmaId}`).subscribe({
            next: (listas) => {
              listas.forEach(l => listasSet.set(l.id, l));
              pendentes--;
              if (pendentes === 0) {
                this.atualizar(() => {
                  this.listas = Array.from(listasSet.values());
                  this.carregando = false;
                });
              }
            },
            error: () => {
              pendentes--;
              if (pendentes === 0) this.atualizar(() => { this.carregando = false; });
            }
          });
        });
      },
      error: () => this.atualizar(() => { this.erro = 'Erro ao carregar listas.'; this.carregando = false; })
    });
  }

  iniciarResolucao(lista: ListaDTO): void {
    this.router.navigate(['/aluno/resolucao', lista.id], {
      state: { lista, alunoId: this.alunoId }
    });
  }
}

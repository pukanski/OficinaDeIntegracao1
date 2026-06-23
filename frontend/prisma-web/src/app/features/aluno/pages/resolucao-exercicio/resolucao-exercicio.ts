import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

interface AlternativaDTO { id: number; letra: string; texto: string; correta: boolean; }
interface QuestaoDTO { id: number; enunciado: string; disciplina: string; materia?: string; dificuldade?: string; provaDescricao?: string; vestibular?: string; anoProva?: number; alternativas: AlternativaDTO[]; }

@Component({
  selector: 'app-resolucao-exercicio',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './resolucao-exercicio.html'
})
export class ResolucaoExercicioComponent implements OnInit {
  listaId!: number;
  tituloLista = '';
  questoes: QuestaoDTO[] = [];
  indiceAtual = 0;
  respostasSalvas = new Map<number, number>(); // questaoId -> alternativaId
  alunoId: number | null = null;
  carregando = false;
  enviando = false;

  private api = environment.gatewayUrl;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private cdr: ChangeDetectorRef,
    private zone: NgZone
  ) {}

  private atualizar(fn: () => void): void {
    this.zone.run(() => { fn(); this.cdr.detectChanges(); });
  }

  ngOnInit(): void {
    this.listaId = +this.route.snapshot.paramMap.get('id')!;
    const state = history.state;
    this.alunoId = state?.alunoId || null;
    this.tituloLista = state?.lista?.titulo || 'Lista de Exercícios';

    if (!this.alunoId) {
      this.http.get<any>(`${this.api}/api/Aluno/me`).subscribe({
        next: (a) => { this.alunoId = a.id; }
      });
    }

    this.carregarQuestoes();
  }

  carregarQuestoes(): void {
    this.carregando = true;
    this.http.get<any>(`${this.api}/api/Lista/${this.listaId}`).subscribe({
      next: (lista) => {
        this.tituloLista = lista.titulo;
        const ids: number[] = lista.questoesIds;
        const questoesTemp: QuestaoDTO[] = [];
        let pendentes = ids.length;

        if (pendentes === 0) { this.atualizar(() => { this.carregando = false; }); return; }

        ids.forEach(id => {
          this.http.get<QuestaoDTO>(`${this.api}/api/Questao/Questoes/${id}`).subscribe({
            next: (q) => {
              questoesTemp.push(q);
              pendentes--;
              if (pendentes === 0) {
                // Ordena pela ordem original dos IDs
                this.atualizar(() => {
                  this.questoes = ids.map(i => questoesTemp.find(q => q.id === i)!).filter(Boolean);
                  this.carregando = false;
                });
              }
            },
            error: () => { pendentes--; if (pendentes === 0) this.atualizar(() => { this.carregando = false; }); }
          });
        });
      },
      error: () => this.atualizar(() => { this.carregando = false; })
    });
  }

  get questaoAtual(): QuestaoDTO { return this.questoes[this.indiceAtual]; }
  get totalRespondidas(): number { return this.respostasSalvas.size; }
  get progressoPercentual(): number { return this.questoes.length ? (this.totalRespondidas / this.questoes.length) * 100 : 0; }
  get ehUltimaQuestao(): boolean { return this.indiceAtual === this.questoes.length - 1; }

  alternativaSelecionada(questaoId: number): number | undefined { return this.respostasSalvas.get(questaoId); }
  estaSelecionada(questaoId: number, altId: number): boolean { return this.respostasSalvas.get(questaoId) === altId; }

  selecionarAlternativa(altId: number): void {
    this.atualizar(() => { this.respostasSalvas.set(this.questaoAtual.id, altId); });
  }

  irPara(i: number): void { this.atualizar(() => { this.indiceAtual = i; }); }
  anterior(): void { if (this.indiceAtual > 0) this.atualizar(() => { this.indiceAtual--; }); }
  proxima(): void { if (!this.ehUltimaQuestao) this.atualizar(() => { this.indiceAtual++; }); }

  obterStatus(i: number): 'atual' | 'respondida' | 'pendente' {
    if (i === this.indiceAtual) return 'atual';
    if (this.questoes[i] && this.respostasSalvas.has(this.questoes[i].id)) return 'respondida';
    return 'pendente';
  }

  finalizarLista(): void {
    if (this.totalRespondidas < this.questoes.length) {
      if (!confirm('Você não respondeu todas as questões. Deseja finalizar mesmo assim?')) return;
    }

    if (!this.alunoId) { alert('Não foi possível identificar o aluno.'); return; }

    this.enviando = true;
    const respostas = this.questoes.map(q => {
      const altId = this.respostasSalvas.get(q.id);
      const alt = q.alternativas.find(a => a.id === altId);
      return {
        alunoId: this.alunoId,
        questaoId: q.id,
        alternativaId: altId || 0,
        disciplina: q.disciplina || 'Geral',
        vestibular: q.provaDescricao || 'Avulsa',
        anoProva: q.anoProva || new Date().getFullYear(),
        dificuldade: q.dificuldade || null,
        acertou: alt?.correta ?? false
      };
    }).filter(r => r.alternativaId > 0);

    // Envia todas as respostas
    Promise.all(
      respostas.map(r => this.http.post(`${this.api}/api/Dados/responder`, r).toPromise().catch(() => null))
    ).then(() => {
      this.enviando = false;
      // Navega para o gabarito passando o mapa de respostas
      this.router.navigate(['/aluno/gabarito', this.listaId], {
        state: { questoes: this.questoes, respostasSalvas: Object.fromEntries(this.respostasSalvas) }
      });
    });
  }
}

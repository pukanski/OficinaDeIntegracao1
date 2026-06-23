import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { Questao } from '../../../../core/models/questao.model';

const DIFICULDADE_ORDEM: Record<string, number> = {
  'Muito Fácil': 1, 'Fácil': 2, 'Médio': 3, 'Difícil': 4, 'Muito Difícil': 5
};

@Component({
  selector: 'app-banco-livre',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './banco-livre.html'
})
export class BancoLivreComponent implements OnInit {
  questoes: Questao[] = [];

  termoBusca = '';
  filtroDisciplina = 'Todas';
  filtroProva = 'Todas';
  filtroMicroArea = '';
  filtroDificuldade = 'Todas';

  modalAberto = false;
  questaoFoco: Questao | null = null;
  alternativaSelecionada: number | null = null;
  resultado: 'acerto' | 'erro' | null = null;

  carregando = false;
  erro = '';
  alunoId: number | null = null;

  private apiUrl = `${environment.gatewayUrl}/api/Questao/Questoes`;
  private api = environment.gatewayUrl;

  constructor(private http: HttpClient, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.http.get<any>(`${this.api}/api/Aluno/me`).subscribe({
      next: (a) => { this.alunoId = a.id; }
    });
    this.carregarQuestoes();
  }

  carregarQuestoes(): void {
    this.carregando = true;
    this.erro = '';
    this.http.get<Questao[]>(this.apiUrl).subscribe({
      next: (data) => { this.questoes = data; this.carregando = false; this.cdr.detectChanges(); },
      error: () => { this.erro = 'Erro ao carregar questões.'; this.carregando = false; this.cdr.detectChanges(); }
    });
  }

  get disciplinasUnicas(): string[] {
    return [...new Set(this.questoes.map(q => q.disciplina).filter(Boolean))];
  }

  get provasUnicas(): string[] {
    return [...new Set(this.questoes.map(q => q.provaDescricao).filter((p): p is string => !!p))];
  }

  get questoesFiltradas(): Questao[] {
    return this.questoes.filter(q => {
      const matchBusca = !this.termoBusca || q.enunciado.toLowerCase().includes(this.termoBusca.toLowerCase());
      const matchDisc = this.filtroDisciplina === 'Todas' || q.disciplina === this.filtroDisciplina;
      const matchMicro = !this.filtroMicroArea || (q.materia || '').toLowerCase().includes(this.filtroMicroArea.toLowerCase());
      const matchDif = this.filtroDificuldade === 'Todas' || q.dificuldade === this.filtroDificuldade;
      const matchProva = this.filtroProva === 'Todas' ||
        (this.filtroProva === 'Avulsa' ? !q.provaDescricao : q.provaDescricao === this.filtroProva);
      return matchBusca && matchDisc && matchMicro && matchDif && matchProva;
    });
  }

  getDificuldadeStars(dificuldade?: string): boolean[] {
    const nivel = DIFICULDADE_ORDEM[dificuldade || ''] || 0;
    return Array(5).fill(false).map((_, i) => i < nivel);
  }

  abrirModal(questaoId: number): void {
    this.questaoFoco = this.questoes.find(q => q.id === questaoId) || null;
    this.alternativaSelecionada = null;
    this.resultado = null;
    this.modalAberto = true;
    this.cdr.detectChanges();
  }

  fecharModal(): void {
    this.modalAberto = false;
    this.questaoFoco = null;
    this.alternativaSelecionada = null;
    this.resultado = null;
    this.cdr.detectChanges();
  }

  selecionar(altId: number): void {
    if (this.resultado) return;
    this.alternativaSelecionada = altId;
    this.cdr.detectChanges();
  }

  responder(): void {
    if (!this.questaoFoco || !this.alternativaSelecionada || !this.alunoId) return;
    const alt = this.questaoFoco.alternativas.find(a => a.id === this.alternativaSelecionada);
    if (!alt) return;

    this.resultado = alt.correta ? 'acerto' : 'erro';
    this.cdr.detectChanges();

    this.http.post(`${this.api}/api/Dados/responder`, {
      alunoId: this.alunoId,
      questaoId: this.questaoFoco.id,
      alternativaId: alt.id,
      disciplina: this.questaoFoco.disciplina || 'Geral',
      vestibular: this.questaoFoco.provaDescricao || 'Banco Livre',
      anoProva: new Date().getFullYear(),
      dificuldade: this.questaoFoco.dificuldade || null,
      acertou: alt.correta
    }).subscribe();
  }
}

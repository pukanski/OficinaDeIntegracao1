import { Component, OnInit } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';

interface AlternativaDTO { id: number; letra: string; texto: string; correta: boolean; }
interface QuestaoGab { id: number; enunciado: string; disciplina: string; alternativas: AlternativaDTO[]; }

@Component({
  selector: 'app-gabarito-lista',
  standalone: true,
  imports: [CommonModule, RouterModule, DecimalPipe],
  templateUrl: './gabarito-lista.html'
})
export class GabaritoListaComponent implements OnInit {
  questoes: QuestaoGab[] = [];
  respostasSalvas: Record<number, number> = {}; // questaoId -> alternativaId

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    const state = history.state;
    this.questoes = state?.questoes || [];
    this.respostasSalvas = state?.respostasSalvas || {};
  }

  gabarito(q: QuestaoGab): AlternativaDTO | undefined {
    return q.alternativas.find(a => a.correta);
  }

  respostaAluno(q: QuestaoGab): AlternativaDTO | undefined {
    const altId = this.respostasSalvas[q.id];
    return q.alternativas.find(a => a.id === altId);
  }

  acertou(q: QuestaoGab): boolean {
    const resp = this.respostaAluno(q);
    return resp?.correta ?? false;
  }

  get totalAcertos(): number { return this.questoes.filter(q => this.acertou(q)).length; }
  get percentualAcertos(): number { return this.questoes.length ? (this.totalAcertos / this.questoes.length) * 100 : 0; }
}

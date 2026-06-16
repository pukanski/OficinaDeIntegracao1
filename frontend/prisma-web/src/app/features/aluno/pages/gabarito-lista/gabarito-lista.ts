import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';

interface AlternativaGabDTO { letra: string; texto: string; }

// DTO esperado da API (GET /api/aluno/gabarito/{listaId})
interface QuestaoGabDTO {
  id: string;
  enunciado: string;
  alternativas: AlternativaGabDTO[];
  gabaritoOficial: string;
  respostaAluno: string; // A letra que o aluno marcou
}

@Component({
  selector: 'app-gabarito-lista',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './gabarito-lista.html'
})
export class GabaritoListaComponent implements OnInit {
  listaId: string = '';
  tituloLista: string = 'Revisão - Equações Quadráticas';
  questoes: QuestaoGabDTO[] = [];

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.listaId = this.route.snapshot.paramMap.get('id') || '';

    // Mock do payload devolvido pela API após a correção
    this.questoes = [
      {
        id: 'q1',
        enunciado: 'Resolver a equação quadrática: x² - 5x + 6 = 0',
        alternativas: [
          { letra: 'A', texto: 'x = 2 ou x = 3' },
          { letra: 'B', texto: 'x = 1 ou x = 4' },
          { letra: 'C', texto: 'x = 2 ou x = 4' },
          { letra: 'D', texto: 'x = 3 ou x = 6' }
        ],
        gabaritoOficial: 'A',
        respostaAluno: 'A' // Exemplo de Acerto
      },
      {
        id: 'q2',
        enunciado: 'Qual a raiz quadrada de 144?',
        alternativas: [
          { letra: 'A', texto: '10' },
          { letra: 'B', texto: '12' },
          { letra: 'C', texto: '14' },
          { letra: 'D', texto: '16' }
        ],
        gabaritoOficial: 'B',
        respostaAluno: 'D' // Exemplo de Erro
      }
    ];
  }

  // Getters reativos para o placar final
  get totalAcertos(): number {
    return this.questoes.filter(q => q.gabaritoOficial === q.respostaAluno).length;
  }

  get percentualAcertos(): number {
    return (this.totalAcertos / this.questoes.length) * 100;
  }
}
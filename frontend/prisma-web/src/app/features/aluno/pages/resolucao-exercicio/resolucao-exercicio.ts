import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

// DTOs para comunicação com a API
interface AlternativaDTO { letra: string; texto: string; }
interface QuestaoResolucaoDTO { id: string; enunciado: string; alternativas: AlternativaDTO[]; }

@Component({
  selector: 'app-resolucao-exercicio',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './resolucao-exercicio.html'
})
export class ResolucaoExercicioComponent implements OnInit {
  listaId!: string;
  tituloLista: string = 'Revisão - Equações Quadráticas';

  // Estado local
  questoes: QuestaoResolucaoDTO[] = [];
  indiceAtual: number = 0;

  // Mapa para guardar as respostas (Chave: ID da questão, Valor: Letra selecionada)
  respostasSalvas: Map<string, string> = new Map();

  constructor(private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    // 1. Pega o ID da lista que veio na URL
    this.listaId = this.route.snapshot.paramMap.get('id') || '';

    // 2. Simula o GET /api/listas/{id}/questoes
    this.questoes = [
      {
        id: 'q1',
        enunciado: 'Resolver a equação quadrática: x² - 5x + 6 = 0',
        alternativas: [
          { letra: 'A', texto: 'x = 2 ou x = 3' },
          { letra: 'B', texto: 'x = 1 ou x = 4' },
          { letra: 'C', texto: 'x = 2 ou x = 4' },
          { letra: 'D', texto: 'x = 3 ou x = 6' },
          { letra: 'E', texto: 'x = 1 ou x = 2' }
        ]
      },
      {
        id: 'q2',
        enunciado: 'Qual a raiz quadrada de 144?',
        alternativas: [
          { letra: 'A', texto: '10' }, { letra: 'B', texto: '12' }, { letra: 'C', texto: '14' }, { letra: 'D', texto: '16' }, { letra: 'E', texto: '18' }
        ]
      },
      {
        id: 'q3',
        enunciado: 'A soma dos ângulos internos de um triângulo é:',
        alternativas: [
          { letra: 'A', texto: '90 graus' }, { letra: 'B', texto: '180 graus' }, { letra: 'C', texto: '270 graus' }, { letra: 'D', texto: '360 graus' }, { letra: 'E', texto: 'Nenhuma das anteriores' }
        ]
      }
    ];
  }

  // Getters para a Interface
  get questaoAtual(): QuestaoResolucaoDTO { return this.questoes[this.indiceAtual]; }
  get totalRespondidas(): number { return this.respostasSalvas.size; }
  get progressoPercentual(): number { return (this.totalRespondidas / this.questoes.length) * 100; }
  get ehUltimaQuestao(): boolean { return this.indiceAtual === this.questoes.length - 1; }

  // Ações
  selecionarAlternativa(letra: string): void {
    this.respostasSalvas.set(this.questaoAtual.id, letra);
  }

  alternativaEstaSelecionada(letra: string): boolean {
    return this.respostasSalvas.get(this.questaoAtual.id) === letra;
  }

  irPara(indice: number): void { this.indiceAtual = indice; }
  anterior(): void { if (this.indiceAtual > 0) this.indiceAtual--; }
  proxima(): void { if (!this.ehUltimaQuestao) this.indiceAtual++; }

  obterStatusNavegador(indice: number): 'atual' | 'respondida' | 'pendente' {
    if (indice === this.indiceAtual) return 'atual';
    if (this.respostasSalvas.has(this.questoes[indice].id)) return 'respondida';
    return 'pendente';
  }

  finalizarLista(): void {
    if (this.totalRespondidas < this.questoes.length) {
      const confirma = confirm('Você não respondeu todas as questões. Deseja finalizar mesmo assim?');
      if (!confirma) return;
    }

    // Estrutura o DTO final para o back-end efetuar a correção automática (RF08)
    const payloadCorrecao = {
      listaId: this.listaId,
      respostas: Array.from(this.respostasSalvas.entries()).map(([questaoId, alternativa]) => ({
        questaoId,
        alternativaSelecionada: alternativa
      }))
    };

    console.log('Enviando para o C# corrigir:', payloadCorrecao);
    alert('Respostas enviadas! A tela de correção com o gabarito será carregada em seguida (Próxima etapa).');
    this.router.navigate(['/aluno/gabarito', this.listaId]);
  }
}
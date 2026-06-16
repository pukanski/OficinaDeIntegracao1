import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Questao } from '../../../../core/models/questao.model';

@Component({
  selector: 'app-banco-questoes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './banco-questoes.html'
})
export class BancoQuestoesComponent implements OnInit {
  questoes: Questao[] = [];
  questoesSelecionadas: Set<string> = new Set();

  termoBusca: string = '';
  filtroDisciplina: string = 'Todas';
  filtroMacroArea: string = 'Todas';
  filtroMicroArea: string = '';
  filtroDificuldade: string = 'Todas';

  // --- NOVAS VARIÁVEIS PARA O MODAL ---
  modalAberto: boolean = false;
  modoModal: 'visualizar' | 'excluir' = 'visualizar';
  questaoFoco: Questao | null = null;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.questoes = [
      { id: '1', enunciado: 'Resolver a equação quadrática x² - 5x + 6 = 0', disciplina: 'Matemática', macroArea: 'Álgebra', microArea: 'Equações Quadráticas', dificuldade: 3 },
      { id: '2', enunciado: 'Calcular a área de um triângulo retângulo', disciplina: 'Matemática', macroArea: 'Geometria', microArea: 'Triângulos', dificuldade: 2 },
      { id: '3', enunciado: 'Explicar a Lei de Newton', disciplina: 'Física', macroArea: 'Mecânica', microArea: 'Dinâmica', dificuldade: 4 },
      { id: '4', enunciado: 'Balancear equação química de combustão', disciplina: 'Química', macroArea: 'Reações Químicas', microArea: 'Combustão', dificuldade: 3 }
    ];
  }

  get disciplinasUnicas(): string[] { return [...new Set(this.questoes.map(q => q.disciplina))]; }
  get macroAreasUnicas(): string[] { return [...new Set(this.questoes.map(q => q.macroArea))]; }

  get questoesFiltradas(): Questao[] {
    return this.questoes.filter(q => {
      const matchBusca = q.enunciado.toLowerCase().includes(this.termoBusca.toLowerCase());
      const matchDisciplina = this.filtroDisciplina === 'Todas' || q.disciplina === this.filtroDisciplina;
      const matchMacroArea = this.filtroMacroArea === 'Todas' || q.macroArea === this.filtroMacroArea;
      const matchMicroArea = q.microArea.toLowerCase().includes(this.filtroMicroArea.toLowerCase());
      const matchDificuldade = this.filtroDificuldade === 'Todas' || q.dificuldade.toString() === this.filtroDificuldade;

      return matchBusca && matchDisciplina && matchMacroArea && matchMicroArea && matchDificuldade;
    });
  }

  toggleSelecao(id: string): void {
    if (this.questoesSelecionadas.has(id)) {
      this.questoesSelecionadas.delete(id);
    } else {
      this.questoesSelecionadas.add(id);
    }
  }

  getDificuldadeArray(nivel: number): boolean[] {
    return Array(5).fill(false).map((_, index) => index < nivel);
  }

  // --- NOVAS FUNÇÕES DO MODAL E AÇÕES ---
  abrirModal(modo: 'visualizar' | 'excluir'): void {
    const idPrimeiraSelecionada = Array.from(this.questoesSelecionadas)[0];
    this.questaoFoco = this.questoes.find(q => q.id === idPrimeiraSelecionada) || null;
    this.modoModal = modo;
    this.modalAberto = true;
  }

  fecharModal(): void {
    this.modalAberto = false;
    this.questaoFoco = null;
  }

  confirmarExclusao(): void {
    // Remove as questões que estavam no Set
    this.questoes = this.questoes.filter(q => !this.questoesSelecionadas.has(q.id));
    this.questoesSelecionadas.clear(); // Limpa a seleção
    this.fecharModal();
  }

  editarSelecionada(): void {
    const id = Array.from(this.questoesSelecionadas)[0];
    alert(`Redirecionando para edição da questão ID: ${id} (Integração futura com a tela Cadastrar Questão)`);
  }

  criarListaAPartirDaSelecao(): void {
    const ids = Array.from(this.questoesSelecionadas);
    // Navega enviando os IDs no 'state' da rota
    this.router.navigate(['/professor/criar-lista'], { state: { questoesPrevias: ids } });
  }
}
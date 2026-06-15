import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // Importação obrigatória para usar ngModel
import { Questao } from '../../../../core/models/questao.model';

@Component({
  selector: 'app-banco-questoes',
  standalone: true,
  imports: [CommonModule, FormsModule], // Adicionado FormsModule aqui
  templateUrl: './banco-questoes.html'
})
export class BancoQuestoesComponent implements OnInit {
  questoes: Questao[] = [];
  
  // Set para guardar os IDs das questões que o professor marcar no checkbox
  questoesSelecionadas: Set<string> = new Set();

  // Variáveis de estado amarradas aos inputs da tela
  termoBusca: string = '';
  filtroDisciplina: string = 'Todas';
  filtroMacroArea: string = 'Todas';
  filtroMicroArea: string = '';
  filtroDificuldade: string = 'Todas';

  ngOnInit(): void {
    // Esse array será substituído por um this.questaoService.getQuestoes().subscribe(...) no futuro
    this.questoes = [
      { id: '1', enunciado: 'Resolver a equação quadrática x² - 5x + 6 = 0', disciplina: 'Matemática', macroArea: 'Álgebra', microArea: 'Equações Quadráticas', dificuldade: 3 },
      { id: '2', enunciado: 'Calcular a área de um triângulo retângulo', disciplina: 'Matemática', macroArea: 'Geometria', microArea: 'Triângulos', dificuldade: 2 },
      { id: '3', enunciado: 'Explicar a Lei de Newton', disciplina: 'Física', macroArea: 'Mecânica', microArea: 'Dinâmica', dificuldade: 4 },
      { id: '4', enunciado: 'Balancear equação química de combustão', disciplina: 'Química', macroArea: 'Reações Químicas', microArea: 'Combustão', dificuldade: 3 }
    ];
  }

  // Extrai dinamicamente as opções únicas para os Dropdowns não ficarem hardcoded
  get disciplinasUnicas(): string[] {
    return [...new Set(this.questoes.map(q => q.disciplina))];
  }

  get macroAreasUnicas(): string[] {
    return [...new Set(this.questoes.map(q => q.macroArea))];
  }

  // O "Cérebro" da tela: Aplica todos os filtros em tempo real
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

  // Função disparada ao clicar no checkbox
  toggleSelecao(id: string): void {
    if (this.questoesSelecionadas.has(id)) {
      this.questoesSelecionadas.delete(id);
    } else {
      this.questoesSelecionadas.add(id);
    }
    console.log('Questões selecionadas para a futura lista:', Array.from(this.questoesSelecionadas));
  }

  getDificuldadeArray(nivel: number): boolean[] {
    return Array(5).fill(false).map((_, index) => index < nivel);
  }
}
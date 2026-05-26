import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { DadosGraficosProfessor } from '../../../core/models/dashboard.model';


@Injectable({
  providedIn: 'root'
})
export class ProfessorService {
  
  getEstatisticasDashboard(): Observable<DadosGraficosProfessor> {
    const mockGraficos: DadosGraficosProfessor = {
      // Adicionamos os valores dos cards aqui
      totalAlunos: 142,
      questoesCadastradas: 48,
      desempenhoMedio: 74,
      listasAtivas: 12,

      linhaLabels: ['Semana 1', 'Semana 2', 'Semana 3', 'Semana 4', 'Semana 5', 'Semana 6'],
      linhaSuaTurma: [65, 72, 68, 81, 90, 85],
      linhaMediaGeral: [70, 71, 70, 72, 74, 75],
      roscaLabels: ['Turma A', 'Turma B', 'Turma C'],
      roscaDados: [28, 32, 25]
    };
    
    return of(mockGraficos);
  }
}
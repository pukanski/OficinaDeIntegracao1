import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

interface ListaPendenteDTO {
  id: string;
  titulo: string;
  resolvidas: number;
  total: number;
  dificuldade: string; // <-- O contrato exige 'dificuldade'
}

interface DashboardAlunoDTO {
  mediaGeral: number;
  exerciciosResolvidos: number;
  totalExercicios: number;
  percentualFrequencia: number;
  melhorDisciplina: string;
  melhorDisciplinaAcertos: number;
  progressoSemanal: { semana: string; valor: number }[];
  desempenhoDisciplinas: { disciplina: string; valor: number }[];
  listasDisponiveis: ListaPendenteDTO[];
}

@Component({
  selector: 'app-dashboard-aluno',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard-aluno.html'
})
export class DashboardAlunoComponent implements OnInit {

  dadosDashboard: DashboardAlunoDTO = {
    mediaGeral: 82,
    exerciciosResolvidos: 24,
    totalExercicios: 30,
    percentualFrequencia: 95,
    melhorDisciplina: 'Biologia',
    melhorDisciplinaAcertos: 88,
    progressoSemanal: [
      { semana: 'Sem 1', valor: 40 },
      { semana: 'Sem 2', valor: 55 },
      { semana: 'Sem 3', valor: 60 },
      { semana: 'Sem 4', valor: 75 },
      { semana: 'Sem 5', valor: 85 },
      { semana: 'Sem 6', valor: 92 }
    ],
    desempenhoDisciplinas: [
      { disciplina: 'Matemática', valor: 85 },
      { disciplina: 'Física', valor: 78 },
      { disciplina: 'Química', valor: 82 },
      { disciplina: 'Biologia', valor: 88 },
      { disciplina: 'História', valor: 75 }
    ],
    listasDisponiveis: [
      // CORREÇÃO: Alterado de 'difficulty' para 'dificuldade' para alinhar com a interface
      { id: 'l1', titulo: 'Revisão - Equações Quadráticas', resolvidas: 12, total: 15, dificuldade: 'Média' },
      { id: 'l2', titulo: 'Leis de Newton e Atrito', resolvidas: 0, total: 20, dificuldade: 'Difícil' }
    ]
  };

  constructor(private router: Router) { }

  ngOnInit(): void {
    // Endereço futuro do consumo da API
  }

  iniciarLista(idLista: string): void {
    this.router.navigate(['/aluno/resolucao', idLista]);
  }
}
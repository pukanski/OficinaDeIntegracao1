import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { ProfessorService } from '../../services/professor.service';
import { DadosGraficosProfessor } from '../../../../core/models/dashboard.model'; // Importe o modelo

@Component({
  selector: 'app-dashboard-professor',
  standalone: true,
  imports: [CommonModule, BaseChartDirective],
  templateUrl: './dashboard-professor.html',
  styleUrls: ['./dashboard-professor.css']
})
export class DashboardProfessorComponent implements OnInit {
  
  // Variável para guardar os dados dos cards
  public dadosGerais?: DadosGraficosProfessor;

  public lineChartData: ChartConfiguration<'line'>['data'] = { labels: [], datasets: [] };
  public lineChartOptions: ChartOptions<'line'> = { responsive: true, maintainAspectRatio: false, plugins: { legend: { position: 'bottom' } } };

  public doughnutChartData: ChartConfiguration<'doughnut'>['data'] = { labels: [], datasets: [] };
  public doughnutChartOptions: ChartOptions<'doughnut'> = { responsive: true, maintainAspectRatio: false, plugins: { legend: { position: 'bottom' } } };

  constructor(private professorService: ProfessorService) {}

  ngOnInit(): void {
    this.carregarGraficos();
  }

  private carregarGraficos(): void {
    this.professorService.getEstatisticasDashboard().subscribe({
      next: (dados) => {
        // Salvamos todos os dados na variável para o HTML usar
        this.dadosGerais = dados;

        this.lineChartData = {
          labels: dados.linhaLabels,
          datasets: [
            {
              data: dados.linhaSuaTurma,
              label: 'Sua Turma',
              borderColor: '#3b82f6',
              backgroundColor: 'rgba(59, 130, 246, 0.5)',
              tension: 0.4,
              pointBackgroundColor: '#3b82f6'
            },
            {
              data: dados.linhaMediaGeral,
              label: 'Média Geral',
              borderColor: '#9ca3af',
              borderDash: [5, 5],
              pointRadius: 0,
              tension: 0.4
            }
          ]
        };

        this.doughnutChartData = {
          labels: dados.roscaLabels,
          datasets: [
            {
              data: dados.roscaDados,
              backgroundColor: ['#60a5fa', '#3b82f6', '#1e3a8a'],
              hoverBackgroundColor: ['#3b82f6', '#2563eb', '#1e40af']
            }
          ]
        };
      },
      error: (erro) => {
        console.error('Erro ao buscar dados do dashboard', erro);
      }
    });
  }
}
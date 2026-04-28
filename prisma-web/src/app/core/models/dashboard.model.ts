export interface DadosGraficosProfessor {
  // Dados dos Cards
  totalAlunos: number;
  questoesCadastradas: number;
  desempenhoMedio: number;
  listasAtivas: number;
  
  // Dados dos Gráficos
  linhaLabels: string[];
  linhaSuaTurma: number[];
  linhaMediaGeral: number[];
  roscaLabels: string[];
  roscaDados: number[];
}
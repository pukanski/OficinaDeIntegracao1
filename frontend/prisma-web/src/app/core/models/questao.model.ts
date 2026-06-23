export interface AlternativaDTO {
  id: number;
  letra: string;
  texto: string;
  correta: boolean;
}

export interface Questao {
  id: number;
  provaId?: number;
  provaDescricao?: string;
  enunciado: string;
  disciplina: string;   // macro-área
  materia?: string;     // micro-área
  dificuldade?: string; // "Muito Fácil" | "Fácil" | "Médio" | "Difícil" | "Muito Difícil"
  numero: number;
  alternativas: AlternativaDTO[];
}

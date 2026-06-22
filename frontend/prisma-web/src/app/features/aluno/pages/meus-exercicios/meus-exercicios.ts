import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

// DTO esperado da API C# (GET /api/aluno/listas-pendentes)
interface ListaPendenteDTO {
  id: string;
  titulo: string;
  materia: string;
  qtdQuestoes: number;
  prazo: string;
  status: 'Pendente' | 'Em Andamento';
}

@Component({
  selector: 'app-meus-exercicios',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './meus-exercicios.html'
})
export class MeusExerciciosComponent implements OnInit {
  // Mocks simulando o retorno filtrado do backend baseado no JWT do aluno
  listas: ListaPendenteDTO[] = [
    { id: 'lst_123', titulo: 'Revisão - Equações Quadráticas', materia: 'Matemática', qtdQuestoes: 3, prazo: '2026-06-20', status: 'Pendente' },
    { id: 'lst_124', titulo: 'Leis de Newton', materia: 'Física', qtdQuestoes: 10, prazo: '2026-06-22', status: 'Em Andamento' }
  ];

  constructor(private router: Router) { }

  ngOnInit(): void { }

  iniciarResolucao(idLista: string): void {
    // Navega para a tela de resolução passando o ID na URL
    this.router.navigate(['/aluno/resolucao', idLista]);
  }
}
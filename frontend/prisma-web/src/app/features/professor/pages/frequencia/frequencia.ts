import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms'; // <-- Adicionado FormsModule

interface TurmaMock { id: string; nome: string; }
interface AlunoFrequencia { id: string; nome: string; status: 'Presente' | 'Ausente' | null; }

// --- NOVA INTERFACE PARA O HISTÓRICO CONECTADO AO BACK-END ---
interface HistoricoFrequencia {
  id: string;
  turmaId: string;
  turmaNome: string;
  data: string;
  presentes: number;
  ausentes: number;
}

@Component({
  selector: 'app-frequencia',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule], // <-- Adicionado FormsModule aqui
  templateUrl: './frequencia.html'
})
export class FrequenciaComponent implements OnInit {
  frequenciaForm!: FormGroup;
  abaAtual: 'registrar' | 'historico' = 'registrar';

  // Filtro de estado para a tabela de histórico
  filtroTurmaHistorico: string = 'Todas';

  turmas: TurmaMock[] = [
    { id: 't1', nome: 'Turma A - Matemática (28 alunos)' },
    { id: 't2', nome: 'Turma B - Física (32 alunos)' }
  ];

  alunos: AlunoFrequencia[] = [
    { id: 'a1', nome: 'Ana Silva', status: null },
    { id: 'a2', nome: 'Bruno Santos', status: null },
    { id: 'a3', nome: 'Carlos Oliveira', status: null },
    { id: 'a4', nome: 'Diana Costa', status: null },
    { id: 'a5', nome: 'Eduardo Rocha', status: null }
  ];

  // --- ARRAYS DE MOCK ESTRUTURADOS IGUAL AO RETORNO DA API C# ---
  historicoChamadas: HistoricoFrequencia[] = [
    { id: 'h1', turmaId: 't1', turmaNome: 'Turma A - Matemática', data: '2026-06-15', presentes: 25, ausentes: 3 },
    { id: 'h2', turmaId: 't1', turmaNome: 'Turma A - Matemática', data: '2026-06-08', presentes: 24, ausentes: 4 },
    { id: 'h3', turmaId: 't2', turmaNome: 'Turma B - Física', data: '2026-06-10', presentes: 30, ausentes: 2 }
  ];

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    const hoje = new Date().toISOString().split('T')[0];
    this.frequenciaForm = this.fb.group({
      turmaId: ['', Validators.required],
      dataChamada: [hoje, Validators.required]
    });
  }

  get presentes(): number { return this.alunos.filter(a => a.status === 'Presente').length; }
  get ausentes(): number { return this.alunos.filter(a => a.status === 'Ausente').length; }
  get naoMarcados(): number { return this.alunos.filter(a => a.status === null).length; }

  // --- FILTRO REATIVO DO HISTÓRICO ---
  get historicoFiltrado(): HistoricoFrequencia[] {
    if (this.filtroTurmaHistorico === 'Todas') {
      return this.historicoChamadas;
    }
    return this.historicoChamadas.filter(h => h.turmaId === this.filtroTurmaHistorico);
  }

  marcarStatus(alunoId: string, status: 'Presente' | 'Ausente'): void {
    const aluno = this.alunos.find(a => a.id === alunoId);
    if (aluno) {
      aluno.status = aluno.status === status ? null : status;
    }
  }

  marcarTodosPresentes(): void { this.alunos.forEach(a => a.status = 'Presente'); }
  limparMarcacoes(): void { this.alunos.forEach(a => a.status = null); }

  // --- GATILHO PARA CARREGAR DETALHES DE UMA CHAMADA PASSADA ---
  visualizarDetalhesHistorico(id: string): void {
    console.log('Solicitando ao back-end o espelho completo da chamada ID:', id);
    alert(`Preparado pro Back: Isso disparará um GET /api/frequencia/${id} para carregar a lista completa de alunos daquele dia para auditoria.`);
  }

  salvarChamada(): void {
    if (this.frequenciaForm.invalid) {
      alert('Selecione uma turma e uma data antes de salvar.');
      return;
    }
    if (this.naoMarcados > 0) {
      const confirma = confirm(`Existem ${this.naoMarcados} alunos não marcados. Deseja salvar mesmo assim?`);
      if (!confirma) return;
    }

    const payload = {
      turmaId: this.frequenciaForm.value.turmaId,
      data: this.frequenciaForm.value.dataChamada,
      registros: this.alunos.map(a => ({ alunoId: a.id, status: a.status }))
    };

    console.log('Payload pronto para o C#:', payload);
    alert('Chamada salva com sucesso! Verifique o console.');
    this.limparMarcacoes();
  }
}
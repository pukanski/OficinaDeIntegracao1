import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

interface Turma { id: number; nome: string; ano: string; turno: string; alunosIds: number[]; }
interface Aluno { id: number; primeiroNome: string; ultimoNome: string; ra: string; }
interface AlunoFrequencia { id: number; nome: string; ra: string; status: boolean | null; }
interface HistoricoItem { turmaId: number; data: string; presentes: number; ausentes: number; }

@Component({
  selector: 'app-frequencia',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './frequencia.html'
})
export class FrequenciaComponent implements OnInit {
  frequenciaForm!: FormGroup;
  abaAtual: 'registrar' | 'historico' = 'registrar';

  alterarAba(aba: 'registrar' | 'historico'): void {
    this.abaAtual = aba;
    if (aba === 'historico') this.carregarTodoHistorico();
  }

  carregarTodoHistorico(): void {
    this.historico = [];
    this.cdr.detectChanges();

    const carregarDeTodasTurmas = (turmas: Turma[]) => {
      if (turmas.length === 0) return;
      Promise.all(
        turmas.map(t =>
          this.http.get<HistoricoItem[]>(`${this.api}/api/Frequencia/historico/${t.id}`)
            .toPromise().catch(() => [] as HistoricoItem[])
        )
      ).then(resultados => {
        const todos = (resultados as HistoricoItem[][]).flat()
          .sort((a, b) => new Date(b.data).getTime() - new Date(a.data).getTime());
        this.zone.run(() => { this.historico = todos; this.cdr.detectChanges(); });
      });
    };

    if (this.turmas.length > 0) {
      carregarDeTodasTurmas(this.turmas);
    } else {
      // Turmas ainda não carregadas — busca primeiro
      this.http.get<Turma[]>(`${this.api}/api/Turma`).subscribe({
        next: (turmas) => {
          this.zone.run(() => { this.turmas = turmas; });
          carregarDeTodasTurmas(turmas);
        }
      });
    }
  }

  turmas: Turma[] = [];
  todosAlunos: Aluno[] = [];
  alunos: AlunoFrequencia[] = [];
  historico: HistoricoItem[] = [];
  filtroTurmaHistorico = 'Todas';

  carregandoAlunos = false;
  salvando = false;
  mensagem = '';
  erro = '';

  private api = environment.gatewayUrl;

  constructor(private fb: FormBuilder, private http: HttpClient, private cdr: ChangeDetectorRef, private zone: NgZone) {}

  ngOnInit(): void {
    const hoje = new Date().toISOString().split('T')[0];
    this.frequenciaForm = this.fb.group({
      turmaId: ['', Validators.required],
      dataChamada: [hoje, Validators.required]
    });

    this.carregarTurmas();
    this.carregarTodosAlunos();

    // Recarrega alunos e estado da chamada quando turma ou data mudam
    this.frequenciaForm.valueChanges.subscribe(() => {
      const turmaId = this.frequenciaForm.get('turmaId')?.value;
      const data = this.frequenciaForm.get('dataChamada')?.value;
      if (turmaId && data) {
        this.carregarAlunosDaTurma(+turmaId, data);
        this.carregarHistorico(+turmaId);
      }
      this.cdr.detectChanges();
    });
  }

  carregarTurmas(): void {
    this.http.get<Turma[]>(`${this.api}/api/Turma`).subscribe({
      next: (data) => { this.turmas = data; this.cdr.detectChanges(); },
      error: () => { this.turmas = []; }
    });
  }

  carregarTodosAlunos(): void {
    this.http.get<Aluno[]>(`${this.api}/api/Aluno/alunos`).subscribe({
      next: (data) => { this.todosAlunos = data; this.cdr.detectChanges(); },
      error: () => { this.todosAlunos = []; }
    });
  }

  carregarAlunosDaTurma(turmaId: number, data: string): void {
    this.carregandoAlunos = true;
    const turma = this.turmas.find(t => t.id === turmaId);
    const ids = turma?.alunosIds || [];

    // Monta a lista base de alunos
    const alunosBase = this.todosAlunos
      .filter(a => ids.includes(a.id))
      .map(a => ({ id: a.id, nome: `${a.primeiroNome} ${a.ultimoNome}`, ra: a.ra, status: null as boolean | null }));

    // Tenta carregar chamada já existente para essa turma/data
    const dataFormatada = data; // já está em yyyy-MM-dd
    this.http.get<any>(`${this.api}/api/Frequencia/chamada/${turmaId}/${dataFormatada}`).subscribe({
      next: (chamada) => {
        if (chamada?.registros?.length > 0) {
          // Pré-preenche o status de cada aluno com o que já foi salvo
          this.alunos = alunosBase.map(a => {
            const reg = chamada.registros.find((r: any) => r.alunoId === a.id);
            return { ...a, status: reg ? reg.presente : null };
          });
        } else {
          this.alunos = alunosBase;
        }
        this.carregandoAlunos = false;
        this.cdr.detectChanges();
      },
      error: () => {
        // Se não encontrou chamada, apenas mostra alunos sem status
        this.alunos = alunosBase;
        this.carregandoAlunos = false;
        this.cdr.detectChanges();
      }
    });
  }

  carregarHistorico(turmaId: number): void {
    this.http.get<HistoricoItem[]>(`${this.api}/api/Frequencia/historico/${turmaId}`).subscribe({
      next: (data) => { this.historico = data; this.cdr.detectChanges(); },
      error: () => { this.historico = []; }
    });
  }

  get presentes(): number { return this.alunos.filter(a => a.status === true).length; }
  get ausentes(): number { return this.alunos.filter(a => a.status === false).length; }
  get naoMarcados(): number { return this.alunos.filter(a => a.status === null).length; }

  get historicoFiltrado(): HistoricoItem[] {
    if (this.filtroTurmaHistorico === 'Todas') return this.historico;
    return this.historico.filter(h => String(h.turmaId) === String(this.filtroTurmaHistorico));
  }

  marcarStatus(alunoId: number, status: boolean): void {
    const aluno = this.alunos.find(a => a.id === alunoId);
    if (aluno) {
      aluno.status = aluno.status === status ? null : status;
      this.cdr.detectChanges();
    }
  }

  marcarTodosPresentes(): void {
    this.alunos.forEach(a => a.status = true);
    this.cdr.detectChanges();
  }

  limparMarcacoes(): void {
    this.alunos.forEach(a => a.status = null);
    this.cdr.detectChanges();
  }

  salvarChamada(): void {
    if (this.frequenciaForm.invalid) {
      this.frequenciaForm.markAllAsTouched();
      return;
    }
    if (this.naoMarcados > 0) {
      if (!confirm(`${this.naoMarcados} aluno(s) não marcados. Deseja salvar mesmo assim? Eles serão registrados como ausentes.`)) return;
      // Marca não marcados como ausentes
      this.alunos.filter(a => a.status === null).forEach(a => a.status = false);
    }

    this.salvando = true;
    this.erro = '';
    this.mensagem = '';

    const turmaId = +this.frequenciaForm.value.turmaId;
    const payload = {
      turmaId,
      data: this.frequenciaForm.value.dataChamada,
      registros: this.alunos.map(a => ({ alunoId: a.id, presente: a.status ?? false }))
    };

    this.http.post(`${this.api}/api/Frequencia/chamada`, payload).subscribe({
      next: () => {
        this.mensagem = 'Chamada salva com sucesso!';
        this.salvando = false;
        this.limparMarcacoes();
        this.carregarHistorico(turmaId);
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.erro = err.error?.message || 'Erro ao salvar chamada.';
        this.salvando = false;
        this.cdr.detectChanges();
      }
    });
  }

  formatarData(data: string): string {
    return data.split('-').reverse().join('/');
  }
}

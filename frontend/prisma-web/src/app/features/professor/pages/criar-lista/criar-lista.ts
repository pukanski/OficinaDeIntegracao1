import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { Questao } from '../../../../core/models/questao.model';

interface Turma { id: number; nome: string; ano: string; turno: string; qtdAlunos: number; }

@Component({
  selector: 'app-criar-lista',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './criar-lista.html'
})
export class CriarListaComponent implements OnInit {
  listaForm!: FormGroup;

  turmas: Turma[] = [];
  questoesSelecionadas: Questao[] = [];

  professorId: number | null = null;
  carregandoTurmas = false;
  salvando = false;
  mensagem = '';
  erro = '';

  private gatewayUrl = environment.gatewayUrl;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.listaForm = this.fb.group({
      titulo: ['', [Validators.required, Validators.minLength(3)]],
      dataVencimento: [''],
      turmasIds: this.fb.array([])
    });

    // IDs vindos do banco de questões via navegação
    const navState = history.state;
    const idsNavegacao = (navState?.questoesPrevias as number[]) || [];

    // Rascunho salvo antes de ir ao banco de questões
    const rascunhoRaw = sessionStorage.getItem('criar_lista_rascunho');
    if (rascunhoRaw) {
      const rascunho = JSON.parse(rascunhoRaw);
      sessionStorage.removeItem('criar_lista_rascunho');

      // Restaura título e data
      this.listaForm.patchValue({
        titulo: rascunho.titulo || '',
        dataVencimento: rascunho.dataVencimento || ''
      });

      // Restaura turmas selecionadas
      (rascunho.turmasIds || []).forEach((id: number) =>
        this.turmasFormArray.push(new FormControl(id))
      );

      // Mescla IDs do rascunho com os novos vindos do banco
      const idsMesclados = [...new Set([...(rascunho.questoesIds || []), ...idsNavegacao])] as number[];
      if (idsMesclados.length) this._carregarQuestoesPorIds(idsMesclados);

    } else if (idsNavegacao.length) {
      this._carregarQuestoesPorIds(idsNavegacao);
    }

    this.carregarTurmas();
    this.carregarProfessorId();
  }

  get turmasFormArray() {
    return this.listaForm.get('turmasIds') as FormArray;
  }

  carregarTurmas(): void {
    this.carregandoTurmas = true;
    this.http.get<Turma[]>(`${this.gatewayUrl}/api/Turma`).subscribe({
      next: (data) => {
        this.turmas = data;
        this.carregandoTurmas = false;
        this.cdr.detectChanges();
      },
      error: () => { this.turmas = []; this.carregandoTurmas = false; this.cdr.detectChanges(); }
    });
  }

  carregarProfessorId(): void {
    this.http.get<any>(`${this.gatewayUrl}/api/Professor/me`).subscribe({
      next: (prof) => { this.professorId = prof.id; },
      error: () => { this.professorId = null; }
    });
  }

  private _carregarQuestoesPorIds(ids: number[]): void {
    ids.forEach(id => {
      this.http.get<Questao>(`${this.gatewayUrl}/api/Questao/Questoes/${id}`).subscribe({
        next: (q) => {
          if (!this.questoesSelecionadas.find(x => x.id === q.id)) {
            this.questoesSelecionadas = [...this.questoesSelecionadas, q];
            this.cdr.detectChanges();
          }
        }
      });
    });
  }

  toggleTurma(e: any): void {
    const val = +e.target.value;
    if (e.target.checked) {
      this.turmasFormArray.push(new FormControl(val));
    } else {
      const idx = this.turmasFormArray.controls.findIndex(c => c.value === val);
      if (idx >= 0) this.turmasFormArray.removeAt(idx);
    }
  }

  isTurmaSelecionada(id: number): boolean {
    return this.turmasFormArray.value.includes(id);
  }

  removerQuestao(id: number): void {
    this.questoesSelecionadas = this.questoesSelecionadas.filter(q => q.id !== id);
  }

  adicionarMaisQuestoes(): void {
    // Salva o estado atual antes de navegar
    sessionStorage.setItem('criar_lista_rascunho', JSON.stringify({
      titulo: this.listaForm.get('titulo')?.value,
      dataVencimento: this.listaForm.get('dataVencimento')?.value,
      turmasIds: this.turmasFormArray.value,
      questoesIds: this.questoesSelecionadas.map(q => q.id)
    }));
    this.router.navigate(['/professor/banco-questoes']);
  }

  gerarLista(): void {
    this.mensagem = '';
    this.erro = '';

    if (this.listaForm.invalid) {
      this.listaForm.markAllAsTouched();
      return;
    }
    if (this.questoesSelecionadas.length === 0) {
      this.erro = 'Adicione pelo menos uma questão antes de gerar a lista.';
      return;
    }
    if (!this.professorId) {
      this.erro = 'Não foi possível identificar o professor. Tente fazer login novamente.';
      return;
    }

    this.salvando = true;
    const form = this.listaForm.value;

    const payload = {
      titulo: form.titulo,
      professorId: this.professorId,
      dataVencimento: form.dataVencimento || null,
      questoesIds: this.questoesSelecionadas.map(q => q.id),
      turmasIds: form.turmasIds
    };

    this.http.post(`${this.gatewayUrl}/api/Lista`, payload).subscribe({
      next: () => {
        this.mensagem = 'Lista criada com sucesso!';
        sessionStorage.removeItem('criar_lista_rascunho');
        this.salvando = false;
        this.listaForm.reset();
        this.turmasFormArray.clear();
        this.questoesSelecionadas = [];
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.erro = err.error?.message || 'Erro ao criar lista.';
        this.salvando = false;
      }
    });
  }
}

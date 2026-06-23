import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { environment } from '../../../../../environments/environment';

interface ProvaDTO { id: number; vestibular: string; ano: number; edicao?: string; }

@Component({
  selector: 'app-cadastrar-questao',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './cadastrar-questao.html'
})
export class CadastrarQuestaoComponent implements OnInit {
  questaoForm!: FormGroup;
  provaForm!: FormGroup;

  provas: ProvaDTO[] = [];
  buscaProva = '';
  provasFiltradas: ProvaDTO[] = [];
  mostrarSugestoes = false;
  provaSelecionada: ProvaDTO | null = null;
  isGerandoIA = false;
  iaSugeriu = false;
  criandoProva = false;
  carregandoProvas = false;
  salvandoProva = false;
  mensagem = '';
  erro = '';

  macroAreasFixas: string[] = [
    'Matemática', 'Química', 'Física', 'Biologia', 'História',
    'Geografia', 'Linguagens', 'Filosofia', 'Sociologia'
  ];

  private apiQuestao = `${environment.gatewayUrl}/api/Questao`;

  constructor(private fb: FormBuilder, private http: HttpClient) { }

  ngOnInit(): void {
    this.questaoForm = this.fb.group({
      provaId: [''],  // opcional — questão avulsa se vazio
      numero: [1, [Validators.required, Validators.min(1)]],
      enunciado: ['', Validators.required],
      alternativaA: ['', Validators.required],
      alternativaB: ['', Validators.required],
      alternativaC: ['', Validators.required],
      alternativaD: ['', Validators.required],
      alternativaE: ['', Validators.required],
      gabarito: ['A', Validators.required],
      macroArea: ['', Validators.required],
      microArea: [''],
      dificuldade: ['', Validators.required]
    });

    this.provaForm = this.fb.group({
      vestibular: ['', Validators.required],
      ano: [new Date().getFullYear(), [Validators.required, Validators.min(1900), Validators.max(2100)]],
      edicao: ['']
    });

    this.carregarProvas();
  }

  carregarProvas(): void {
    this.carregandoProvas = true;
    this.http.get<ProvaDTO[]>(`${this.apiQuestao}/Provas`).subscribe({
      next: (provas) => {
        this.provas = provas;
        this.provasFiltradas = provas;
        this.carregandoProvas = false;
      },
      error: () => {
        this.provas = [];
        this.provasFiltradas = [];
        this.carregandoProvas = false;
      }
    });
  }

  filtrarProvas(): void {
    const busca = this.buscaProva.toLowerCase();
    this.provasFiltradas = this.provas.filter(p =>
      `${p.vestibular} ${p.ano} ${p.edicao || ''}`.toLowerCase().includes(busca)
    );
    this.mostrarSugestoes = true;
  }

  selecionarProva(prova: ProvaDTO | null): void {
    this.provaSelecionada = prova;
    this.buscaProva = prova ? `${prova.vestibular} ${prova.ano}${prova.edicao ? ' — ' + prova.edicao : ''}` : '';
    this.questaoForm.patchValue({ provaId: prova ? prova.id : '' });
    this.mostrarSugestoes = false;
  }

  limparProva(): void {
    this.provaSelecionada = null;
    this.buscaProva = '';
    this.questaoForm.patchValue({ provaId: '' });
    this.provasFiltradas = this.provas;
  }

  get provaLabel(): string {
    const id = this.questaoForm.get('provaId')?.value;
    const prova = this.provas.find(p => p.id === +id);
    if (!prova) return '';
    return `${prova.vestibular} ${prova.ano}${prova.edicao ? ' - ' + prova.edicao : ''}`;
  }

  salvarProva(): void {
    if (this.provaForm.invalid) {
      this.provaForm.markAllAsTouched();
      return;
    }
    this.salvandoProva = true;
    const payload = {
      vestibular: this.provaForm.value.vestibular,
      ano: +this.provaForm.value.ano,
      edicao: this.provaForm.value.edicao || null
    };
    this.http.post<ProvaDTO>(`${this.apiQuestao}/Provas`, payload).subscribe({
      next: (prova) => {
        this.provas.push(prova);
        this.provasFiltradas = [...this.provas];
        this.selecionarProva(prova);
        this.provaForm.reset({ ano: new Date().getFullYear() });
        this.criandoProva = false;
        this.salvandoProva = false;
      },
      error: (err) => {
        this.erro = err.error?.erro || 'Erro ao criar prova.';
        this.salvandoProva = false;
      }
    });
  }

  gerarSugestoesIA(): void {
    if (this.questaoForm.get('enunciado')?.invalid) {
      alert('Preencha o enunciado antes de pedir sugestões à IA.');
      return;
    }
    this.isGerandoIA = true;
    this.iaSugeriu = false;

    const payload = {
      statement: this.questaoForm.get('enunciado')?.value,
      available_disciplines: this.macroAreasFixas,
      available_subjects: {
        'Matemática': ['Álgebra', 'Geometria Plana', 'Geometria Espacial', 'Trigonometria', 'Funções', 'Estatística', 'Probabilidade', 'Aritmética'],
        'Física': ['Mecânica', 'Termodinâmica', 'Óptica', 'Eletromagnetismo', 'Ondulatória', 'Cinemática', 'Dinâmica'],
        'Química': ['Química Orgânica', 'Química Inorgânica', 'Físico-Química', 'Reações Químicas', 'Estequiometria', 'Termoquímica'],
        'Biologia': ['Citologia', 'Genética', 'Ecologia', 'Evolução', 'Fisiologia', 'Botânica', 'Zoologia'],
        'História': ['História do Brasil', 'História Geral', 'Segunda Guerra Mundial', 'Revolução Industrial', 'Idade Média', 'Antiguidade'],
        'Geografia': ['Geografia Física', 'Geografia Humana', 'Geopolítica', 'Climatologia', 'Cartografia', 'Urbanização'],
        'Linguagens': ['Interpretação de Texto', 'Gramática', 'Literatura', 'Redação', 'Língua Estrangeira'],
        'Filosofia': ['Filosofia Antiga', 'Filosofia Moderna', 'Ética', 'Lógica', 'Epistemologia'],
        'Sociologia': ['Sociologia Clássica', 'Movimentos Sociais', 'Cultura', 'Política', 'Estratificação Social']
      }
    };

    this.http.post<any>(`${environment.gatewayUrl}/api/ia/classify`, payload).subscribe({
      next: (res) => {
        this.isGerandoIA = false;
        this.iaSugeriu = true;
        this.questaoForm.patchValue({
          macroArea: res.discipline || '',
          microArea: res.subject || '',
          dificuldade: res.difficulty || ''
        });
      },
      error: () => {
        this.isGerandoIA = false;
        this.erro = 'Serviço de IA indisponível. Classifique manualmente.';
      }
    });
  }

  salvarQuestao(): void {
    if (this.questaoForm.invalid) {
      this.questaoForm.markAllAsTouched();
      return;
    }
    this.mensagem = '';
    this.erro = '';
    const form = this.questaoForm.getRawValue();
    const payload = {
      provaId: form.provaId ? +form.provaId : null,
      numero: +form.numero,
      disciplina: form.macroArea,
      materia: form.microArea || null,
      enunciado: form.enunciado,
      dificuldade: form.dificuldade,
      alternativas: [
        { letra: 'A', texto: form.alternativaA, correta: form.gabarito === 'A', questaoId: 0 },
        { letra: 'B', texto: form.alternativaB, correta: form.gabarito === 'B', questaoId: 0 },
        { letra: 'C', texto: form.alternativaC, correta: form.gabarito === 'C', questaoId: 0 },
        { letra: 'D', texto: form.alternativaD, correta: form.gabarito === 'D', questaoId: 0 },
        { letra: 'E', texto: form.alternativaE, correta: form.gabarito === 'E', questaoId: 0 }
      ]
    };
    this.http.post(`${this.apiQuestao}/Questoes`, payload).subscribe({
      next: () => {
        this.mensagem = 'Questão salva com sucesso!';
        this.questaoForm.patchValue({ enunciado: '', alternativaA: '', alternativaB: '', alternativaC: '', alternativaD: '', alternativaE: '', gabarito: 'A', macroArea: '', microArea: '', dificuldade: '', numero: +form.numero + 1 });
        this.iaSugeriu = false;
      },
      error: (err) => {
        this.erro = err.error?.erro || 'Erro ao salvar a questão.';
      }
    });
  }
}

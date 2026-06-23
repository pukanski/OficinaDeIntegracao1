import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Questao } from '../../../../core/models/questao.model';
import { environment } from '../../../../../environments/environment';

const DIFICULDADE_ORDEM: Record<string, number> = {
  'Muito Fácil': 1, 'Fácil': 2, 'Médio': 3, 'Difícil': 4, 'Muito Difícil': 5
};

@Component({
  selector: 'app-banco-questoes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './banco-questoes.html'
})
export class BancoQuestoesComponent implements OnInit {
  questoes: Questao[] = [];
  questoesSelecionadas: Set<number> = new Set();
  idsNoRascunho: Set<number> = new Set();

  termoBusca = '';
  filtroDisciplina = 'Todas';
  filtroMicroArea = '';
  filtroDificuldade = 'Todas';
  filtroProva = 'Todas';

  // Autocomplete de prova
  buscaProvaFiltro = '';
  mostrarSugestoesProva = false;

  // Modal de edição
  modalEdicaoAberto = false;
  questaoEditando: any = null;
  salvandoEdicao = false;
  macroAreasFixas = ['Matemática','Química','Física','Biologia','História','Geografia','Linguagens','Filosofia','Sociologia'];
  dificuldadesFixas = ['Muito Fácil','Fácil','Médio','Difícil','Muito Difícil'];

  modalAberto = false;
  modoModal: 'visualizar' | 'excluir' = 'visualizar';
  questaoFoco: Questao | null = null;

  carregando = false;
  erro = '';

  private apiUrl = `${environment.gatewayUrl}/api/Questao/Questoes`;

  constructor(private router: Router, private http: HttpClient, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    // Carrega IDs que já estão no rascunho da lista
    const rascunhoRaw = sessionStorage.getItem('criar_lista_rascunho');
    if (rascunhoRaw) {
      const rascunho = JSON.parse(rascunhoRaw);
      this.idsNoRascunho = new Set<number>(rascunho.questoesIds || []);
    }
    this.carregarQuestoes();
  }

  carregarQuestoes(): void {
    this.carregando = true;
    this.erro = '';
    this.http.get<Questao[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.questoes = data;
        this.carregando = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.erro = 'Erro ao carregar questões.';
        this.carregando = false;
        this.cdr.detectChanges();
      }
    });
  }

  get disciplinasUnicas(): string[] {
    return [...new Set(this.questoes.map(q => q.disciplina).filter(Boolean))];
  }

  get provasUnicas(): string[] {
    return [...new Set(
      this.questoes
        .map(q => q.provaDescricao)
        .filter((p): p is string => !!p)
    )];
  }

  get provasFiltroSugestoes(): string[] {
    const busca = this.buscaProvaFiltro.toLowerCase();
    return this.provasUnicas.filter(p => p.toLowerCase().includes(busca));
  }

  selecionarProvaFiltro(prova: string): void {
    this.filtroProva = prova;
    this.buscaProvaFiltro = prova;
    this.mostrarSugestoesProva = false;
    this.cdr.detectChanges();
  }

  limparFiltroProva(): void {
    this.filtroProva = 'Todas';
    this.buscaProvaFiltro = '';
    this.cdr.detectChanges();
  }

  abrirEdicao(): void {
    const id = Array.from(this.questoesSelecionadas)[0];
    this.questaoEditando = JSON.parse(JSON.stringify(this.questoes.find(q => q.id === id)));
    this.modalEdicaoAberto = true;
    this.cdr.detectChanges();
  }

  fecharEdicao(): void {
    this.modalEdicaoAberto = false;
    this.questaoEditando = null;
    this.cdr.detectChanges();
  }

  salvarEdicao(): void {
    if (!this.questaoEditando) return;
    this.salvandoEdicao = true;
    const payload = {
      provaId: this.questaoEditando.provaId || null,
      numero: this.questaoEditando.numero || 1,
      disciplina: this.questaoEditando.disciplina,
      materia: this.questaoEditando.materia || null,
      enunciado: this.questaoEditando.enunciado,
      dificuldade: this.questaoEditando.dificuldade || null,
      alternativas: this.questaoEditando.alternativas
    };
    this.http.put(`${this.apiUrl}/${this.questaoEditando.id}`, payload).subscribe({
      next: () => {
        const idx = this.questoes.findIndex(q => q.id === this.questaoEditando.id);
        if (idx >= 0) this.questoes[idx] = { ...this.questoes[idx], ...this.questaoEditando };
        this.questoesSelecionadas.clear();
        this.salvandoEdicao = false;
        this.fecharEdicao();
        this.cdr.detectChanges();
      },
      error: () => { this.salvandoEdicao = false; this.cdr.detectChanges(); }
    });
  }

  get questoesFiltradas(): Questao[] {
    return this.questoes.filter(q => {
      const matchBusca = !this.termoBusca ||
        q.enunciado.toLowerCase().includes(this.termoBusca.toLowerCase());
      const matchDisciplina = this.filtroDisciplina === 'Todas' ||
        q.disciplina === this.filtroDisciplina;
      const matchMicro = !this.filtroMicroArea ||
        (q.materia || '').toLowerCase().includes(this.filtroMicroArea.toLowerCase());
      const matchDificuldade = this.filtroDificuldade === 'Todas' ||
        q.dificuldade === this.filtroDificuldade;
      const matchProva = !this.buscaProvaFiltro ||
        (this.buscaProvaFiltro === 'Avulsa' ? !q.provaDescricao :
        (q.provaDescricao || '').toLowerCase().includes(this.buscaProvaFiltro.toLowerCase()));
      return matchBusca && matchDisciplina && matchMicro && matchDificuldade && matchProva;
    });
  }

  getDificuldadeStars(dificuldade?: string): boolean[] {
    const nivel = DIFICULDADE_ORDEM[dificuldade || ''] || 0;
    return Array(5).fill(false).map((_, i) => i < nivel);
  }

  toggleSelecao(id: number): void {
    if (this.questoesSelecionadas.has(id)) {
      this.questoesSelecionadas.delete(id);
    } else {
      this.questoesSelecionadas.add(id);
    }
  }

  abrirModal(modo: 'visualizar' | 'excluir'): void {
    const id = Array.from(this.questoesSelecionadas)[0];
    this.questaoFoco = this.questoes.find(q => q.id === id) || null;
    this.modoModal = modo;
    this.modalAberto = true;
  }

  fecharModal(): void {
    this.modalAberto = false;
    this.questaoFoco = null;
  }

  confirmarExclusao(): void {
    const ids = Array.from(this.questoesSelecionadas);
    const deletes = ids.map(id =>
      this.http.delete(`${this.apiUrl}/${id}`).toPromise().catch(() => null)
    );
    Promise.all(deletes).then(() => {
      this.questoes = this.questoes.filter(q => !this.questoesSelecionadas.has(q.id));
      this.questoesSelecionadas.clear();
      this.fecharModal();
    });
  }

  editarSelecionada(): void {
    // TODO: navegar para edição
  }

  criarListaAPartirDaSelecao(): void {
    const ids = Array.from(this.questoesSelecionadas);
    this.router.navigate(['/professor/criar-lista'], { state: { questoesPrevias: ids } });
  }
}

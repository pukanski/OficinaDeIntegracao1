import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

interface LogEntry {
  id: number;
  tipo: string;
  descricao: string;
  detalhe: string;
  criadoEm: Date;
  atualizadoEm?: Date;
}

@Component({
  selector: 'app-logs',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './logs.html'
})
export class LogsComponent implements OnInit {
  abaAtual: 'usuarios' | 'questoes' | 'listas' | 'turmas' = 'usuarios';
  busca = '';
  carregando = false;
  logs: LogEntry[] = [];

  readonly abas: { id: 'usuarios' | 'questoes' | 'listas' | 'turmas'; label: string }[] = [
    { id: 'usuarios', label: '👤 Usuários' },
    { id: 'questoes', label: '📝 Questões' },
    { id: 'listas',   label: '📋 Listas' },
    { id: 'turmas',   label: '🏫 Turmas' }
  ];

  private api = environment.gatewayUrl;

  constructor(private http: HttpClient, private cdr: ChangeDetectorRef, private zone: NgZone) {}

  ngOnInit(): void { this.carregarAba(); }

  alterarAba(aba: 'usuarios' | 'questoes' | 'listas' | 'turmas'): void {
    this.abaAtual = aba;
    this.busca = '';
    this.logs = [];
    this.carregarAba();
  }

  carregarAba(): void {
    this.carregando = true;
    switch (this.abaAtual) {
      case 'usuarios': this.carregarUsuarios(); break;
      case 'questoes': this.carregarQuestoes(); break;
      case 'listas': this.carregarListas(); break;
      case 'turmas': this.carregarTurmas(); break;
    }
  }

  carregarUsuarios(): void {
    Promise.all([
      this.http.get<any>(`${this.api}/api/Admin/usuarios`).toPromise().catch(() => ({ professores: [], alunos: [] })),
      this.http.get<any[]>(`${this.api}/api/Aluno/alunos`).toPromise().catch(() => [])
    ]).then(([adminData, alunosDetalhados]) => {
      const entries: LogEntry[] = [];

      (adminData.professores || []).forEach((p: any) => entries.push({
        id: p.id, tipo: 'Professor',
        descricao: p.nome || p.email,
        detalhe: p.email,
        criadoEm: new Date(p.criadoEm || Date.now())
      }));

      (alunosDetalhados || []).forEach((a: any) => entries.push({
        id: a.id, tipo: 'Aluno',
        descricao: `${a.primeiroNome} ${a.ultimoNome}`,
        detalhe: a.email,
        criadoEm: new Date(a.criadoEm || Date.now()),
        atualizadoEm: a.atualizadoEm ? new Date(a.atualizadoEm) : undefined
      }));

      this.zone.run(() => {
        this.logs = entries.sort((a, b) => b.criadoEm.getTime() - a.criadoEm.getTime());
        this.carregando = false;
        this.cdr.detectChanges();
      });
    });
  }

  carregarQuestoes(): void {
    this.http.get<any[]>(`${this.api}/api/Questao/Questoes`).subscribe({
      next: (questoes) => {
        this.zone.run(() => {
          this.logs = questoes.map(q => ({
            id: q.id,
            tipo: q.disciplina || 'Questão',
            descricao: q.enunciado?.substring(0, 80) + (q.enunciado?.length > 80 ? '...' : ''),
            detalhe: `${q.materia || '—'} · ${q.dificuldade || 'Sem classificação'}`,
            criadoEm: new Date(q.criadoEm),
            atualizadoEm: q.atualizadoEm ? new Date(q.atualizadoEm) : undefined
          })).sort((a, b) => b.criadoEm.getTime() - a.criadoEm.getTime());
          this.carregando = false;
          this.cdr.detectChanges();
        });
      },
      error: () => this.zone.run(() => { this.carregando = false; this.cdr.detectChanges(); })
    });
  }

  carregarListas(): void {
    // Busca professores para depois buscar listas de cada um
    this.http.get<any>(`${this.api}/api/Admin/usuarios`).subscribe({
      next: (data) => {
        const professores: any[] = data.professores || [];
        if (professores.length === 0) {
          this.zone.run(() => { this.carregando = false; this.cdr.detectChanges(); });
          return;
        }
        const allListas: any[] = [];
        let pendentes = professores.length;
        professores.forEach(prof => {
          this.http.get<any[]>(`${this.api}/api/Lista/professor/${prof.id}`).subscribe({
            next: (listas) => {
              listas.forEach(l => allListas.push({ ...l, professorNome: prof.nome }));
              pendentes--;
              if (pendentes === 0) this._finalizarListas(allListas);
            },
            error: () => { pendentes--; if (pendentes === 0) this._finalizarListas(allListas); }
          });
        });
      },
      error: () => this.zone.run(() => { this.carregando = false; this.cdr.detectChanges(); })
    });
  }

  private _finalizarListas(listas: any[]): void {
    this.zone.run(() => {
      this.logs = listas.map(l => ({
        id: l.id,
        tipo: 'Lista',
        descricao: l.titulo,
        detalhe: `${l.questoesIds?.length || 0} questões · Prof. ${l.professorNome || l.professorId}`,
        criadoEm: new Date(l.criadoEm)
      })).sort((a, b) => b.criadoEm.getTime() - a.criadoEm.getTime());
      this.carregando = false;
      this.cdr.detectChanges();
    });
  }

  carregarTurmas(): void {
    this.http.get<any[]>(`${this.api}/api/Turma`).subscribe({
      next: (turmas) => {
        this.zone.run(() => {
          this.logs = turmas.map(t => ({
            id: t.id,
            tipo: 'Turma',
            descricao: t.nome,
            detalhe: `${t.ano} · ${t.turno} · ${t.qtdAlunos} alunos`,
            criadoEm: new Date(t.criadoEm),
            atualizadoEm: t.atualizadoEm ? new Date(t.atualizadoEm) : undefined
          })).sort((a, b) => b.criadoEm.getTime() - a.criadoEm.getTime());
          this.carregando = false;
          this.cdr.detectChanges();
        });
      },
      error: () => this.zone.run(() => { this.carregando = false; this.cdr.detectChanges(); })
    });
  }

  get logsFiltrados(): LogEntry[] {
    if (!this.busca) return this.logs;
    const b = this.busca.toLowerCase();
    return this.logs.filter(l =>
      l.descricao.toLowerCase().includes(b) ||
      l.detalhe.toLowerCase().includes(b) ||
      l.tipo.toLowerCase().includes(b)
    );
  }

  badgeColor(tipo: string): string {
    const map: Record<string, string> = {
      'Professor': 'bg-blue-50 text-blue-700',
      'Aluno': 'bg-green-50 text-green-700',
      'Lista': 'bg-purple-50 text-purple-700',
      'Turma': 'bg-orange-50 text-orange-700'
    };
    return map[tipo] || 'bg-slate-100 text-slate-600';
  }

  formatarData(d: Date): string {
    return d.toLocaleString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });
  }
}

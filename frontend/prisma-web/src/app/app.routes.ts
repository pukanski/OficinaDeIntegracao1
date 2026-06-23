import { Routes } from '@angular/router';
import { LayoutAdminComponent } from './core/components/layout-admin/layout-admin';
import { LayoutAlunoComponent } from './core/components/layout-aluno/layout-aluno';
import { LayoutComponent } from './core/components/layout/layout';
import { authGuard } from './core/guards/auth.guard';
import { PainelAdminComponent } from './features/admin/pages/painel-admin/painel-admin';
import { DashboardAlunoComponent } from './features/aluno/pages/dashboard-aluno/dashboard-aluno';
import { GabaritoListaComponent } from './features/aluno/pages/gabarito-lista/gabarito-lista';
import { BancoLivreComponent } from './features/aluno/pages/banco-livre/banco-livre';
import { MeusExerciciosComponent } from './features/aluno/pages/meus-exercicios/meus-exercicios';
import { ResolucaoExercicioComponent } from './features/aluno/pages/resolucao-exercicio/resolucao-exercicio';
import { LoginComponent } from './features/auth/pages/login/login';
import { BancoQuestoesComponent } from './features/professor/pages/banco-questoes/banco-questoes';
import { CadastrarQuestaoComponent } from './features/professor/pages/cadastrar-questao/cadastrar-questao';
import { CriarListaComponent } from './features/professor/pages/criar-lista/criar-lista';
import { DashboardProfessorComponent } from './features/professor/pages/dashboard-professor/dashboard-professor';
import { GerenciarTurmasComponent } from './features/professor/pages/gerenciar-turmas/gerenciar-turmas';
import { FrequenciaComponent } from './features/professor/pages/frequencia/frequencia';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },

  // Rotas do Professor (mantidas intactas)
  {
    path: 'professor',
    component: LayoutComponent,
    canActivate: [authGuard],
    data: { role: 'professor' },
    children: [
      { path: 'dashboard', component: DashboardProfessorComponent },
      { path: 'banco-questoes', component: BancoQuestoesComponent },
      { path: 'cadastrar-questao', component: CadastrarQuestaoComponent },
      { path: 'criar-lista', component: CriarListaComponent },
      { path: 'frequencia', component: FrequenciaComponent },
      { path: 'turmas', component: GerenciarTurmasComponent }
    ]
  },

  // Rotas do Aluno
  {
    path: 'aluno',
    component: LayoutAlunoComponent,
    canActivate: [authGuard],
    data: { role: 'aluno' },
    children: [
      { path: 'dashboard', component: DashboardAlunoComponent },
      { path: 'exercicios', component: MeusExerciciosComponent },
      { path: 'resolucao/:id', component: ResolucaoExercicioComponent },
      { path: 'gabarito/:id', component: GabaritoListaComponent },
      { path: 'banco', component: BancoLivreComponent }
    ]
  },

  // Rotas do Administrador
  {
    path: 'admin',
    component: LayoutAdminComponent,
    canActivate: [authGuard],
    data: { role: 'admin' },
    children: [
      { path: 'painel', component: PainelAdminComponent }
    ]
  }
];
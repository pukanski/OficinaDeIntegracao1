import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/pages/login/login';
import { LayoutComponent } from './core/components/layout/layout';
import { LayoutAlunoComponent } from './core/components/layout-aluno/layout-aluno';
import { DashboardProfessorComponent } from './features/professor/pages/dashboard-professor/dashboard-professor';
import { BancoQuestoesComponent } from './features/professor/pages/banco-questoes/banco-questoes';
import { CadastrarQuestaoComponent } from './features/professor/pages/cadastrar-questao/cadastrar-questao';
import { CriarListaComponent } from './features/professor/pages/criar-lista/criar-lista';
import { FrequenciaComponent } from './features/professor/pages/frequencia/frequencia';
import { DashboardAlunoComponent } from './features/aluno/pages/dashboard-aluno/dashboard-aluno';
import { MeusExerciciosComponent } from './features/aluno/pages/meus-exercicios/meus-exercicios';
import { ResolucaoExercicioComponent } from './features/aluno/pages/resolucao-exercicio/resolucao-exercicio';
import { GabaritoListaComponent } from './features/aluno/pages/gabarito-lista/gabarito-lista';
import { LayoutAdminComponent } from './core/components/layout-admin/layout-admin';
import { PainelAdminComponent } from './features/admin/pages/painel-admin/painel-admin';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },

  // Rotas do Professor (mantidas intactas)
  {
    path: 'professor',
    component: LayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardProfessorComponent },
      { path: 'banco-questoes', component: BancoQuestoesComponent },
      { path: 'cadastrar-questao', component: CadastrarQuestaoComponent },
      { path: 'criar-lista', component: CriarListaComponent },
      { path: 'frequencia', component: FrequenciaComponent }
    ]
  },

  // Rotas do Aluno
  {
    path: 'aluno',
    component: LayoutAlunoComponent,
    children: [
      { path: 'dashboard', component: DashboardAlunoComponent },
      { path: 'exercicios', component: MeusExerciciosComponent },
      { path: 'resolucao/:id', component: ResolucaoExercicioComponent },
      { path: 'gabarito/:id', component: GabaritoListaComponent }
    ]
  },

  // Rotas do Administrador
  {
    path: 'admin',
    component: LayoutAdminComponent,
    children: [
      { path: 'painel', component: PainelAdminComponent }
    ]
  }
];
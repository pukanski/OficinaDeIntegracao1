import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/pages/login/login';
import { LayoutComponent } from './core/components/layout/layout';
import { DashboardProfessorComponent } from './features/professor/pages/dashboard-professor/dashboard-professor';
import { BancoQuestoesComponent } from './features/professor/pages/banco-questoes/banco-questoes';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  {
    path: 'professor',
    component: LayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardProfessorComponent },
      { path: 'banco-questoes', component: BancoQuestoesComponent }
    ]
  }
];
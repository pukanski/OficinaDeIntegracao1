import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/pages/login/login';
import { DashboardProfessorComponent } from './features/professor/pages/dashboard-professor/dashboard-professor';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'dashboard-professor', component: DashboardProfessorComponent },
  { path: '', redirectTo: '/dashboard-professor', pathMatch: 'full' }, // Mudei aqui temporariamente
  { path: '**', redirectTo: '/dashboard-professor' } // Mudei aqui temporariamente
];
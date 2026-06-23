import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-layout-admin',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './layout-admin.html'
})
export class LayoutAdminComponent {
  nomeUsuario = 'Administrador';
  tipoUsuario = 'Admin';

  constructor(private authService: AuthService, private router: Router) {}

  sair(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

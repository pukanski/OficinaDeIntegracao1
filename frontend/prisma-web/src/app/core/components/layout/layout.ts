import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterModule], // Necessário para o router-outlet funcionar
  templateUrl: './layout.html'
})
export class LayoutComponent {
  constructor(private authService: AuthService, private router: Router) { }

  sair(): void {
    this.authService.logout(); // Limpa o localStorage e desloga do Supabase
    this.router.navigate(['/login']); // Chuta para a tela de login
  }
}
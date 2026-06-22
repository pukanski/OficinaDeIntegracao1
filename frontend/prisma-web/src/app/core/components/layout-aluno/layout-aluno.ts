import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-layout-aluno',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './layout-aluno.html'
})
export class LayoutAlunoComponent {
  constructor(private authService: AuthService, private router: Router) { }

  sair(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
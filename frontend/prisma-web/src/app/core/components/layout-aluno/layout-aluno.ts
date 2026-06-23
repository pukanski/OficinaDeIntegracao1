import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-layout-aluno',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './layout-aluno.html'
})
export class LayoutAlunoComponent implements OnInit {
  nomeUsuario = '';
  tipoUsuario = 'Aluno';

  constructor(private authService: AuthService, private router: Router, private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any>(`${environment.gatewayUrl}/api/Aluno/me`).subscribe({
      next: (a) => { this.nomeUsuario = `${a.primeiroNome} ${a.ultimoNome}`; }
    });
  }

  sair(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

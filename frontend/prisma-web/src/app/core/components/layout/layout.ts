import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './layout.html'
})
export class LayoutComponent implements OnInit {
  nomeUsuario = '';
  tipoUsuario = 'Professor';

  constructor(private authService: AuthService, private router: Router, private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any>(`${environment.gatewayUrl}/api/Professor/me`).subscribe({
      next: (p) => { this.nomeUsuario = `${p.primeiroNome} ${p.ultimoNome}`; }
    });
  }

  sair(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

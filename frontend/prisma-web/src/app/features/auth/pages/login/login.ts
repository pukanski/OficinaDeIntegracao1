import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  perfilSelecionado: 'aluno' | 'professor' | 'admin' = 'professor';

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    if (this.authService.restoreSession()) {
      const role = this.authService.role;
      if (role === 'admin') this.router.navigate(['/admin/painel']);
      else if (role === 'professor') this.router.navigate(['/professor/dashboard']);
      else if (role === 'aluno') this.router.navigate(['/aluno/dashboard']);
      return;
    }

    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  alterarPerfil(perfil: 'aluno' | 'professor' | 'admin'): void {
    this.perfilSelecionado = perfil;
    this.loginForm.reset();
  }

  async onSubmit(): Promise<void> {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const { email, senha } = this.loginForm.value;

    try {
      await this.authService.login(email, senha, this.perfilSelecionado);

      if (this.perfilSelecionado === 'admin') {
        this.router.navigate(['/admin/painel']);
        return;
      }

      const endpoint = this.perfilSelecionado === 'aluno' ? '/api/Aluno/me' : '/api/Professor/me';

      await firstValueFrom(this.http.get(`${environment.gatewayUrl}${endpoint}`));

      const rotaDash = this.perfilSelecionado === 'aluno' ? '/aluno/dashboard' : '/professor/dashboard';
      this.router.navigate([rotaDash]);

    } catch (error: any) {
      console.error('Falha no login ou validação de perfil:', error);
      this.authService.logout();

      if (error.message && error.message.includes('Acesso negado')) {
        alert(error.message);
      } else if (error.status === 404) {
        alert(`O usuário não está cadastrado como ${this.perfilSelecionado} no banco de dados.`);
      } else {
        alert('Credenciais rejeitadas pelo Supabase.');
      }
    }
  }
}
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService, UserRole } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  perfilSelecionado: UserRole = 'professor';
  carregando = false;
  erro = '';

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  alterarPerfil(perfil: UserRole): void {
    this.perfilSelecionado = perfil;
    this.erro = '';
    this.loginForm.reset();
  }

  async onSubmit(): Promise<void> {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.carregando = true;
    this.erro = '';

    try {
      const role = await this.authService.login(
        this.loginForm.value.email,
        this.loginForm.value.senha,
        this.perfilSelecionado
      );

      if (role === 'professor') {
        this.router.navigate(['/professor/dashboard']);
      } else if (role === 'aluno') {
        this.router.navigate(['/aluno/dashboard']);
      } else {
        this.router.navigate(['/admin/painel']);
      }
    } catch (error: any) {
      this.erro = 'Credenciais inválidas. Verifique seu e-mail e senha.';
      console.error(error);
    } finally {
      this.carregando = false;
    }
  }
}

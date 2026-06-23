import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService, UserRole } from '../../../../core/services/auth.service';
import { createClient, SupabaseClient } from '@supabase/supabase-js';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  perfilSelecionado: UserRole = 'professor';
  carregando = false;
  erro = '';

  // Reset de senha
  modalSenhaAberto = false;
  emailReset = '';
  enviandoReset = false;
  msgReset = '';
  erroReset = false;

  private supabase: SupabaseClient;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) {
    this.supabase = createClient(environment.supabaseUrl, environment.supabaseAnonKey);
  }

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
      if (role === 'professor') this.router.navigate(['/professor/dashboard']);
      else if (role === 'aluno') this.router.navigate(['/aluno/dashboard']);
      else this.router.navigate(['/admin/painel']);
    } catch {
      this.erro = 'Credenciais inválidas. Verifique seu e-mail e senha.';
    } finally {
      this.carregando = false;
    }
  }

  abrirEsqueciSenha(): void {
    this.emailReset = this.loginForm.value.email || '';
    this.msgReset = '';
    this.erroReset = false;
    this.modalSenhaAberto = true;
  }

  fecharEsqueciSenha(): void {
    this.modalSenhaAberto = false;
    this.emailReset = '';
    this.msgReset = '';
  }

  async enviarResetSenha(): Promise<void> {
    if (!this.emailReset) return;
    this.enviandoReset = true;
    this.msgReset = '';

    const { error } = await this.supabase.auth.resetPasswordForEmail(this.emailReset, {
      redirectTo: `${window.location.origin}/redefinir-senha`
    });

    this.enviandoReset = false;
    if (error) {
      this.erroReset = true;
      this.msgReset = 'Erro ao enviar e-mail. Verifique o endereço e tente novamente.';
    } else {
      this.erroReset = false;
      this.msgReset = 'Link enviado! Verifique sua caixa de entrada.';
    }
  }
}

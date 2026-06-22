import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
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
    private authService: AuthService // <-- Injeção do motor real do Supabase
  ) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  alterarPerfil(perfil: 'aluno' | 'professor' | 'admin'): void {
    this.perfilSelecionado = perfil;
    this.loginForm.reset();
  }

  // O método agora é assíncrono para aguardar a resposta da nuvem
  async onSubmit(): Promise<void> {
    if (this.loginForm.valid) {
      const email = this.loginForm.value.email;
      const senha = this.loginForm.value.senha;

      try {
        console.log('Iniciando handshake com o Supabase...');

        // 1. O disparo real para a nuvem
        await this.authService.login(email, senha);
        console.log('✅ Token JWT recebido e ancorado no navegador.');

        // 2. O roteamento tático provisório
        if (this.perfilSelecionado === 'professor') {
          this.router.navigate(['/professor/dashboard']);
        } else if (this.perfilSelecionado === 'aluno') {
          this.router.navigate(['/aluno/dashboard']);
        } else if (this.perfilSelecionado === 'admin') {
          this.router.navigate(['/admin/painel']);
        }

      } catch (error: any) {
        console.error('❌ Falha na barreira de entrada:', error.message);
        alert('Credenciais rejeitadas. Verifique se este usuário existe no painel do Supabase.');
      }

    } else {
      this.loginForm.markAllAsTouched();
    }
  }
}
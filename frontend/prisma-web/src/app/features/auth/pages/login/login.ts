import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router'; // <-- Importação adicionada

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

  // Injetamos o Router no construtor
  constructor(private fb: FormBuilder, private router: Router) {}

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

  onSubmit(): void {
    if (this.loginForm.valid) {
      const payload = {
        perfil: this.perfilSelecionado,
        email: this.loginForm.value.email,
        senha: this.loginForm.value.senha
      };
      
      console.log('Mock Auth OK, Payload:', payload);

      // Redirecionamento baseado no perfil selecionado
      if (this.perfilSelecionado === 'professor') {
        this.router.navigate(['/professor/dashboard']);
      } else {
        alert(`A área do ${this.perfilSelecionado} ainda não foi implementada no front-end.`);
      }

    } else {
      this.loginForm.markAllAsTouched(); 
    }
  }
}
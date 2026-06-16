import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router'; // <-- Necessário para o router-outlet e routerLink

@Component({
  selector: 'app-layout-aluno',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './layout-aluno.html'
})
export class LayoutAlunoComponent { }
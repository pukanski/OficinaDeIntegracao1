import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterModule], // Necessário para o router-outlet funcionar
  templateUrl: './layout.html'
})
export class LayoutComponent {}
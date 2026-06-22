import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-layout-admin',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './layout-admin.html'
})
export class LayoutAdminComponent {

  constructor(private authService: AuthService, private router: Router) { }

  sair(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
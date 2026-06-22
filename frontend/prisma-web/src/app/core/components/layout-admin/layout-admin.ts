import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-layout-admin',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './layout-admin.html'
})
export class LayoutAdminComponent { }
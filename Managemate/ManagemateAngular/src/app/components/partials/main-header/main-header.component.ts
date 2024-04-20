import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from '@angular/router';

@Component({
  selector: 'main-header',
  templateUrl: './main-header.component.html',
  standalone: true,
  imports: [TranslateModule, CommonModule, RouterModule],
  styleUrl: './main-header.component.css'
})
export class MainHeaderComponent {

  constructor(private auth: AuthService) { }

  isLoggedIn(): boolean {
    return this.auth.isLoggedIn();
  }
}

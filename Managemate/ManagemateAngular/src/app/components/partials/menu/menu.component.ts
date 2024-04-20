import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  standalone: true,
  imports:[CommonModule, RouterModule],
  styleUrl: './menu.component.css'
})
export class MenuComponent {

}

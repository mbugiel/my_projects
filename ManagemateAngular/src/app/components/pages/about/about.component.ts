import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from '@angular/router';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  standalone: true,
  imports:[CommonModule, RouterModule],
  styleUrl: './about.component.css'
})
export class AboutComponent {

}

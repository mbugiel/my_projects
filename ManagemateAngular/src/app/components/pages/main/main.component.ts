import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from '@angular/router';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  standalone: true,
  imports:[CommonModule, RouterModule],
  styleUrl: './main.component.css'
})
export class MainComponent {

}

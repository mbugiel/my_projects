import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from '@angular/router';
import {MenuComponent} from '../../../partials/menu/menu.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  standalone: true,
  imports:[CommonModule, RouterModule, MenuComponent],
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
}

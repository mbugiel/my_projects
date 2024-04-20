import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from '@angular/router';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  standalone: true,
  imports:[CommonModule, RouterModule],
  styleUrl: './contact.component.css'
})
export class ContactComponent {

}

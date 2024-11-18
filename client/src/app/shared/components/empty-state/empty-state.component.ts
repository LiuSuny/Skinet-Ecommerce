import { Component } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { routes } from '../../../app.routes';
import { RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [
    MatIcon,
    RouterLink,
    MatButton 
  ],
  templateUrl: './empty-state.component.html',
  styleUrl: './empty-state.component.scss'
})
export class EmptyStateComponent {

}

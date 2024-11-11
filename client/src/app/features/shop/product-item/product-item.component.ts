import { Component, Input } from '@angular/core';
import { Product } from '../../../shared/models/product';
import { MatCard, MatCardActions, MatCardContent } from '@angular/material/card';
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [
    MatCard,
    MatCardContent,
    CurrencyPipe,
    MatCardActions,
    MatButton,
    MatIcon
  ],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss'
})
//this is a child component class of shopcomponent 
//and we need to pass down list of items from the parent component- which is shopcomponent
export class ProductItemComponent {
  @Input() product?: Product
}

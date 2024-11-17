import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../shared/models/product';
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatDivider } from '@angular/material/divider';
import { CartService } from '../../../core/services/cart.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    CurrencyPipe,
    MatButton,
    MatIcon,
    MatFormField,
    MatInput,
    MatLabel,
    MatDivider,
    FormsModule
  ],
  templateUrl: './product-details.component.html', 
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {

  private shopService = inject(ShopService);
  private cartService = inject(CartService);
  private activeRoute = inject(ActivatedRoute);

  product?: Product;
  quantityInCart = 0;
  quantity = 1;

  ngOnInit(): void {
    this.loadProduct();
  }
  
  //load specific products
  loadProduct(){
    const id = this.activeRoute.snapshot.paramMap.get('id'); //note: this match id which was initialize in app.route 'shop/:id',
     if(!id) return;
     this.shopService.getProductById(+id).subscribe({
      next: response => {this.product = response,
                          this.updateQuantityInCart();
                        },
      error: error=> console.log(error)
     })
  }
 
  updateCart(){
    if(!this.product)return;
    if(this.quantity > this.quantityInCart){
      const itemToAdd  = this.quantity - this.quantityInCart; //this check if the quantity is greater than what is in cart
      this.quantityInCart += itemToAdd;
      this.cartService.addItemToCart(this.product, itemToAdd);
    } else {
      const removeItem  = this.quantityInCart - this.quantity;
      this.quantityInCart -= removeItem;
      this.cartService.removeItemFromCart(this.product.id, removeItem);
    }
  }
  //updating the quantity of products in cart 
  updateQuantityInCart(){
    this.quantityInCart = this.cartService.cart()?.items
    .find(x=> x.productId ===this.product?.id)?.quantity || 0;
    this.quantity = this.quantityInCart || 1;

  }

  getButtonText(){
    return this.quantityInCart > 0 ? 'Update cart ' : 'Add to cart';
  }

}

import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { map } from 'rxjs';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';


@Injectable({
  providedIn: 'root'
})
export class CartService {

  baseUrl = environment.apiUrl;
  
  private http = inject(HttpClient);

  cart = signal<Cart | null>(null); //using signal
  //getting the total count of item add to cart with signal computed
  itemCount = computed(() => {
    return this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0);
  })
 
  selectedDelivery = signal<DeliveryMethod | null>(null); //update delivery cost

  //total order summery of item using reduce fron js
  totals = computed(() => {
       const cart = this.cart();
       const delivery = this.selectedDelivery();
       if(!cart)return null;
       const subtotal = cart.items.reduce((sum, item) =>  sum + item.price * item.quantity, 0);
       const shipping = delivery ? delivery.price : 0;
       const discount = 0;
      
       return {
        subtotal,
        shipping,
        discount,
        total: subtotal + shipping - discount
       }
  })



  getCart(id: string){
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map(cart => {
        this.cart.set(cart)
        return cart;
      })
    )
  }

//this method update the cart on the redis server
  setCart(cart: Cart){
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: cart => this.cart.set(cart)
    })
  }

  //adding product to cart
  addItemToCart(item: CartItem | Product, quantity = 1) {
    //getting of the product items and if not create a cart
    const cart = this.cart() ?? this.createCart();
    if(this.isProduct(item)){
      item = this.mapProductToCartItem(item)
    }
    
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);
     return this.setCart(cart); //returning the signal
  }

  //empty the cart
  removeItemFromCart(productId: number, quantity = 1){
    const cart = this.cart();
    if(!cart)return;

    const index = cart.items.findIndex(x => x.productId === productId);

    if(index !== -1){

      if(cart.items[index].quantity > quantity){
        cart.items[index].quantity -= quantity; //reducing the quantity if it greater than the default one 
      }else{
        cart.items.splice(index, 1) //removing 
      }

      if(cart.items.length === 0){
        this.deleteCart();
      }else {
        this.setCart(cart)
      }
    }
   
  }
 //removing from redis serve side
  deleteCart() {
    return this.http.delete(this.baseUrl + 'cart?id=' + this.cart()?.id).subscribe({
      next: () => {
        localStorage.removeItem('cart_id');
        this.cart.set(null);
      }
    })
  }
  
  private addOrUpdateItem(items: CartItem[], item: CartItem, quantity: number): CartItem[] {
     const index = items.findIndex(x => x.productId === item.productId);
     if(index === -1){
      item.quantity = quantity;
      items.push(item); //sending to cart
     } else{
      items[index].quantity +=quantity; //to increase the amount in the basket
     }

     return items;
  }

  //mapping product to cartitem just dto sort of
  private mapProductToCartItem(item: Product): CartItem {
     return {
        productId: item.id,
        productName: item.name,
        price: item.price,
        quantity : 0,
        pictureUrl: item.pictureUrl,
        brand: item.brand,
        type: item.type
     }
  }

  private isProduct(item: CartItem | Product): item is Product {
    //the idea of this check if the item has id property b/c product has id property and the cartitem is not 
    //then that is not equal to undefined and item is a product and product is going to return true 
      return (item as Product).id !==undefined; 
  }

  //create a cart 
  private createCart(): Cart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }
}

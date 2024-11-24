import { inject, Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { forkJoin, of, tap } from 'rxjs';
import { AccountService } from './account.service';
import { SignalrService } from './signalr.service';

@Injectable({
  providedIn: 'root'
})
//this service class is use to persistance of data
export class InitService {

  //cart persitance
  private cartService = inject(CartService);
  private signalrService = inject(SignalrService);
 
  //current user data persistance
  private accountService = inject(AccountService);

  //method for product cart persistance
  init(){
    const cartId = localStorage.getItem('cart_id');
    const cart$ = cartId ? this.cartService.getCart(cartId) : of(null);

    //forkjoin - allow us to wait for multiple observable to complete and emit their latest value as array
    //and this good when we want to combine multiple http request
    return forkJoin ({
      //return a observable cart
      cart$,
      user: this.accountService.getUserInfo().pipe(
        tap(user => {
          if(user) this.signalrService.createHubConnection();
        })
      )
    })
  }
}

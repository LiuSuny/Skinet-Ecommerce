import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { inject } from '@angular/core';
import { SnarkbarService } from '../services/snarkbar.service';
import { CartService } from '../services/cart.service';

export const emptyCartGuard: CanActivateFn = (route, state) => {

  const cartService = inject(CartService);
  const router = inject(Router);
  const snack = inject(SnarkbarService);

  if(!cartService.cart() || cartService.cart()?.items.length === 0){
    snack.error("Sorry your cart is empty");
   router.navigateByUrl('/cart');
   return false; 
  }
  return true;
};

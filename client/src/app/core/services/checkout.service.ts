import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { AccountService } from './account.service';
import { environment } from '../../../environments/environment';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  private cartService = inject(CartService); //needed the cart details id so need acces to cart service
  private accountService = inject(AccountService);
  
  deliveryMethod: DeliveryMethod[] = [];

  getDeliveryMethods(){
      if(this.deliveryMethod.length > 0) return of(this.deliveryMethod);
    return this.http.get<DeliveryMethod[]>(this.baseUrl + 'payments/delivery-method').pipe(
      map(del => {
        //we sort then
        this.deliveryMethod = del.sort((a, b) => b.price - a.price);
        return del;
      })
    )
  }
}

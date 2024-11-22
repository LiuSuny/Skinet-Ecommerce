import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';

@Pipe({
  name: 'payment',
  standalone: true
})
export class PaymentPipe implements PipeTransform {

  transform(value?: ConfirmationToken['payment_method_preview'], ...args: unknown[]): unknown {
    if(value?.card){
      //destructuring 
      const {brand, exp_month, exp_year, last4} = value.card;
      return `${brand.toUpperCase()}, **** **** **** ${last4}, Exp: ${exp_month}/${exp_year}`;
     
    } else{
      return 'Unknown address';
    }

  }

}

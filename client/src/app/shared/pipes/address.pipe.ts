import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';
import { ShippingAddress } from '../models/order';

@Pipe({
  name: 'address',
  standalone: true
})
/*Pipes allows its users to change the format in which data
 is being displayed on the screen. For instance, consider 
 the date format. Dates can be represented in multiple ways, 
 and the user can decide which one to use with the help of Angular Pipes.*/
export class AddressPipe implements PipeTransform {

  transform(value?: ConfirmationToken['shipping'] | ShippingAddress, ...args: unknown[]): unknown {
    if(value && 'address' in value && value.name){
      //destructuring 
      const {line1, line2, city, state, postal_code, country} =
       (value as ConfirmationToken['shipping'])?.address!;

      return `${value.name}, ${line1}${line2 ? ', ' + line2 : ''}, 
              ${city}, ${state}, ${postal_code}, ${country}`;
    } 
    else if(value && 'line1' in value) {
      const {line1, line2, city, state, postalCode, country} = value as ShippingAddress
     return `${value.name}, ${line1}${line2 ? ', ' + line2 : ''}, 
             ${city}, ${state}, ${postalCode}, ${country}`;
    }
    else{
      return 'Unknown address';
    }

  }

}

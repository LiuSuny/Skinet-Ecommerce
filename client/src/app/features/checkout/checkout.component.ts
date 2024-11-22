import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { OrderSummaryComponent } from "../../shared/components/order-summary/order-summary.component";
import {MatStepperModule} from '@angular/material/stepper';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { StripeService } from '../../core/services/stripe.service';
import { StripeAddressElement, StripePaymentElement } from '@stripe/stripe-js';
import { SnarkbarService } from '../../core/services/snarkbar.service';
import {MatCheckboxChange, MatCheckboxModule} from '@angular/material/checkbox';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { Address } from '../../shared/models/user';
import { firstValueFrom } from 'rxjs';
import { AccountService } from '../../core/services/account.service';
import { CheckoutDeliveryComponent } from "./checkout-delivery/checkout-delivery.component";
import { CheckoutReviewComponent } from "./checkout-review/checkout-review.component";
import { CartService } from '../../core/services/cart.service';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    OrderSummaryComponent,
    MatStepperModule,
    MatButton,
    RouterLink,
    MatCheckboxModule,
    CheckoutDeliveryComponent,
    CheckoutReviewComponent,
    CurrencyPipe  
],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent implements OnInit, OnDestroy{
  
  private stripeService = inject(StripeService);
  addressElement?: StripeAddressElement;
  paymentElement?: StripePaymentElement;
  private snackService = inject(SnarkbarService);
  private accountService = inject(AccountService);
  cartService = inject(CartService);


   saveAddress = false

  async ngOnInit(){
    
    try{
      this.addressElement = await this.stripeService.CreateAddressElement();
      //if we have list of address element then we need to mount it - id pass at the template #address-element
      this.addressElement.mount('#address-element');

      //this responsible for strip payment
      this.paymentElement = await this.stripeService.createPaymentElement();
      //if we have list of address element then we need to mount it - id pass at the template #address-element
      this.paymentElement.mount('#payment-element');
    }
    catch(error: any) {
      this.snackService.error(error.messsage)
    }
  }
  
  async onStepChange(event:StepperSelectionEvent){
       if(event.selectedIndex === 1){
          
        if(this.saveAddress){
            const address = await this.getAddressFromStripeAddress();
            address && firstValueFrom(this.accountService.updateAddress(address))
        }
       }
       if(event.selectedIndex === 2){
        await firstValueFrom(this.stripeService.CreateOrUpdatePaymentIntent());
       }
  }

  //method that save default populated address to our backend db
  private async getAddressFromStripeAddress(): Promise<Address | null> {

     const result = await this.addressElement?.getValue();
     const address = result?.value.address;

     if(address){
      return{
        line1: address.line1,
        line2: address.line2 || undefined,
        city: address.city,
        state: address.state,
        postalCode: address.postal_code,
        country: address.country
      }
     } else {
      return null;
     }
  }
 
  onSaveAddressCheckBoxChange(event: MatCheckboxChange){
     this.saveAddress = event.checked;
  }
  ngOnDestroy(): void {
    this.stripeService.disposeElement();
  }
}

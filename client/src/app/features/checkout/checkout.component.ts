import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { OrderSummaryComponent } from "../../shared/components/order-summary/order-summary.component";
import {MatStepper, MatStepperModule} from '@angular/material/stepper';
import { MatButton } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { StripeService } from '../../core/services/stripe.service';
import { ConfirmationToken, StripeAddressElement, StripeAddressElementChangeEvent, StripePaymentElement, StripePaymentElementChangeEvent } from '@stripe/stripe-js';
import { SnarkbarService } from '../../core/services/snarkbar.service';
import {MatCheckboxChange, MatCheckboxModule} from '@angular/material/checkbox';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { Address } from '../../shared/models/user';
import { firstValueFrom } from 'rxjs';
import { AccountService } from '../../core/services/account.service';
import { CheckoutDeliveryComponent } from "./checkout-delivery/checkout-delivery.component";
import { CheckoutReviewComponent } from "./checkout-review/checkout-review.component";
import { CartService } from '../../core/services/cart.service';
import { CurrencyPipe, JsonPipe } from '@angular/common';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';

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
    CurrencyPipe,
    //JsonPipe
    MatProgressSpinnerModule
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
  private router = inject(Router);
  cartService = inject(CartService);

  saveAddress = false
  //validate user stepper
  completionStatus = signal<{address: boolean, card: boolean, delivery: boolean}>({
    address: false, card: false, delivery: false
  })

  confirmationToken?: ConfirmationToken;
  
  //property for load
  loading = false;


  //one way to bind event method to a class using ctor but they r other way using arrow function
    // constructor(){
    //   this.handleAddressChange = this.handleAddressChange.bind(this)
    // }


  async ngOnInit(){
    
    try{
      this.addressElement = await this.stripeService.CreateAddressElement();
      //if we have list of address element then we need to mount it - id pass at the template #address-element
      this.addressElement.mount('#address-element');
      this.addressElement.on('change', this.handleAddressChange);//passing in the event method to validate

      //this responsible for strip payment
      this.paymentElement = await this.stripeService.createPaymentElement();
      //if we have list of address element then we need to mount it - id pass at the template #address-element
      this.paymentElement.mount('#payment-element');
      this.paymentElement.on('change', this.handlePaymentChange);//passing in the event method to validate

    }
    catch(error: any) {
      this.snackService.error(error.messsage)
    }
  }

  //this event method validate if the address is correcting filled before stepping into another stepper
  handleAddressChange = (event:StripeAddressElementChangeEvent ) => {
    this.completionStatus.update(state => {
        state.address = event.complete
        return state;
    })
  }

   //this event method validate if the payment is correcting filled before stepping into another stepper
   handlePaymentChange = (event: StripePaymentElementChangeEvent) => {
    this.completionStatus.update(state => {
        state.card = event.complete
        return state;
    })
  }
  
   //this event method validate if the delivery is correcting filled before stepping into another stepper
   //in this we r emitting from child component to parents class using signal output
   handleDeliveryChange (event: boolean) {
    this.completionStatus.update(state => {
        state.delivery = event;
        return state;
    })
  }

  async getConfirmationToken(){
    //first we need to validate the stepper completestatus are true before confirming
      try {
        if(Object.values(this.completionStatus()).every(status => status === true)){
             const result = await this.stripeService.createConfirmationTokens();
             if(result.error) throw new Error(result.error.message);
             this.confirmationToken = result.confirmationToken;
             console.log(this.confirmationToken); //to be remove later on
        }
      } catch (error: any) {
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
       if(event.selectedIndex === 3){
        await this.getConfirmationToken();
       }
  }

  async confirmPayment(stepper: MatStepper){
    this.loading = true;
      try {
        if(this.confirmationToken){
          const result = await this.stripeService.confirmPayments(this.confirmationToken);
          if(result.error) {
            throw new Error(result.error.message);
          }else{
            this.cartService.deleteCart();
            this.cartService.selectedDelivery.set(null);
            this.router.navigateByUrl('/checkout/success')
          }

        }

      } catch (error: any) {
        this.snackService.error(error.messsage || 'Something went wrong');
        stepper.previous();
      } finally{
        this.loading = false;
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

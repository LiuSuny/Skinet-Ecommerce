import { inject, Injectable } from '@angular/core';
import {ConfirmationToken, loadStripe, Stripe, StripeAddressElement, StripeAddressElementOptions, StripeElements, StripePaymentElement} from '@stripe/stripe-js';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CartService } from './cart.service';
import { Cart } from '../../shared/models/cart';
import { firstValueFrom, lastValueFrom, map } from 'rxjs';
import { AccountService } from './account.service';


@Injectable({
  providedIn: 'root'
})
export class StripeService {

  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  private cartService = inject(CartService); //needed the cart details id so need acces to cart service
  private accountService = inject(AccountService);

 //need a single instance of stripe to work with
 private stripePromise: Promise<Stripe | null>;

 //stripe element are used for storage like our payment details etc
 private elements?: StripeElements;

 //stripe element  address are used for storage like our address details etc
 private addressElements?: StripeAddressElement;

 //stripe payment element
  private paymentElement?: StripePaymentElement;

 constructor(){
  this.stripePromise = loadStripe(environment.stripePublicKey);
 }

  //method to get stripe instance
  getStripeInstance() /*: Promise<Stripe | null>*/{
    return this.stripePromise;
  }

  //initialize elements store client address cart details we use async here instead of
  // promise that stripe also use because want to await this task
  async initializeElement(){
    //check if we did not have elements already then initialize it
    if(!this.elements){
         const stripe = await this.getStripeInstance();
         //check if we have stripe then we create or update payment intent
         if(stripe){
          //to await the creation of our cart and since our CreateOrUpdatePaymentIntent() return observable we need to convert
          //it from observable method to promise to await it using FIRSTVALUEFROM or LastvalueFrom
            const cart = await firstValueFrom(this.CreateOrUpdatePaymentIntent()); //this give our carts
            this.elements = stripe.elements({clientSecret: cart.clientSecret,
               appearance: {labels: 'floating'
            }})
         } else{
             throw new Error('Stripe has not been loaded');
         }
    }
    return this.elements;
  }
  
  //
  async createPaymentElement(){
    if(!this.paymentElement){
      const element = await this.initializeElement();
      if(element){
        this.paymentElement = element.create('payment');
      }else{
        throw new Error('The element instance has not been initialize');
      }
    }
    return this.paymentElement;
  }

  async CreateAddressElement(){
    //check if we did not have elements already then initialize it
    if(!this.addressElements){
         const element = await this.initializeElement();
         if(element){  

           const user = this.accountService.currentUser(); //get the currentuser from acct service
    
           let defaultValues: StripeAddressElementOptions['defaultValues'] = {};

            if(user) {
              defaultValues.name = user.firstName + ' ' + user.lastName;
            }
            //check default address by mapping from our currentuser defualt address to StripeAddressElementOptions
            if(user?.address){
              defaultValues.address = {
                line1 : user.address.line1,
                line2 : user.address.line1,
                city: user.address.city,
                state: user.address.state,
                postal_code: user.address.postalCode,
                country: user.address.country
              }
            }

            const options : StripeAddressElementOptions  = {
                 mode: 'shipping',  //shipping or billing
                 defaultValues //passing in the default address opton
            };
            this.addressElements = element.create('address', options); //create the shipping 
         } else {
              throw new Error('The element instance has not been loaded');
         }

    }
    return this.addressElements;
  }

  //generate a confirm token from stripe
  async createConfirmationTokens(){
      const stripe = await this.getStripeInstance(); //get hold of stripe
      const elements = await this.initializeElement(); //get access to element

      //next we need to submit th element
        const result = await elements.submit();

      if(result.error) throw new Error(result.error?.message);
      
      //then create the token
      if(stripe){
        return await stripe.createConfirmationToken({elements})
      }else{
        throw new Error('stripe not available');
      }
         
  }

  //this method confirm the payment
  async confirmPayments(confirmationTokens: ConfirmationToken){
    const stripe = await this.getStripeInstance(); //get hold of stripe
    const elements = await this.initializeElement(); //get access to element
     //next we need to submit th element
     const result = await elements.submit();

     if(result.error) throw new Error(result.error?.message);

     const clientSecret = this.cartService.cart()?.clientSecret;
     if(stripe && clientSecret){
       return await stripe.confirmPayment({
        clientSecret: clientSecret,
        confirmParams: {
           confirmation_token: confirmationTokens.id
        },
         redirect: 'if_required'
       })
     }else{
      throw new Error('stripe is not available');
    }
  }

  //method to create or update payment intent
  CreateOrUpdatePaymentIntent(){
    const cart = this.cartService.cart(); //getting hold of the cart
    if(!cart) throw new Error('Problem with cart');
    
    return this.http.post<Cart>(this.baseUrl + 'payments/'+ cart.id, {}).pipe(
      map(cart => {
        this.cartService.setCart(cart)
        return cart;
      }
       
      )
    )

  }

  //this remove users details address once logout of our application and prevent others using the default address
  disposeElement(){
    this.elements = undefined;
    this.addressElements = undefined;
    this.paymentElement = undefined;
  }

}

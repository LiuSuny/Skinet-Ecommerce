import { Component, inject, OnDestroy } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { SignalrService } from '../../../core/services/signalr.service';
import { CurrencyPipe, DatePipe, NgIf } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AddressPipe } from '../../../shared/pipes/address.pipe';
import { PaymentPipe } from '../../../shared/pipes/payment.pipe';
import { OrderService } from '../../../core/services/order.service';

@Component({
  selector: 'app-checkout-success',
  standalone: true,
  imports: [
    MatButton,
    RouterLink ,
    CurrencyPipe,
    DatePipe,
    MatProgressSpinnerModule,
    AddressPipe,
    PaymentPipe,
    NgIf 
  ],
  templateUrl: './checkout-success.component.html',
  styleUrl: './checkout-success.component.scss'
})
export class CheckoutSuccessComponent implements OnDestroy {
   signalrService = inject(SignalrService);
  private orderService = inject(OrderService);

   ngOnDestroy(): void {
     this.orderService.orderComplete = false;
     this.signalrService.orderSourceSignal.set(null);
   }
}
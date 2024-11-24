import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';
import { Order } from '../../../shared/models/order';
import { MatCardModule } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { AddressPipe } from "../../../shared/pipes/address.pipe";
import { PaymentPipe } from "../../../shared/pipes/payment.pipe";

@Component({
  selector: 'app-order-detailed',
  standalone: true,
  imports: [
    MatCardModule,
    DatePipe,
    MatButton, 
    CurrencyPipe,
    AddressPipe,
    PaymentPipe,
    RouterLink 
],
  templateUrl: './order-detailed.component.html',
  styleUrl: './order-detailed.component.scss'
})
export class OrderDetailedComponent implements OnInit{
  private orderService = inject(OrderService);
  private activatedRoute = inject(ActivatedRoute)
  order?: Order;

  //then we will be needing this lifecycle hook b/c we will be going out to our api 
    ngOnInit(): void {
      this. loadOrder();
    }

    loadOrder(){
      //get hold of the id first
      const id = this.activatedRoute.snapshot.paramMap.get('id');
      //we check if we have id first
      if(!id) return;
       
      //if the id is true then
      this.orderService.getOrdersDetailById(+id).subscribe ({
        next: order => this.order = order
      })

    }
}

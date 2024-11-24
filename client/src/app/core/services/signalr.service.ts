import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { SnarkbarService } from './snarkbar.service';
import { Order } from '../../shared/models/order';


@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  hubUrl = environment.hubUrl;
  
  //hub connection
  hubConnection?: HubConnection;

   orderSourceSignal = signal<Order | null>(null);

 createHubConnection()
 {
     //this take care of creating the hub connection
    this.hubConnection = new HubConnectionBuilder()
        .withUrl(this.hubUrl,   //passing in the url
         {
            withCredentials: true //so our cookies can be send
        })
        .withAutomaticReconnect() //if there is a network problem our user is going to try and  reconnect to our hub
        .build();

        //next we start the hubconnection
        this.hubConnection
         .start()
         .catch(error => console.log(error));

       //listen to server event if user is online 
       this.hubConnection.on('OrderCompleteNotification', (order: Order) => {
       this.orderSourceSignal.set(order);
     })
    }

    stopHubConnection()
    {   
         //next we stop the hubconnection
         if(this.hubConnection?.state === HubConnectionState.Connected){
          this.hubConnection?.stop()
          .catch(error => console.log(error)); 
         }
           
    }
}

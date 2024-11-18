import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Address, User } from '../../shared/models/user';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  
  currentUser = signal<User | null>(null);


  login(values: any){
    let params = new HttpParams();
      params = params.append('useCookies', true);
    
      //return this.http.post<User>(this.baseUrl + 'login', values, {params, withCredentials: true});
      return this.http.post<User>(this.baseUrl + 'login', values, {params});


  }

  register(values: any){

      return this.http.post(this.baseUrl + 'account/register', values)
  }

  getUserInfo(){
     
    //return this.http.get<User>(this.baseUrl + 'account/user-info', { withCredentials: true}).pipe(
      return this.http.get<User>(this.baseUrl + 'account/user-info').pipe(
        map(user => {
          this.currentUser.set(user);
          return user;
        })
      )

  }

  logOut(){

    //return this.http.post(this.baseUrl + 'account/logout', {}, { withCredentials: true}); //interceptor now handle the withcredential
      return this.http.post(this.baseUrl + 'account/logout', {});
  }

  updateAddress(address: Address){

      return this.http.post(this.baseUrl + 'account/address', address);

  }
}

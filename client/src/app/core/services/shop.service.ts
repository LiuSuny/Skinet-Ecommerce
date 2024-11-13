import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Product } from '../../shared/models/product';
import { Pagination } from '../../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ShopService {

  baseUrl = 'https://localhost:5001/api/'
  private http = inject(HttpClient);
  types: string[] = [];
  brands: string[] = [];

  

  getProducts(){

   return this.http.get<Pagination<Product>>(this.baseUrl + 'products?pageSize=20');
  }

  
  getProductsBrand(){
    if(this.brands.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
      next: response => this.brands = response
    })
   }

   getProductsType(){
    if(this.types.length > 0) return; //help us not store info in our service
    return this.http.get<string[]>(this.baseUrl + 'products/types').subscribe({
      next: response => this.types = response
    })
   }
}

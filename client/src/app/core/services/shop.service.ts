import { HttpClient, HttpParams } from '@angular/common/http';
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

  

  getProducts(brands?: string[], types?: string[]){
     let params = new HttpParams();
     //check if we have brands or types
     if(brands && brands.length> 0){
        params =   params.append('brands', brands.join(','));
     }
     
     if(types && types.length> 0){
      params =   params.append('types', types.join(','));
    }

    params = params.append('pageSize', 20);
   return this.http.get<Pagination<Product>>(this.baseUrl + 'products', {params});
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

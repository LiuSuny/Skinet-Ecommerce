import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Product } from '../../shared/models/product';
import { Pagination } from '../../shared/models/pagination';
import { ShopParams } from '../../shared/models/shopParams';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ShopService {

  baseUrl = environment.apiUrl;

  private http = inject(HttpClient);
  types: string[] = [];
  brands: string[] = [];

  
 // method to get all our products
  getProducts(shopParams: ShopParams) {

    let params = new HttpParams();
    //filtering brands
    if (shopParams.brands.length > 0) {
      params = params.append('brands', shopParams.brands.join(','));
    }
    //filtering types
    if (shopParams.types.length > 0) {
      params = params.append('types', shopParams.types.join(','));
    }
    //filtering sorting
    if (shopParams.sort) {
      params = params.append('sort', shopParams.sort);
    }
    //filtering by search
    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }
    //adding page size
    params = params.append('pageSize', shopParams.pageSize);
    //adding page number
    params = params.append('pageIndex', shopParams.pageNumber);
    
    //get all products
    return this.http.get<Pagination<Product>>(this.baseUrl + 'products', {params});
  }
  
  //get a specific product by id
  getProductById(id: number){
    return this.http.get<Product>(this.baseUrl + 'products/' + id);
  }

  //Get product by brand method
  getProductByBrands(){
    if(this.brands.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
      next: response => this.brands = response
    })
   }

  //Get product by type method
   getProductByTypes(){
    if(this.types.length > 0) return; //help us not store info in our service
    return this.http.get<string[]>(this.baseUrl + 'products/types').subscribe({
      next: response => this.types = response
    })
   }
}

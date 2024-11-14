import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/product';
import { MatCard } from '@angular/material/card';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    ProductItemComponent,
    MatButton,
    MatIcon 
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog); //this service come from of angular material not our component dialog
    
  products: Product[] = [];
  selectedBrands: string[] = [];
  selectedTypes: string[] =[];

  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop(){
    this.shopService.getProductsBrand();
    this.shopService.getProductsType();
    this.shopService.getProducts().subscribe({
      next: response => this.products = response.data,
      error:error => console.log(error)
     })
  }

  //Method that open filter dialog using angular material
  openFilterDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.selectedBrands,
        selectedTypes: this.selectedTypes
      }

    });
    dialogRef.afterClosed().subscribe({
      next: result => {
        //checking if we have result
        if(result)
        {
          this.selectedBrands = result.selectedBrands;
          this.selectedTypes = result.selectedTypes
          //apply filter
          this.shopService.getProducts(this.selectedBrands, this.selectedTypes).subscribe({
            next: response => this.products = response.data,
            error:error => console.log(error)

          })

        }
      }
    })
  }
}

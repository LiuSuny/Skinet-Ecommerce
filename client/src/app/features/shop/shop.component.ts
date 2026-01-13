import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/product';
import { MatCard } from '@angular/material/card';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { ShopParams } from '../../shared/models/shopParams';
import { Pagination } from '../../shared/models/pagination';
import { EmptyStateComponent } from "../../shared/components/empty-state/empty-state.component";

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator, //angular material paginator
    FormsModule,
    EmptyStateComponent
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog); //this service come from angular material not our component dialog
    
  products?: Pagination<Product>; //getting of our products togather with pagination
 //sort option
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low-High', value: 'priceAsc'},
    {name: 'Price: High-Low', value: 'priceDesc'},
  ]

  //shop paramaters model
  shopParams = new ShopParams();

  //default page options
  pageSizeOptions = [5,10,15,20]
  
  ngOnInit() {
    this.initialiseShop();
  }
  //initially our shop
  initialiseShop() {
    this.shopService.getProductByTypes();
    this.shopService.getProductByBrands();
    this.getProducts();
  }
  
  //Redetfilter our shop
  resetFilters() {
    this.shopParams = new ShopParams();
    this.getProducts();
  }

  //get all our product
  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: response => this.products = response,
      error: error => console.error(error)
    })
  }

  //method that handle product searching
  onSearchChange() {
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  //method that handle page change event
  handlePageEvent(event: PageEvent) {
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }

  //method that handle page sortig
  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      this.shopParams.pageNumber = 1; //put user on page number 1 after sorting products
      this.getProducts();
    }
  }

  //method that handle filtering of brands, types and sorting and opening of modals
  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types
      }
    });
    //closing our modal dialog
    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.types = result.selectedTypes;
          this.shopParams.pageNumber = 1; //put user on page number 1 after sorting products
          this.getProducts();
        }
      }
    })
  }
}


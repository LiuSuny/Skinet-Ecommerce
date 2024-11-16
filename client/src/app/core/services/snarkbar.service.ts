import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})

//snackbar material in angular is like a toastr alert 
export class SnarkbarService{

  private snackbar = inject(MatSnackBar);

  error(message: string){
    this.snackbar.open(message, 'Close', {
      duration: 5000, //how long it going to display
      panelClass: ['snark-error']  //for extra css so we customize the look instead of default one
    })
  }


  success(message: string){
    this.snackbar.open(message, 'Close', {
      duration: 5000, //how long it going to display
      panelClass: ['snark-success']  //for extra css so we customize the look instead of default one
    })
  }
}

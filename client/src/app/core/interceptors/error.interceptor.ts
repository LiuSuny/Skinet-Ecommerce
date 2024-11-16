import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnarkbarService } from '../services/snarkbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  const router = inject(Router);
  const snackbarService = inject(SnarkbarService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) =>{

      if(err.status === 400){
        if(err.error.errors)
        {
          const modelStateError =[];
          for(const key in err.error.errors){
             if(err.error.errors[key]){
               modelStateError.push(err.error.errors[key]) //the idea is to flatten the--- 
               //error response we get back from our validation response and push then into array modelStateError =[];
             }
        }
        throw modelStateError.flat();
      } 
      else {
        snackbarService.error(err.error.title || err.error);
       }
      }

      if(err.status === 401){
        snackbarService.error(err.error.title || err.error);
      }
      if(err.status === 404){
        router.navigateByUrl('/not-found');
      }
      if(err.status === 500){
        const navigationExtra: NavigationExtras = {state:{error: err.error}}
        router.navigateByUrl('/server-error', navigationExtra);
      }
      return throwError(() => err);
    })

  )
};

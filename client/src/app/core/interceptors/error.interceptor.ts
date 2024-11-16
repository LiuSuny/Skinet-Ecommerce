import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnarkbarService } from '../services/snarkbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  const router = inject(Router);
  const snackbarService = inject(SnarkbarService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) =>{
      if(err.status === 400){
        snackbarService.error(err.error.title || err.error);
      }
      if(err.status === 401){
        snackbarService.error(err.error.title || err.error);
      }
      if(err.status === 404){
        router.navigateByUrl('/not-found');
      }
      if(err.status === 500){
        router.navigateByUrl('/server-error');
      }
      return throwError(() => err);
    })

  )
};

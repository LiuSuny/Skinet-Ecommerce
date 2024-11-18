import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountService } from '../services/account.service';

//this help us to pass withcredential in this our cookies
export const authInterceptor: HttpInterceptorFn = (req, next) => {

  
  const cloneRequest = req.clone({
    withCredentials: true
  })

  return next(cloneRequest);
};

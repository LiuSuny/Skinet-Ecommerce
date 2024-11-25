import { HttpInterceptorFn } from '@angular/common/http';
import { delay, finalize, identity } from 'rxjs';
import { BusyService } from '../services/busy.service';
import { inject } from '@angular/core';
import { environment } from '../../../environments/environment';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {

  const busyService =  inject(BusyService);
  busyService.busy()
  return next(req).pipe(
    //delay(500),
    //checking if we are in production or delopement to add delay or not
     (environment.production ? identity : delay(500)),
    finalize(() => {
      busyService.idile();
     })
  )

};

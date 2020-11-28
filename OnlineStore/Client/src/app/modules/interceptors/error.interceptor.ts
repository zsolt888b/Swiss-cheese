import { Injectable, Injector } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private injector : Injector, private toastr : ToastrService, private router : Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        retry(1),
        catchError((error: HttpErrorResponse) => {

          if(error.status == 401){
            this.toastr.error("Unauthorized!");
            this.router.navigate(['']);
            return throwError("Server side error");
          }

          let self = this;
          let reader = new FileReader();
          reader.onload = function(){
            self.injector.get(ToastrService).error(reader.result as string)
          }
          reader.readAsText(error.error)

          return throwError("Server side error");
        })
      )
  }
}

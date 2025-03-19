import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  let authService : AuthService;
  
  try {
    authService = inject(AuthService);
  } catch (e) {
    console.error("AuthService non è disponibile nel contesto corrente:", e);
    return next(req);
  }

  const token = authService.getToken();


  if (token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(cloned);
  }

  return next(req);
};

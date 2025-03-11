import { HttpClient } from '@angular/common/http';
import { inject, Injectable} from '@angular/core';
import { Router } from '@angular/router';



@Injectable({
  providedIn: 'root'
})
export class AuthService {

  router=inject(Router);
  http=inject(HttpClient);
  baseUrl = 'https://localhost:7264/api';
  private tokenKey = 'authToken';

  createUser(formData:any){
    const Url = this.baseUrl+'/Auth/register';
    return this.http.post(Url, formData)
  }

  loginUser(formData:any){
    const Url = this.baseUrl+'/Auth/login';
    return this.http.post(Url, formData)
  }

  // Metodo per salvare il token
  saveToken(token: string): void {
    sessionStorage.setItem(this.tokenKey, token); // Usa localStorage se vuoi che il token resti dopo la chiusura del browser
  }

  // Metodo per recuperare il token
  getToken(): string | null {
    return sessionStorage.getItem(this.tokenKey);
  }

  // Metodo per rimuovere il token (logout)
  logoutUser(): void {
    sessionStorage.removeItem(this.tokenKey);
    this.router.navigateByUrl('/')
  }
}


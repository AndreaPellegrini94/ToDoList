import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { UserEmail } from '../models/userMail.types';
import { Observable } from 'rxjs';
import { UserPassword } from '../models/userPassword.type';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  router=inject(Router);
  http=inject(HttpClient);
  baseUrl = 'https://localhost:7264/api';



  getUser(){
    const Url = this.baseUrl + '/User';
    return this.http.get<UserEmail>(Url);
  }

  
  deleteUser(){
    const Url = this.baseUrl + `/User/Delete/`;
    return this.http.delete(Url, {responseType: 'text'});
  }

  updatePassword(password : UserPassword ): Observable<string> {
    const Url = `${this.baseUrl}/User/Update/`;
    return this.http.put<string>(Url, password, {responseType: 'text' as 'json'});
  }

}

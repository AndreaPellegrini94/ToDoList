import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  router=inject(Router);
  http=inject(HttpClient);
  baseUrl = 'https://localhost:7264/api';
  
  deleteUser(){
    const Url = this.baseUrl + `/User/Delete/`;
    return this.http.delete(Url)
  }

}

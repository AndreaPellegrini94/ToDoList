import { Component, inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-header',
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  authService = inject(AuthService);
  router = inject(Router);

  isAuthenticated(): boolean {
    return this.authService.getToken() !== null;
  }

  logout(): void {
    const confirmation = window.confirm('Do you want to logout?');
    if(confirmation){
      this.authService.logoutUser();
      this.router.navigate(['/']); 
    }
  }

  gotoTodoLis(){
    this.router.navigate(['/todos']); 
  }

  getUserName(): string {
    const token = sessionStorage.getItem('authToken');
    if (token) {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress']; 
    }
    return 'Guest';
  }
}

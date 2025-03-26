import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  authService = inject(AuthService);
  userService = inject(UserService);
  router = inject(Router);

  isAuthenticated(): boolean {
    return this.authService.getToken() !== null;
  }

  isOnTodos():boolean {
    return this.router.url !== '/todos';
  }

  isOnProfile():boolean{
    return this.router.url !== '/profile';
  }

  viewProfile() {
    this.router.navigate(['/profile']);
  }

  gotoTodoLis(){
    this.router.navigate(['/todos']); 
  }

  logout(): void {
    const confirmation = window.confirm('Do you want to logout?');
    if(confirmation){
      this.authService.logoutUser();
      this.router.navigate(['/']); 
    }
  }

}

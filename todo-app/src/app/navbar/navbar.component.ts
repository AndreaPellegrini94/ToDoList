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

  viewProfile() {
    this.router.navigate(['/profile']);
  }

  editProfile() {
    this.router.navigate(['/edit-profile']);
  }

  deleteAccount() {
    if (confirm('Sei sicuro di voler eliminare il tuo account?')) {
      this.userService.deleteUser().subscribe(() => {
        alert('Account eliminato con successo');
        this.router.navigate(['/login']);
      });
    }
  }

}

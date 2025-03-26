import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { catchError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { UserEmail } from '../models/userMail.types';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {

  authService = inject(AuthService);
  userService = inject(UserService);
  router = inject(Router);
  toastr = inject(ToastrService);
  emailItem =  signal<String>('');
  fb = inject(FormBuilder);

  showPasswordForm = false;
 
  passwordForm = this.fb.group({
    currentPassword: [''],
    newPassword: ['']
  });

  ngOnInit(): void {
    
    const token = this.authService.getToken();
        
        if(token != null){
        this.userService.getUser()
          .pipe(
            catchError((err) => {
              console.log(err);
              throw err;
            })
          )
          .subscribe((response: UserEmail) => {
            this.emailItem.set(response.email);
            console.log(response.email);  // La mail iniziale dalla risposta
          });
        }
        else {
          this.toastr.error('You are not loged in: please go to login or register first.')
          this.router.navigateByUrl("/");
        }
  }


  delete(): void{
    const confirmation = window.confirm('Are you sure you want to delete your account?');
    if (confirmation) {
      this.userService.deleteUser().subscribe({
        next: (response) => {
          alert(response); // Mostra la risposta del server ("Delete successful")
          this.authService.logoutUser();
        },
        error: (err) => {
          alert(err.error || 'An error occurred while deleting your account.');
        }
      });
    }
  }


  updatePassword(): void {

    const passwordData = {
      currentPassword: this.passwordForm.value.currentPassword!,
      newPassword: this.passwordForm.value.newPassword!
    };

    this.userService.updatePassword(passwordData)
    .subscribe({
      next: () => {
        this.toastr.success("Password updated successfully!");
        this.passwordForm.reset();
      },
      error: (err) => {
        console.log(err)
        this.toastr.error(err.error, 'Failed to update password.');
      }
    });
  }

}





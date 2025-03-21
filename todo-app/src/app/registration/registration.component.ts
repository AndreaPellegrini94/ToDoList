import { Component, inject } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators,} from '@angular/forms'
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { RouterLink, Router } from '@angular/router';



@Component({
  selector: 'app-registration',
  imports: [ReactiveFormsModule],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css'
})
export class RegistrationComponent {
  form!: FormGroup

  service = inject(AuthService);

  toastr = inject(ToastrService);

  router= inject(Router);

  
  constructor(public formBuilder: FormBuilder){
    
    this.form = this.formBuilder.group({
      username : [''],
      email: [''],
      password: ['']
    })
  }
  onSubmit(){
     
      this.service.createUser(this.form.value)
      .subscribe({
        next:res=>{
            this.form.reset();
            this.toastr.success("New User created", "Registration Successful");
            this.router.navigateByUrl('/')

        },
        error:(err:any)=>{
      

          if (typeof err.error === "string") {
            // Caso in cui l'errore è un semplice messaggio stringa
            this.toastr.error(err.error, "Validation Error");
          } 
          else if (Array.isArray(err.error)) {
            // Caso in cui il backend restituisce un array di errori
            err.error.forEach((e: any) => {
              this.toastr.error(e.description, "Validation Error");
            });
          } 
          else if (err.error && err.error.errors) {
            // Caso classico degli errori di validazione modello
            Object.entries(err.error.errors).forEach(([key, messages]) => {
              (messages as string[]).forEach((message: string) => {
                this.toastr.error(message, "Validation Error");
              });
            });
          } 
          else {
            // Caso fallback per errori sconosciuti
            this.toastr.error("Something went wrong. Please try again.", "Error");
          }
        
        }
      })
  }
  goBack(){
    this.router.navigateByUrl('/')
  }
}

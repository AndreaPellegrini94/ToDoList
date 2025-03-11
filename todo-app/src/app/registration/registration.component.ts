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
       
          if(err.error == 'Invalid email.'){
            this.toastr.error(err.error,"Please Tray Again")
            
          }
          else{
            
            this.toastr.error(`${err.error[0].description}`,"Please Tray Again")
          }
        }
      })
  }
  goBack(){
    this.router.navigateByUrl('/')
  }
}

import { Routes } from '@angular/router';

export const routes: Routes = [
    
    {
        path: '',
        pathMatch: 'full',
        loadComponent:()=>{
            return import('./login/login.component').then(
                m=>m.LoginComponent
                )
        }
    },
    
    {
        path: 'registration',
        loadComponent:()=>{
            return import('./registration/registration.component').then(
                m=>m.RegistrationComponent
                )
        }
    },
    {
        path: 'todos',
        loadComponent:()=>{
            return import('./todos/todos.component').then(
                m=>m.TodosComponent
                )
        }
    }
];

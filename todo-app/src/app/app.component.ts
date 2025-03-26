import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { NavbarComponent } from "./navbar/navbar.component";


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  template:`
    
    <app-login></app-login>
    <app-registration></app-registration>
    <app-todos></app-todos>
    <app-profile></app-profile>
    <main>
    </main>
    <router-outlet/>
    `,
})
export class AppComponent {
  title = 'todo-app';
}

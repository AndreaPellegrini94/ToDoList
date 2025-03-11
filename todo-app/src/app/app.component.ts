import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './header/header.component';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet,HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  template:`
    
    <app-login></app-login>
    <app-registration></app-registration>
    <app-todos></app-todos>
    <main>
    </main>
    <router-outlet/>
    `,
})
export class AppComponent {
  title = 'todo-app';
}

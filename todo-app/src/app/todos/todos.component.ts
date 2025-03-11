import { Component, inject, OnInit, signal } from '@angular/core';
import { TodosService } from '../services/todos.service';
import { CommonModule } from '@angular/common'; 
import { todoItem } from '../models/todo.types';
import { catchError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { HighlightCompletedTodoDirective } from '../directives/highlight-completed-todo.directive';


@Component({
  selector: 'app-todos',
  standalone: true, 
  imports: [CommonModule, FormsModule, HighlightCompletedTodoDirective], 
  templateUrl: './todos.component.html',
  styleUrls: ['./todos.component.css']
})
export class TodosComponent implements OnInit {
  todoService = inject(TodosService);
  todoItems = signal<Array<todoItem>>([]);  // Signal per gestire lo stato dei ToDo
  newTodoTitle: string = '';  //titolo del todo che voglio modificare
  editTodoId: number | null = null ; //contenitore per gestire l íd del todo di cui voglio modificare il testo

  ngOnInit(): void {
    this.todoService.getUserTodos()
      .pipe(
        catchError((err) => {
          console.log(err);
          throw err;
        })
      )
      .subscribe((todos) => {
        this.todoItems.set(todos);  // Imposta i ToDo iniziali dalla risposta
      });
  }

  addTodo(): void {
    if (!this.newTodoTitle.trim()) {
      return;
    }

    let newTodo: todoItem = {
      title: this.newTodoTitle,
      isDone: false,
      id : 0,
      userId: '',
    };

    this.todoService.addTodoTitle(newTodo)
      .pipe(
        catchError((err) => {
          console.log(err);
          throw err;
        })
      )
      .subscribe((addedTodo) => {
        this.todoItems.set([...this.todoItems(), addedTodo]); // Aggiungi il nuovo ToDo alla lista
        this.newTodoTitle = '';  // Resetta il titolo dell'input
      });
  }

  deleteTodo(todoId: number): void {
    const confirmation = window.confirm('Do you want to cancel this element?');
    if (confirmation) {
      this.todoService.deleteTodoItem(todoId).subscribe(() => {
        this.todoItems.set(this.todoItems().filter(todo => todo.id !== todoId));
      });
    }
  }

  updateToDo(todo: todoItem) {
    // Assicurati che 'completed' venga mappato correttamente a 'IsDone'
    const updatedTodo = {
      id: todo.id,
      title: todo.title,
      isDone: !todo.isDone,  // Mappa 'completed' a 'IsDone'
      userId: todo.userId      // Mantieni gli altri campi invariati
    };

    // Chiama il servizio per aggiornare il To-Do
    this.todoService.updateToDoItem(updatedTodo).subscribe({
      next: () => {
        
        // Aggiorniamo l'array locale
        this.todoItems.set(
          this.todoItems().map(t => 
            t.id === todo.id ? { ...t, isDone: updatedTodo.isDone } : t
          )
        );
      },
      error: (err) => {
        console.error('Update error', err);
      }
    });
  }

  editTodoTitle(todo: todoItem) {
    this.editTodoId = todo.id;
  }

  // Salvataggio della modifica del titolo del ToDo
  saveTodoTitle(todo: todoItem) {
     //lo riprostino post modifica
    if (!todo.title.trim()) { // Se il titolo è vuoto (o solo spazi)
      alert('Please enter a valid activity')
      return;
  }
    this.todoService.updateToDoItem(todo).subscribe({
      next: () => {      
        this.editTodoId = null;
      },
      error: (err) => {
        console.error('Update error :', err);
      }
    });
    console.log(todo);
  
} 
}

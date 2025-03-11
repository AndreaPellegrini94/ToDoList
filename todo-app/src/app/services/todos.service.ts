import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { todoItem } from '../models/todo.types';
import { Observable } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class TodosService {
  
  http=inject(HttpClient);
  baseUrl = 'https://localhost:7264/api';

   getUserTodos(){
    const Url = this.baseUrl + '/ToDo';
    return this.http.get<Array<todoItem>>(Url);
   }

   addTodoTitle(todo: todoItem): Observable<todoItem> {
    const Url = this.baseUrl + `/ToDo/Add`
    const token = sessionStorage.getItem('authToken');  // Prendi il token da sessionStorage
    if (!token) {
      throw new Error('Token not found');
    }

    return this.http.post<todoItem>(Url, todo)
  }
   
  deleteTodoItem(todoid: number):Observable<void> {
    const Url = this.baseUrl + `/ToDo/Delete/${todoid}`;
    return this.http.delete<void>(Url)
  }

  updateToDoItem(todo: todoItem): Observable<void> {
    const Url = this.baseUrl + `/ToDo/Update/${todo.id}`;   
    return this.http.put<void>(Url, todo);
  }
}
  
  


import { Injectable } from '@angular/core';
import { ToDoItem } from '../models/todoitem';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable()
export class ToDoItemService {

  constructor(private httpClient: HttpClient) { }

  getToDoItems(): Observable<ToDoItem[]> {
    console.log("Get all to do items");
    return this.httpClient.get<ToDoItem[]>(`https://localhost:44314/api/ToDoItem`)
      .pipe(catchError(this.handleError('get to do item', null)));
  }
  getToDoItem(id: string): Observable<ToDoItem> {
    console.log("Get to do item");
    return this.httpClient.get<ToDoItem>(`https://localhost:44314/api/ToDoItem/${id}`)
    .pipe(catchError(this.handleError('get to do item', null)));
  }
  upsertToDoItem(item: ToDoItem): Observable<ToDoItem> {
    return this.httpClient.put<ToDoItem>(`https://localhost:44314/api/ToDoItem`, item)
      .pipe(catchError(this.handleError<ToDoItem>('upsert to do item', null)));
  }
  deleteToDoItem(id: string): Observable<any>{
    console.log("delete from the database");
    return this.httpClient.delete(`https://localhost:44314/api/ToDoItem/${id}`)
    .pipe(catchError(this.handleError<ToDoItem>('delete to do item', null)));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error); // log to console instead
      console.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}

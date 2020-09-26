import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { tap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { User } from 'src/app/models/user';
import { USERS_URL } from 'src/app/utils/config';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }

  public addUser(user: User): Observable<User> {
    return this.http.post<User>(USERS_URL, user, this.httpOptions).pipe(
      tap((newUser: User) => console.log(`added user w/ id=${newUser.id}`)),
      catchError(this.handleError<User>('addUser'))
    );
  }

  public getUser(id: number): Observable<User> {
    const url = `${USERS_URL}/${id}`;
    return this.http.get<User>(url).pipe(
      tap(_ => console.log(`fetched user id=${id}`)),
      catchError(this.handleError<User>(`get User id=${id}`))
    );
  }

  public deleteUser(id: number): Observable<User> {
    const url = `${USERS_URL}/${id}`;
    return this.http.delete<User>(url, this.httpOptions).pipe(
      tap(_ => console.log(`deleted user id=${id}`)),
      catchError(this.handleError<User>('deleteUser'))
    );
  }

  public updateUser(user: User): Observable<any> {
    return this.http.put(USERS_URL, user, this.httpOptions).pipe(
      catchError(this.handleError<any>('updateUser'))
    );
  }

  public resetPassword(email: string): Observable<User> {
    const url = `${USERS_URL}/resetPassword/${email}`;
    return this.http.get<User>(url, this.httpOptions).pipe(
      catchError(this.handleError<User>('reset password'))
    );
  }

  public uploadFile(formData, userId): Observable<any> {
    const url = `${USERS_URL}/upload/${userId}`;
    return this.http.post(url, formData).pipe(
      catchError(this.handleError('file error'))
    );
  }
}

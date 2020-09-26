import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { tap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { Power } from '../models/power';
import { POWERS_URL } from 'src/app/utils/config';

@Injectable({
  providedIn: 'root'
})
export class PowerService {

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }

  public getPowers(): Observable<Power[]> {
    return this.http.get<Power[]>(POWERS_URL)
      .pipe(
        tap(_ => console.log('feeeeetched powers')),
        catchError(this.handleError<Power[]>('getPowers', []))
      );
  }

  public isPowerValid(power): boolean {
    if (power.name === "" || power.mainTrait === "" || isNaN(power.strength)) {
      return false;
    }
    return true;
  }

  public addPower(power: Power): Observable<Power> {
    return this.http.post<Power>(POWERS_URL, power, this.httpOptions).pipe(
      tap((newPower: Power) => console.log(`added power w/ id=${newPower.id}`)),
      catchError(this.handleError<Power>('addPower'))
    );
  }

  public updatePower(power: Power): Observable<any> {
    return this.http.put(POWERS_URL, power, this.httpOptions).pipe(
      tap(_ => console.log(`updated power id=${power.id}`)),
      catchError(this.handleError<any>('updatePower'))
    );
  }

  public deletePower(power: Power | number): Observable<Power> {
    const id = typeof power === 'number' ? power : power.id;
    const url = `${POWERS_URL}/${id}`;

    return this.http.delete<Power>(url, this.httpOptions).pipe(
      tap(_ => console.log(`deleted power id=${id}`)),
      catchError(this.handleError<Power>('deletePower'))
    );
  }

  public getPowerByName(name): Observable<Power> {
    const url = `${POWERS_URL}/getPowerByName/${name}`;

    return this.http.get<Power>(url)
      .pipe(
        tap(_ => console.log('feeeeetched power by name')),
        catchError(this.handleError<Power>('getPowerByName'))
      );
  }
}

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { tap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { Hero } from '../models/hero';
import { ChartData } from '../models/chartData';
import { HEROES_URL } from 'src/app/utils/config';

@Injectable({
  providedIn: 'root'
})
export class HeroService {

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  public getUserDetails(username, password) {
    let user = { username, password };
    return this.http.post(HEROES_URL, user, this.httpOptions).pipe(
      catchError(this.handleError<JSON>('send user details'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }

  public getHero(id: number): Observable<Hero> {
    const url = `${HEROES_URL}/${id}`;
    return this.http.get<Hero>(url).pipe(
      tap(_ => console.log(`fetched hero id=${id}`)),
      catchError(this.handleError<Hero>(`getHero id=${id}`))
    );
  }

  public getHeroes(): Observable<Hero[]> {
    return this.http.get<Hero[]>(HEROES_URL)
      .pipe(
        tap(_ => console.log('feeeeetched heroes')),
        catchError(this.handleError<Hero[]>('getHeroes', []))
      );
  }

  public getHeroTypes(): Observable<Hero[]> {
    const url = `${HEROES_URL}/types`;
    return this.http.get<Hero[]>(url)
      .pipe(
        tap(_ => console.log('feeeeetched heroes')),
        catchError(this.handleError<Hero[]>('getHeroes', []))
      );
  }

  public isHeroValid(hero): boolean {
    if (!hero.name && !hero.mainPower) {
      return false;
    }
    return true;
  }

  public addHero(hero: Hero): Observable<Hero> {
    return this.http.post<Hero>(HEROES_URL, hero, this.httpOptions).pipe(
      catchError(this.handleError<Hero>('addHero'))
    );
  }

  public updateHero(hero: Hero): Observable<any> {
    return this.http.put(HEROES_URL, hero, this.httpOptions).pipe(
      catchError(this.handleError<any>('updateHero'))
    );
  }

  public deleteHero(id: number): Observable<Hero> {
    const url = `${HEROES_URL}/${id}`;
    return this.http.delete<Hero>(url, this.httpOptions).pipe(
      tap(_ => console.log(`deleted hero id=${id}`)),
      catchError(this.handleError<Hero>('deleteHero'))
    );
  }

  public getPowerFrequencies(): Observable<any> {
    const url = `${HEROES_URL}/frequencies`;
    return this.http.get(url)
      .pipe(
        tap(_ => console.log('feeeeetched powers frequencies')),
        catchError(this.handleError<Observable<any>>('getFrequencies'))
      );
  }

  public getHeroChartData(): Observable<ChartData> {
    const url = `${HEROES_URL}/heroChartData`;
    return this.http.get<ChartData>(url)
      .pipe(
        tap(_ => console.log('feeeeetched heroChartData')),
        catchError(this.handleError<any>('get heroChartData'))
      );
  }

  public uploadFile(formData, heroId) {
    const url = `${HEROES_URL}/upload/${heroId}`;
    return this.http.post(url, formData).pipe(
      catchError(this.handleError('file error'))
    );
  }
}
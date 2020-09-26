import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  private heroIdSource = new Subject<number>();
  currentHeroId = this.heroIdSource.asObservable();

  constructor() { }

  changeCurrentHeroId(heroId: number) {
    this.heroIdSource.next(heroId);
  }
}
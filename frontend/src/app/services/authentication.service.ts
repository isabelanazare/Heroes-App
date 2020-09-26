import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AUTH_URL } from 'src/app/utils/config';
import { User } from 'src/app/models/user';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private _currentUserSubject: BehaviorSubject<User>;
    public currentUser: Observable<User>;

    constructor(private http: HttpClient) {
        this._currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this._currentUserSubject.asObservable();
    }

    public get currentUserValue(): User {
        return this._currentUserSubject.value;
    }

    public login(email: string, password: string) {
        let user = {
            email: email,
            password: password
        }

        return this.http.post<any>(AUTH_URL, user)
            .pipe(map(user => {
                localStorage.setItem('currentUser', JSON.stringify(user));
                localStorage.setItem('userId', JSON.stringify(user.id));
                this._currentUserSubject.next(user);
                return user;
            }));
    }

    public logout() {
        localStorage.removeItem('currentUser');
        localStorage.removeItem('userId');
        this._currentUserSubject.next(null);
    }
}
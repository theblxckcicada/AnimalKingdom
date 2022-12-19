import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Subject, tap, throwError } from 'rxjs';
import { User } from './user.model';

export interface ResponseData {
  king: string;
  idToken: string;
  email: string;
  refreshToken: string;
  expiresIn: string;
  localId: string;
}
@Injectable({ providedIn: 'root' })
export class AuthService {
  authenticatedUser = new BehaviorSubject<User>(null);
  user: User;
  loggedIn: boolean = false;

  constructor(private http: HttpClient) {}

  isAuthenticated() {
    const promise = new Promise((resolve, reject) => {
      setTimeout(() => {
        resolve(this.loggedIn);
      }, 800);
    });
    console.log(promise);
    return promise;
  }
  login(email: string, password: string) {
    return this.http
      .post<ResponseData>(
        'https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyBTgqBln2niC3S4dp971at6Vu09j_nhCrY',
        {
          email: email,
          password: password,
          returnSecureToken: true,
        }
      )
      .pipe(
        catchError(this.handleError),
        tap((responseData: ResponseData) => {
          this.handleAuthentication(
            responseData.email,
            responseData.localId,
            responseData.idToken,
            +responseData.expiresIn
          );
        })
      );
  }
  signUp(email: string, password: string) {
    return this.http
      .post<ResponseData>(
        'https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=AIzaSyBTgqBln2niC3S4dp971at6Vu09j_nhCrY',
        {
          email: email,
          password: password,
          returnSecureToken: true,
        }
      )
      .pipe(
        catchError(this.handleError),
        tap((responseData: ResponseData) => {})
      );
  }

  private handleError(errorRes: HttpErrorResponse) {
    let errorMessage = 'Unknown error occured!!';
    console.log('Error occured in the handle error');
    if (!errorRes.error || !errorRes.error.error) {
      return throwError(errorMessage);
    }
    switch (errorRes.error.error.message) {
      case 'EMAIL_EXISTS':
        errorMessage = 'Email entered already exists';
        break;
      case 'INVALID_PASSWORD':
        errorMessage = 'Invalid password';
        break;
      case 'EMAIL_NOT_FOUND':
        errorMessage = 'User with that email does not exists';
        break;
    }
 
    return throwError(errorMessage);
  }
  private handleAuthentication(
    email: string,
    localId: string,
    idToken: string,
    expiresIn: number
  ) {
    const expiryDate = new Date(new Date().getTime() + expiresIn * 1000);
    const user = new User(email, localId, idToken, expiryDate);
    this.authenticatedUser.next(user);
    this.loggedIn = true;
    localStorage.setItem('user', JSON.stringify(user));
  }

  autoLogin() {
    const userData: {
      email: string;
      localId: string;
      idToken: string;
      expiresIn: string;
    } = JSON.parse(localStorage.getItem('user'));

    if (!userData) {
      return;
    }

    const loadedUser = new User(
      userData.email,
      userData.localId,
      userData.idToken,
      new Date(userData.expiresIn)
    );
    if (loadedUser.token) {
      this.loggedIn = true;
      this.authenticatedUser.next(loadedUser);
    }
  }

  logout() {
    this.loggedIn = false;
    this.user = null;
    this.authenticatedUser.next(this.user);
  }
}

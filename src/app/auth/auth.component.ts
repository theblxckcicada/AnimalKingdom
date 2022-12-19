import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService, ResponseData } from './auth-service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
})
export class AuthComponent implements OnInit {
  login: boolean = false;
  isLoading: boolean = false;
  form: FormGroup;
  error: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.form = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.required]),
    });
  }
  onBackToLogin(){
    this.error = null;
    this.isLoading = false;
  }
  onSubmit() {
    console.log('submiteed');
    if (!this.form.valid) {
      return;
    }

    let email = this.form.get('email').value;
    let password = this.form.get('password').value;
    let authObs: Observable<ResponseData>;
    this.isLoading = true;
    if (this.login) {
      
      authObs = this.authService.login(email, password);
    }
    if (!this.login) {
      authObs = this.authService.signUp(email, password);
    }

    authObs.subscribe(
      (resposeData) => {
        if (this.isLoading) {
          this.router.navigate(['/animal'], { relativeTo: this.route });
        }
        this.isLoading = false;
      },
      (error) => {
        console.log('Error occured');
        this.error = error;
        this.isLoading = false;
      }
    );

    this.form.reset();
  }
  onNoAccount() {
    this.login = false;
  }
  onHaveAccount() {
    this.login = true;
  }
}

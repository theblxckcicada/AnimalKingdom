import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AnimalComponent } from './animal/animal.component';
import { AnimalListComponent } from './animal/animal-list/animal-list.component';
import { AnimalDetailsComponent } from './animal/animal-details/animal-details.component';
import { AnimalAddComponent } from './animal/animal-add/animal-add.component';
import { HeaderComponent } from './header/header.component';
import { AnimalItemComponent } from './animal/animal-list/animal-item/animal-item.component';
import { AnimalService } from './animal/animal.service';
import { FirstPageComponent } from './first-page/first-page.component';
import { AppModuleRouting } from './app-module-routing';
import { DropdownDirective } from './animal/dropdown.directive';
import { FormsModule, NgModel, ReactiveFormsModule } from '@angular/forms';
import { AnimalEditComponent } from './animal/animal-edit/animal-edit.component';
import { AuthComponent } from './auth/auth.component';
import { AuthInterceptorService } from './auth/auth-interceptor.service';
import { AuthService } from './auth/auth-service';
import { AuthGuardService } from './auth/auth-guard.service';
import { AnimalGuardService } from './shared/animal-guard.service';
import { LoadingSpinner } from './shared/loading-spinner.component';

@NgModule({
  declarations: [
    AppComponent,
    AnimalComponent,
    AnimalListComponent,
    AnimalDetailsComponent,
    AnimalAddComponent,
    HeaderComponent,
    AnimalItemComponent,
    FirstPageComponent,
    DropdownDirective,
    AnimalEditComponent,
    AuthComponent,
    LoadingSpinner
  
  ],
  imports: [
    BrowserModule,
    AppModuleRouting,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [AnimalService,
    AnimalGuardService,  DropdownDirective, 
    AuthGuardService, {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }

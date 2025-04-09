import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnimalDetailsComponent } from './animal/animal-details/animal-details.component';
import { AnimalAddComponent } from './animal/animal-add/animal-add.component';
import { AnimalComponent } from './animal/animal.component';
import { FirstPageComponent } from './first-page/first-page.component';
import { AnimalEditComponent } from './animal/animal-edit/animal-edit.component';
import { AuthComponent } from './auth/auth.component';
import { AuthGuardService } from './auth/auth-guard.service';
import { AnimalGuardService } from './shared/animal-guard.service';
import { AnimalsResover } from './animal/animals.resolver';

const appRoutes: Routes = [
  {
    path: '',
    redirectTo: '/animal',
    pathMatch: 'full',
  },
  {
    path: 'animal',
    component: AnimalComponent,
    children: [
      { path: '', component: FirstPageComponent },
      {
        path: ':id',
        component: AnimalDetailsComponent,
        // resolve: [AnimalsResover],
      },
      {
        path: ':id/edit',
        component: AnimalEditComponent,
        canActivate: [AnimalGuardService],
      },
    ],
  },
  {
    path: 'add',
    component: AnimalAddComponent,
    canActivate: [AuthGuardService],
  },
  { path: 'auth', component: AuthComponent },
  { path: '**', redirectTo: '/animal' },
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule],
})
export class AppModuleRouting {
  constructor() {}
}

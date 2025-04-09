import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Resolve,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import { DataStorageService } from '../data-storage/data-storage.service';
import { Animal } from './animal.model';
import { AnimalService } from './animal.service';

@Injectable({ providedIn: 'root' })
export class AnimalsResover implements Resolve<Animal[]> {
  constructor(
    private dataStorageService: DataStorageService,
    private animalService: AnimalService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Animal[] | Observable<Animal[]> | Promise<Animal[]> {
    const recipes = this.animalService.getAllAnimals();
    if (recipes.length === 0) {
      return this.dataStorageService.fetchAnimalsData();
    } else {
      return recipes;
    }
  }
}

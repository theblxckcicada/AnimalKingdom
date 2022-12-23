import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, tap } from 'rxjs/operators';
import { Animal } from '../animal/animal.model';
import { AnimalService } from '../animal/animal.service';

@Injectable({ providedIn: 'root' })
export class DataStorageService {
  constructor(private http: HttpClient, private animalService: AnimalService) {}

  saveAnimalsData() {
    const animals: Animal[] = this.animalService.getAllAnimals();
    this.http
      .put(
        'https://ng-animal-project-default-rtdb.firebaseio.com/animals.json',
        animals
      )
      .subscribe((animals) => {});
  }

  fetchAnimalsData() {
    return this.http
      .get<Animal[]>('https://localhost:7027/api/Animals', {
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
        },
      })
      .pipe(
        tap((animals) => {
          //   animals.sort(() => (Math.random() > 0.5 ? 1 : -1));
          this.animalService.setAnimals(animals);
        })
      );
  }
}

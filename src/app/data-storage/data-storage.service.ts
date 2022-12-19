import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Animal } from "../animal/animal.model";
import { AnimalService } from "../animal/animal.service";




@Injectable({ providedIn: 'root' })
export class DataStorageService {

    constructor(private http: HttpClient, private animalService: AnimalService) { }

    saveAnimalsData() {
        const animals: Animal[] = this.animalService.getAllAnimals();
        this.http.put('https://ng-animal-project-default-rtdb.firebaseio.com/animals.json',
            animals
        ).subscribe(animals => {
            console.log(animals);
        });
    }

    fetchAnimalsData() {
        this.http.get<Animal[]>('https://ng-animal-project-default-rtdb.firebaseio.com/animals.json'
        ).subscribe( animals => {
            console.log(animals);
            this.animalService.setAnimals(animals);
        })
    }

}
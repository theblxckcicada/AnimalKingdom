import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth-service';
import { DataStorageService } from 'src/app/data-storage/data-storage.service';
import { Animal } from '../animal.model';
import { AnimalService } from '../animal.service';

@Component({
  selector: 'app-animal-list',
  templateUrl: './animal-list.component.html',
  styleUrls: ['./animal-list.component.css']
})
export class AnimalListComponent implements OnInit {
  animals: Animal[] = [];
  tempAnimals: Animal[] = [];
  animalCategories: string[];
  isAuthenticated:boolean = false;
  @Input() category: string = "All";

  constructor(private animalService: AnimalService, private router: Router,
    private dataStorageService: DataStorageService,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.dataStorageService.fetchAnimalsData();
    this.animals = this.animalService.getAllAnimals();
    this.tempAnimals = this.animalService.getAllAnimals();

    this.isAuthenticated = this.authService.loggedIn;

    this.animalService.animalsEmitter.subscribe((animals: Animal[]) => {
      if (animals !== null) {
        this.animals = animals;
        this.tempAnimals = animals;
        this.animalCategories = this.animalService.getAllCategories();
      }

    })


  }

  listByCategory(category: string) {
    const animalsByCat: Animal[] = [];
    this.animals = this.tempAnimals.slice();

    if (category === "All") {
      this.animals = this.tempAnimals.slice();
      this.category = category;
    } else {
      for (let animal of this.animals) {
        if (animal.category === category) {
          animalsByCat.push(animal);
        }
      }
      this.animals = animalsByCat;
      this.category = category + 's';
    }

  }
  onAddingNewAnimal() {
    this.router.navigate(['/add']);
  }
}

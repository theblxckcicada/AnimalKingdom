import { Component } from '@angular/core';
import { Animal } from '../animal/animal.model';
import { AnimalService } from '../animal/animal.service';

@Component({
  selector: 'app-first-page',
  templateUrl: './first-page.component.html',
  styleUrls: ['./first-page.component.css'],
})
export class FirstPageComponent {
  animals: Animal[] = [];
  constructor(private animalService: AnimalService) {}

  ngOnInit() {
    this.animals = this.animalService.getAllAnimals();
    this.animalService.animalsEmitter.subscribe((animals) => {
      this.animals = animals;
    });
  }
}

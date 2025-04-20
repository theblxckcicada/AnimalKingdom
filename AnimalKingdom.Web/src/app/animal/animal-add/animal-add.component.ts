import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DataStorageService } from 'src/app/data-storage/data-storage.service';
import { Animal } from '../animal.model';
import { AnimalService } from '../animal.service';

@Component({
  selector: 'app-animal-add',
  templateUrl: './animal-add.component.html',
  styleUrls: ['./animal-add.component.css'],
})
export class AnimalAddComponent {

  editable: boolean;
  animalNames = [];
  editableAnimal: Animal;
  form: FormGroup;

  constructor(
    private animalService: AnimalService,
    private dataStorageService: DataStorageService
  ) {}
  ngOnInit() {
    this.animalNames = this.animalService.getAllAnimalNames();
    this.form = new FormGroup({
      name: new FormControl(null, [
        Validators.required,
        this.nameValidator.bind(this),
      ]),
      category: new FormControl('Carnivore'),
      description: new FormControl(null, Validators.required),
      imagePath: new FormControl(
        null,
        Validators.required
      ),
    });

  }
  onAddingAnimal() {
    const animal = new Animal(
      this.form.get('name').value,
      this.form.get('category').value,
      this.form.get('description').value,
      this.form.get('imagePath').value
    );
    this.animalService.addAnimal(animal);
    this.dataStorageService.saveAnimalsData();

    this.form.reset();
  }


  nameValidator(control: FormControl): { [s: string]: boolean } {
    for (let name of this.animalNames) {
      if (control.value !== null) {
        if (control.value.toUpperCase() === name.toUpperCase()) {
          return { validatedName: false };
        }
      }
    }
    return null;
  }
}

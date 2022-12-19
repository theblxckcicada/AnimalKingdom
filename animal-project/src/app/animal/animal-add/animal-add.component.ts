import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DataStorageService } from 'src/app/data-storage/data-storage.service';
import { Animal } from '../animal.model';
import { AnimalService } from '../animal.service';

@Component({
  selector: 'app-animal-add',
  templateUrl: './animal-add.component.html',
  styleUrls: ['./animal-add.component.css'],
})
export class AnimalAddComponent {
  imageSrc: string;
  name: string;
  description: string;
  imagePath: string;
  category: string;
  editable: boolean;
  animalNames = [];
  editableAnimal: Animal;
  form: FormGroup;

  constructor(
    private animalService: AnimalService,
    private router: Router,
    private route: ActivatedRoute,
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
      imagePath: new FormControl(null, Validators.required),
    });
    this.imageValidator();
  }
  onAddingAnimal() {
    const animal = new Animal(
      this.form.get('name').value,
      this.form.get('category').value,
      this.form.get('description').value,
      this.form.get('imagePath').value
    );
    console.log(animal);
    this.animalService.addAnimal(animal);
    this.dataStorageService.saveAnimalsData();
    console.log(this.form);

    this.form.reset();
  }

  imageValidator(){
    setTimeout(()=>{
        this.imageSrc = this.form.get('imagePath').value;
    },1000);
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

import { Component, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Animal } from '../animal.model';
import { AnimalService } from '../animal.service';

@Component({
  selector: 'app-animal-edit',
  templateUrl: './animal-edit.component.html',
  styleUrls: ['./animal-edit.component.css'],
})
export class AnimalEditComponent {
  animalNames = [];
  animal: Animal;
  @ViewChild('form') form: NgForm;
  editable: boolean;
  imageSrc: string;

  constructor(
    private animalService: AnimalService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.formInit();
  }

  formInit() {
    this.animalService.editAnimal.subscribe(
      (animal) => {
        this.editable = true;
        this.animal = animal;
        console.log(this.editable);
        console.log(this.animal);

        this.imageSrc = this.animal.imagePath;

        // this.form.form.setValue({
        //   name: this.animal.name,
        //   category: this.animal.category,
        //   description: this.animal.description,
        //   imagePath: this.animal.imagePath,
        // });

        console.log(this.form);
      }
    );
  }

  onAddingAnimal() {
    this.animalService.addAnimal(
      new Animal(
        this.form.value.name,
        this.form.value.category,
        this.form.value.description,
        this.form.value.imagePath
      )
    );
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

import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription, take } from 'rxjs';
import { DataStorageService } from 'src/app/data-storage/data-storage.service';
import { Animal } from '../animal.model';
import { AnimalService } from '../animal.service';

@Component({
  selector: 'app-animal-edit',
  templateUrl: './animal-edit.component.html',
  styleUrls: ['./animal-edit.component.css'],
})
export class AnimalEditComponent implements OnInit, OnDestroy {
  animalNames = [];
  animal: Animal = null;
  form: FormGroup;
  editable: boolean;
  imageSrc: string;
  subscription: Subscription;

  constructor(
    private animalService: AnimalService,
    private router: Router,
    private route: ActivatedRoute,
    private dataStorageService: DataStorageService
  ) {}

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      category: new FormControl('Carnivore', [Validators.required]),
      description: new FormControl(null, [Validators.required]),
      imagePath: new FormControl(null, [Validators.required]),
    });
    this.formInit();
    console.log(this.animal);
  }

  formInit() {
    this.subscription = this.route.queryParams.subscribe((params) => {
      this.animal = this.animalService.getAnimalById(params[0]);
      this.editable = true;
      this.imageSrc = this.animal.imagePath;
      this.form.setValue({
        name: this.animal.name,
        category: this.animal.category,
        description: this.animal.description,
        imagePath: this.animal.imagePath,
      });
    });
  }

  onEditingAnimal() {
    console.log('starting editing')
    this.animalService.updateAnimal(
      new Animal(
        this.form.value.name,
        this.form.value.category,
        this.form.value.description,
        this.form.value.imagePath
      )
    );
    this.dataStorageService.saveAnimalsData();
    this.router.navigate(['../'], { relativeTo: this.route });
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

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}

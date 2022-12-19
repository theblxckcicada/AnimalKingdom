import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth-service';
import { Animal } from '../animal.model';
import { AnimalService } from '../animal.service';

@Component({
  selector: 'app-animal-details',
  templateUrl: './animal-details.component.html',
  styleUrls: ['./animal-details.component.css'],
})
export class AnimalDetailsComponent implements OnInit {
  animal: Animal = null;
  id: string;
  isAuthenticated: boolean =false;
  constructor(
    private animalService: AnimalService,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params['id'];
      this.animal = this.animalService.getAnimalById(this.id);
     this.isAuthenticated =  this.authService.loggedIn;
    });
  }
  onEditAnimal() {
    this.router.navigate(['edit'], { relativeTo: this.route });
    this.animalService.editAnimal.next(this.animalService.getAnimalById(this.id));
  }

  onDeleteAnimal() {
    this.animalService.deleteAnimal(this.animal);
    this.router.navigate(['../'], { relativeTo: this.route });
  }
}

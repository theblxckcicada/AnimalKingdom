import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AnimalService } from '../animal/animal.service';
import { AuthService } from '../auth/auth-service';
import { DataStorageService } from '../data-storage/data-storage.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  isAuthenticated = false;
  subscription: Subscription;
  animalFilter: string = '';
  form: FormGroup;

  constructor(
    private router: Router,
    private dataStorageService: DataStorageService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private animalService: AnimalService
  ) {}

  ngOnInit() {
    this.form = new FormGroup({
      'text': new FormControl(
        this.animalFilter,
        this.AnimalValidator.bind(this)
      ),
    });

    this.subscription = this.authService.authenticatedUser.subscribe((user) => {
      this.isAuthenticated = !!user;
    });
  }

  AnimalValidator() {
    const promise = new Promise((resolve, reject) =>
      setTimeout(() => {
        this.animalFilter = this.form.get('text').value;
        this.onAnimalSearch();
      }, 200)
    );
  }
  onAnimalSearch() {
    this.animalService.animalFilterWord.next(this.animalFilter);
  }
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/auth'], { relativeTo: this.route });
  }
  navigateBackToAnimals() {
    this.router.navigate(['animal']);
  }

  saveData() {
    this.dataStorageService.saveAnimalsData();
  }

  fetchData() {
    this.dataStorageService.fetchAnimalsData().subscribe();
  }
}

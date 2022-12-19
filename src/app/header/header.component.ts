import { Component, OnInit } from '@angular/core';
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

  constructor(
    private router: Router,
    private dataStorageService: DataStorageService,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.subscription = this.authService.authenticatedUser.subscribe((user) => {
      this.isAuthenticated = !!user;
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/auth'], {relativeTo: this.route});
  }
  navigateBackToAnimals() {
    this.router.navigate(['animal']);
  }

  saveData() {
    this.dataStorageService.saveAnimalsData();
  }

  fetchData() {
    this.dataStorageService.fetchAnimalsData();
  }
}

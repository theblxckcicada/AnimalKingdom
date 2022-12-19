import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Animal } from 'src/app/animal/animal.model';
import { AnimalService } from '../../animal.service';

@Component({
  selector: 'app-animal-item',
  templateUrl: './animal-item.component.html',
  styleUrls: ['./animal-item.component.css']
})
export class AnimalItemComponent implements OnInit {
  @Input() animal: Animal = null;
  @Input() index: string;

  constructor(private animalService: AnimalService, private route: ActivatedRoute, private router: Router) { }

  onSelectAnimal() {
    this.animalService.animalEmitter.next(this.animal);
  }
  ngOnInit(): void {
    this.index = this.animal.name;
  }
}

import { Pipe, PipeTransform } from '@angular/core';
import { Animal } from '../animal/animal.model';

@Pipe({
  name: 'search',
})
export class SearchPipe implements PipeTransform {
  transform(animals: Animal[], animalFilter: string): any {
    if (animals.length === 0 || animalFilter === '') {
      return animals;
    }

    const resultArray = [];
    for (const animal of animals) {
      if (animal.name.toUpperCase().includes(animalFilter.toUpperCase())) {
        resultArray.push(animal);
      }
    }
    return resultArray;
  }
}

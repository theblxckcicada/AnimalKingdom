import { EventEmitter, Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { DataStorageService } from "../data-storage/data-storage.service";
import { Animal } from "./animal.model";


@Injectable({providedIn: 'root'})
export class AnimalService {
    // animals: Animal[] = [

    //     new Animal("Tiger", "Carnivore", "The tiger is the largest living cat species and a member of the genus Panthera." +
    //         "It is most recognisable for its dark vertical stripes on orange fur with a white underside. " +
    //         "An apex predator, it primarily preys on ungulates, such as deer and wild boar.",
    //         "https://images.pexels.com/photos/47312/tiger-predator-animal-tooth-47312.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"),

    //     new Animal("Elephant", "Herbivore", "Elephants are the largest existing land animals. Three living species are currently recognised: " +
    //         "the African bush elephant, the African forest elephant, and the Asian elephant." +
    //         "They are the only surviving members of the family Elephantidae and the order Proboscidea",
    //         "https://images.pexels.com/photos/1054666/pexels-photo-1054666.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"),
    //     new Animal("Python", "Carnivore", "Python snakes are Large in size and powerful. They can kill any living animal or human by squeezing." +
    //         "They have a triangular head, sharp teeth, and prehensile tails. Their teeth are backward curving.",
    //         "https://arc-anglerfish-washpost-prod-washpost.s3.us-east-1.amazonaws.com/public/V6X3A4DGSQZG3EZA2VLAHMXROA_size-normalized.jpg"),
    //     new Animal("Peacock", "Omnivore", "A peacock is a shiny blue bird who fans out his large colorful iridescent tail feathers, " +
    //         "especially when he's flirting with the peahens. A peacock is a male peafowl. A male peacock is more flamboyant than his" +
    //         "female counterpart — he's the one with those long brilliant tail feathers marked with eye-like designs.",
    //         "https://images.pexels.com/photos/45911/peacock-plumage-bird-peafowl-45911.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1")
    //     ,
    //     new Animal("Brown White Spider", "Carnivore", "icrohabitats and are commonly found around houses. The egg sac is distinct with spikes." +
    //         "They\'re brown with banded legs, and their abdomen varies from cream to black. They\'re generally inoffensive and will escape " +
    //         "into their retreat. If disturbed they\'ll retreat, or they will fall to the ground but will bite when picked up. Spiders only" +
    //         "bite when hurt.Their venom is neurotoxic, and bites will be very painful. If bitten, symptoms can include nausea, sweating," +
    //         "disorientation and shortness of breath. You should seek immediate medical assistance at a hospital with a poison unit, " +
    //         "especially in the case of children.  The venom is less lethal than that of Black button spiders.",
    //         "https://images.pexels.com/photos/68186/jumping-spider-tarantula-bird-spider-insect-68186.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1")
    //     ,
    //     new Animal("Giraffe", "Herbivore", "The giraffe is a large African hoofed mammal belonging to the genus Giraffa. " +
    //         "It is the tallest living terrestrial animal and the largest ruminant on Earth." +
    //         "Traditionally, giraffes were thought to be one species, Giraffa camelopardalis, with nine subspecies.",
    //         "https://images.pexels.com/photos/6790746/pexels-photo-6790746.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1")
    // ]

    animals: Animal[] = [];
    animalEmitter = new Subject<Animal>();
    animalsEmitter = new Subject<Animal[]>();
    categories: string[] = ['All'];
    id: number;
    isValid: boolean = false;
    animalIsEditable = new Subject<boolean>();
    editableAnimal = new Subject<Animal>();
    editAnimal = new Subject<Animal>();

    // constructor(private dataStorageService: DataStorageService) { }



    addAnimal(animal: Animal) {
        for (let an of this.animals) {
            if (an.name !== animal.name) {
                if ((this.animals.indexOf(an) + 1) == this.animals.length) {
                    this.animals.push(animal);
                }
            }
          
        }
        console.log('animal added');
        console.log(animal);
        // this.dataStorageService.saveAnimalsData();
        this.animalsEmitter.next(this.animals);
    }
    setAnimals(animals: Animal[]) {
        this.animals = animals;
        this.animalsEmitter.next(this.animals);
    }

    getAllAnimals() {
        this.animals.sort(() => (Math.random() > .5 ? 1 : -1));
        return this.animals;
    }

    getAllAnimalNames() {
        const animalNames = [];
        for (let animal of this.animals) {
            animalNames.push(animal.name);
        }

        return animalNames;
    }
    getAllCategories() {
        const cats = [];
        for (let animal of this.animals) {
            cats.push(animal.category);
        }

        cats.forEach(element => { if (!this.categories.includes(element)) { this.categories.push(element) } });
        return this.categories;
    }

    getAnimalsByCategory(category: string) {
        const animalsByCat: Animal[] = [];

        for (let animal of this.animals) {
            if (animal.category === category) {
                animalsByCat.push(animal);
            }
        }

        return animalsByCat;
    }

    getAnimalById(id: string) {
        for (let animal of this.animals) {
            if (animal.name === id) {
                return animal;
            }
        }
    }
    deleteAnimal(animal: Animal) {
        for (let an of this.animals) {
            if (animal.name.toUpperCase() === an.name.toUpperCase()) {
                this.animals.splice(this.animals.indexOf(an), 1);
            }
        }
        this.animalsEmitter.next(this.animals);
    }

}
import { Directive, HostBinding, HostListener } from "@angular/core";

@Directive({
    selector: "[dropdownDirective]"
})

export class DropdownDirective {

    @HostBinding('class.open') isOpen = false;

    @HostListener('click') dropdownOpen() {
        this.isOpen = !this.isOpen;
    }
}
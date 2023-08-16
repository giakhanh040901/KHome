import { Component } from '@angular/core';

@Component({
    selector: 'app-root',
    template: `
        <router-outlet></router-outlet>
        <spinner-loading></spinner-loading>
    `
})
export class RootComponent {

}

import { Component, Input, OnInit } from '@angular/core';

@Component({
	selector: 'e-loading',
	styleUrls: ['./e-loading.component.scss'],
	template: `
    <div class="e-loading p-component-overlay" [style]="style" [style.background]="background">
      <i class="pi pi-spin pi-spinner" [style.font-size.rem]="2"></i>
    </div>
  `
})
export class ELoadingComponent {
	constructor() {}

	@Input() style: any;
	@Input() background: string = '';
}

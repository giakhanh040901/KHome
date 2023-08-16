import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'e-loading',
  styleUrls: ['./e-loading.component.scss'],
  template: `
    <div class="e-loading p-component-overlay">
      <i class="pi pi-spin pi-spinner" [style.font-size.rem]="2"></i>
    </div>
  `
})
export class ELoadingComponent {}

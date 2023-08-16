import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'form-steps',
  templateUrl: './form-steps.component.html',
  styleUrls: ['./form-steps.component.scss'],
})
export class FormStepsComponent implements OnInit {
  @Input()
  public stepsItems: MenuItem[] = [];
  @Input()
  public readonly: boolean = true;
  @Input()
  public activeIndex: number = 0;
  @Output()
  public activeIndexChange: EventEmitter<number> = new EventEmitter<number>();

  constructor() {}

  ngOnInit() {}

  public onActiveIndexChange(event: any) {
    this.activeIndex = event;
    this.activeIndexChange.emit(this.activeIndex);
  }
}

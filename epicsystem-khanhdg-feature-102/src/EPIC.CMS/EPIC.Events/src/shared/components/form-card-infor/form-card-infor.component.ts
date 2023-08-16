import { Component, Input, OnInit } from '@angular/core';
import { IImage } from '@shared/interface/InterfaceConst.interface';

@Component({
  selector: 'form-card-infor',
  templateUrl: './form-card-infor.component.html',
  styleUrls: ['./form-card-infor.component.scss'],
})
export class FormCardInforComponent implements OnInit {
  @Input()
  public bgIcon: string = '';
  @Input()
  public icon: IImage;
  @Input()
  public colorLabel: string = '';
  @Input()
  public label: string = '';
  @Input()
  public colorValue: string = '';
  @Input()
  public value: string = '';

  constructor() {}

  ngOnInit() {}
}

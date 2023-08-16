import { Component, Input, OnInit } from '@angular/core';
import { IActionTable } from '@shared/interface/InterfaceConst.interface';

@Component({
  selector: 'form-menu',
  templateUrl: './form-menu.component.html',
  styleUrls: ['./form-menu.component.scss'],
})
export class FormMenuComponent implements OnInit {
  @Input()
  public classContainer: string = '';
  @Input()
  public icon: string = 'pi pi-ellipsis-h';
  @Input()
  public classMenu: string = '';
  @Input()
  public actions: IActionTable[] = [];

  constructor() {}

  ngOnInit() {}
}

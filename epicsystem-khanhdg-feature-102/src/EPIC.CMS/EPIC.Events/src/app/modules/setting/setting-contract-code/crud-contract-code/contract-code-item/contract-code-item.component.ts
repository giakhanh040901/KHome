import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SettingContractCode } from '@shared/AppConsts';
import { ContractCodeItem } from '@shared/interface/setting/setting-contract-code/ContractCode.model';

@Component({
  selector: 'contract-code-item',
  templateUrl: './contract-code-item.component.html',
  styleUrls: ['./contract-code-item.component.scss'],
})
export class ContractCodeItemComponent implements OnInit {
  @Input()
  public index: number = 0;
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public dataSource: ContractCodeItem = new ContractCodeItem();
  @Output()
  public dataSourceChange: EventEmitter<ContractCodeItem> = new EventEmitter<ContractCodeItem>();
  @Output()
  public _onRemove: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();
  constructor() {}

  public get listValueContractCode() {
    return SettingContractCode.listValueContractCode;
  }

  SettingContractCode = SettingContractCode;

  ngOnInit() {}

  public onChange(event: any) {
    if (event) {
      this.dataSourceChange.emit(this.dataSource);
      this._onChange.emit(event);

    }
  }

  public remove(event: any) {
    if (event) {
      this._onRemove.emit(event);
    }
  }
}

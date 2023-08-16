import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { EConfigDataModal, FormNotificationConst } from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { KeyConfigContractCodeDetail } from '@shared/consts/config-contract-code.const';
import { CrudComponentBase } from '@shared/crud-component-base';
import { INotiDataModal } from '@shared/interface/InterfaceConst.interface';
import {
  ContractCodeItem,
  CreateOrEditContractCode,
} from '@shared/interface/setting/setting-contract-code/ContractCode.model';
import { SettingContractCodeService } from '@shared/services/setting-contract-code-service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'crud-contract-code',
  templateUrl: './crud-contract-code.component.html',
  styleUrls: ['./crud-contract-code.component.scss'],
})
export class CrudContractCodeComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private ref: DynamicDialogRef,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig,
    private settingContractCodeService: SettingContractCodeService
  ) {
    super(injector, messageService);
  }
  public crudDTO: CreateOrEditContractCode = new CreateOrEditContractCode();
  public type: string = EConfigDataModal.CREATE;

  dataSeedKey = {
    [KeyConfigContractCodeDetail.ORDER_ID]: 999,
    [KeyConfigContractCodeDetail.INVEST_DATE]: "14/02/2023",
    [KeyConfigContractCodeDetail.EVENT_CODE]: "MSK",
    [KeyConfigContractCodeDetail.ORDER_ID_PREFIX_0]: "EV007",
}
  ngOnInit() {
    if (this.config.data) {
      this.type = this.config.data.type;
    }
    if (this.type === EConfigDataModal.CREATE) {
      this.crudDTO.contractCodeItem = [new ContractCodeItem()];
    } else {
      this.crudDTO.mapData(this.config.data.dataSource);
    }
  }

  public get isDisable() {
    return this.type === EConfigDataModal.VIEW;
  }

  public add(event: any) {
    if (event) {
      this.crudDTO.contractCodeItem.push(new ContractCodeItem());
    }
  }

  public test(){
    console.log('siu');
    
  }
  public remove(event: any, index: number) {
    if (event) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Thông báo',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: 'Bạn có chắc chắn muốn xóa giá trị?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.crudDTO.contractCodeItem.splice(index, 1);
        }
      });
    }
  }

  genContractCodeStructure() {    
    this.crudDTO.contractCode = '';
    this.crudDTO.contractCodeStructure = '';
    this.crudDTO.contractCodeItem.forEach(element => {
      console.log('!!! element ', element);
      
        this.crudDTO.contractCodeStructure += '<'+ (element?.value ? element.value : element?.value) +'>';
        this.crudDTO.contractCode += element?.character || this.dataSeedKey[element?.value];

    });
  }

  public save(event?: any) {
    if (event) {
      console.log('crudDTO ', this.crudDTO);
      
      if (this.crudDTO.isValidData()) {
        this.settingContractCodeService
          .createOrEdit(this.crudDTO.toObjectSendToAPI(), this.type === EConfigDataModal.EDIT)
          .subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, '')) {
                this.ref.close(response);
              }
            },
            (err) => {}
          );
      } else {
        this.messageDataValidator(this.crudDTO.dataValidator);
      }
    }
  }

  public close(event: any) {
    this.ref.close();
  }
}

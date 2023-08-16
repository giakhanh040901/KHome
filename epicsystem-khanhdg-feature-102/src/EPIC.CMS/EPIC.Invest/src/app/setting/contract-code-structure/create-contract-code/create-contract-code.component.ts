import { Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService, ConfirmationService } from 'primeng/api';
import { AtributionConfirmConst, ContractFormConst, YesNoConst } from '@shared/AppConsts';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ContractFormService } from '@shared/services/contract-form.service';

@Component({
  selector: 'app-create-contract-code',
  templateUrl: './create-contract-code.component.html',
  styleUrls: ['./create-contract-code.component.scss'],
})
export class CreateContractCodeComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _contractFormService: ContractFormService,
    private dialogService: DialogService,
    public configDialog: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    public confirmationService: ConfirmationService,
  ) {
    super(injector, messageService);
  }
    
    YesNoConst = YesNoConst;
    ContractFormConst = ContractFormConst;

    contractForm: any = {
        'status': null,
        'name': null,   
        'configContractCodeDetails' : [],
        'contractCode': 'EPIC/20220222/RongVang',
    }

    policyTempId: number;

    blockText: RegExp = /[0-9,.]/;
    submitted: boolean;
    isContractTemp: boolean;

    isCollapse = false;
    configContractCodeDetails: any = [];

    ngOnInit(): void {
        if( this.configDialog?.data?.contractCodeStructure) {
            this.contractForm = this.configDialog?.data?.contractCodeStructure;
            this.configContractCodeDetails = this.contractForm?.configContractCodeDetails
        }
        //
        this.genContractCodeStructure();
    }

    genContractCodeStructure() {
        this.contractForm.contractCodeStructure = '';
        this.configContractCodeDetails.forEach(element => {
            this.contractForm.contractCodeStructure += '<'+ (element?.value ? element.value : element?.key) +'>';
        });
    }

    addvalue() {
        // this.index = this.index +1
        this.configContractCodeDetails.push({ configContractCodeId: 0});
    }

    removeElement(index) {
        this.confirmationService.confirm({
            message: 'Bạn chắc chắn xóa giá trị này?',
            ...AtributionConfirmConst,
            accept: () => {
                this.configContractCodeDetails.splice(index, 1);
                this.genContractCodeStructure();
            }
        });
    }

    collapse() {
        this.isCollapse = !this.isCollapse;
    }

    save() {
        let index = 0;
        this.configContractCodeDetails = this.configContractCodeDetails.map(configContractCodeDetail => {
            configContractCodeDetail.sortOrder =  index++;
            return configContractCodeDetail;
        });
        
        this.contractForm.configContractCodeDetails = this.configContractCodeDetails;
        this.submitted = true;
        if (this.contractForm.id) {
            let body = {...this.contractForm};
            this._contractFormService.update(body).subscribe((response) => {
                if (this.handleResponseInterceptor(response)) {
                    this.ref.close({ data: response, accept: true });
                } 
                    this.submitted = false;
                }, (err) => {
                    this.submitted = false;
                }
            );
        } else {
            this._contractFormService.create(this.contractForm).subscribe((response) => {
                if (this.handleResponseInterceptor(response)) {
                    this.ref.close({ data: response, accept: true });
                }
                    this.submitted = false;
                }, (err) => {
                    this.submitted = false;
                }
            );
        }
    }

    close() {
        this.ref.close();
    }
}



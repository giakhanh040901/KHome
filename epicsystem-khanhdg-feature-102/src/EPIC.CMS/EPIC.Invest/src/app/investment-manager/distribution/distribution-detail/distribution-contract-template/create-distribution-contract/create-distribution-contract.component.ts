import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { ActiveDeactiveConst, ContractFormConst, DistributionContractTemplateConst, SampleContractConst, YesNoConst } from '@shared/AppConsts';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ContractFormService } from '@shared/services/contract-form.service';
import { DistributionService } from '@shared/services/distribution.service';
import { SampleContractService } from '@shared/services/sample-contract.service';

@Component({
  selector: 'app-create-distribution-contract',
  templateUrl: './create-distribution-contract.component.html',
  styleUrls: ['./create-distribution-contract.component.scss'],
  providers: [DialogService]
})
export class CreateDistributionContractComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private _contractFormService: ContractFormService,
        public configDialog: DynamicDialogConfig,
        public ref: DynamicDialogRef,
        private _distributionService: DistributionService,
        private _sampleContractService: SampleContractService,
    ) {
        super(injector, messageService);
    }

    // CONST
    YesNoConst = YesNoConst;
    ContractFormConst = ContractFormConst;
    SampleContractConst = SampleContractConst;
    DistributionContractTemplateConst = DistributionContractTemplateConst;

    contractDistributionTemplate: any = {
        distributionId: null,
        policyId: null,
        policyIds: null,
    }

    fieldDates = ['startDate', 'endDate'];
    submitted: boolean;
    
    policies = [];
    contractCodeTemps = [];
    contractTemplates = [];

    distributionId: any;
    contractTemplateChoose: any = {};

    ngOnInit(): void {
        this.distributionId = this.configDialog?.data?.distributionId;
        if(this.configDialog?.data?.contractDistributionTemplate) {
            this.contractDistributionTemplate = this.configDialog?.data?.contractDistributionTemplate;
            this.formatCalendarDisplay(this.fieldDates, this.contractDistributionTemplate);
            this.contractTemplateChoose = this.contractDistributionTemplate.contractTemplateTemp
        }
        
        if(this.distributionId ) {
            this.isLoading = true;
            forkJoin([
                this._distributionService.getAllPolicyActive(this.distributionId, ActiveDeactiveConst.ACTIVE), 
                this._contractFormService.getAllNoPermisson(),
                this._sampleContractService.getAllNoPermisson(this.contractDistributionTemplate?.contractSource)
            ]).subscribe(([resPolicy, resContractCode, resContract]) => {
                this.isLoading = false;
                // Danh sách chính sách
                if (this.handleResponseInterceptor(resPolicy, "")) {
                this.policies = resPolicy?.data.map(item => {
                    item.labelName = item.code + ' - ' + item.name;
                    return item;
                });
                }
                // Danh sách mẫu cấu trúc hợp đồng
                if (this.handleResponseInterceptor(resContractCode, "")) {
                this.contractCodeTemps = resContractCode?.data;
                }
                //Danh sách mẫu hợp đồng
                if (this.handleResponseInterceptor(resContract, "")) {
                    this.contractTemplates = resContract?.data?.map(item => {
                        item.labelName = item.description ? item.name + " - " + item.description : item.name;
                        return item;
                    });
                }
            }, (err) => {
                console.log('err__', err);
            });   
        }
    }

    changeContractSource(source) {
        this.getSampleContract()
    }

    changeContractTemplateTemp(contractTemplateTempId) {
        this.contractTemplateChoose = this.contractTemplates.find(c => c.id == contractTemplateTempId);
    }

    getSampleContract() {
        this.isLoading = true;
        this._sampleContractService.getAllNoPermisson(this.contractDistributionTemplate?.contractSource).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.contractTemplates = res?.data?.map(item => {
                    item.labelName = item.description ? item.name + " - " + item.description : item.name;
                    return item;
                });
            }
        }, (err) => {
            this.isLoading = false;
        });
    }

    save() {
        if(this.validForm()) {
            this.submitted = true;
            this.isLoading = true;
            let body = this.formatCalendar(this.fieldDates,this.contractDistributionTemplate);
            if (this.contractDistributionTemplate.id) {
                this._contractFormService.updateContractForm(body).subscribe((response) => {
                    if (this.handleResponseInterceptor(response)) {
                        this.messageSuccess('Cập nhật thành công!');
                        this.ref.close(true);
                    } 
                    this.submitted = false;
                    this.isLoading = false;
                }, (err) => {
                    this.submitted = false;
                    this.isLoading = false;
                });
            } else {
                this._contractFormService.createContractForm(body).subscribe((response) => {
                    if (this.handleResponseInterceptor(response)) {
                        this.messageSuccess('Thêm thành công!');
                        this.ref.close(true);
                        this.submitted = false;
                    } 
                    this.submitted = false;
                }, (err) => {
                    this.submitted = false;
                });
            }
        } else {
            this.messageError('Vui lòng nhập dữ liệu cho các trường có dấu (*)');
        }
    }

    validForm() {
        return ((!this.contractDistributionTemplate?.id && this.contractDistributionTemplate?.policyIds) || (this.contractDistributionTemplate?.id && this.contractDistributionTemplate?.policyId))
                && this.contractDistributionTemplate?.contractSource 
                && this.contractDistributionTemplate?.contractTemplateTempId
                && this.contractDistributionTemplate?.configContractId
                && this.contractDistributionTemplate?.displayType
                && this.contractDistributionTemplate?.startDate;
    }

    close() {
        this.ref.close();
    }
}



import { Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ConfirmationService, MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { ActiveDeactiveConst, ContractFormConst, DistributionContractTemplateConst, FormNotificationConst, SampleContractConst, YesNoConst } from '@shared/AppConsts';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ContractFormService } from '@shared/services/contract-form.service';
import { DistributionService } from '@shared/services/distribution.service';
import { SampleContractService } from '@shared/services/sample-contract.service';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';

@Component({
  selector: 'app-create-multiple-distribution-contract-template',
  templateUrl: './create-multiple-distribution-contract-template.component.html',
  styleUrls: ['./create-multiple-distribution-contract-template.component.scss'],
  providers: [DialogService, ConfirmationService, MessageService],
})
export class CreateMultipleDistributionContractTemplateComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private _contractFormService: ContractFormService,
        public configDialog: DynamicDialogConfig,
        public ref: DynamicDialogRef,
        private dialogService: DialogService,
        private _distributionService: DistributionService,
        private _sampleContractService: SampleContractService,
        public confirmationService: ConfirmationService,
    ) {
        super(injector, messageService);
    }

    // CONST
    YesNoConst = YesNoConst;
    ContractFormConst = ContractFormConst;
    SampleContractConst = SampleContractConst;
    DistributionContractTemplateConst = DistributionContractTemplateConst;

    distributionPolicyContractUploads: any = [];
    contractDistributionTemplate: any = {
        distributionId: null,
        policyId: null,
        policyIds: null,
        chooseContract: true,
        distributionPolicyContractUploads: [],
    }

    fieldDates = ['startDate', 'endDate'];
    submitted: boolean;
    
    policies = [];
    contractCodeTemps = [];
    contractTemplates = [];

    distributionId: number;
    contractTemplateChoose: any = {};

    public changeChooseContract(chooseContract) {
        if(chooseContract) {
            delete this.distributionPolicyContractUploads;
            delete this.contractDistributionTemplate.quantity;
        } else {
            delete this.contractDistributionTemplate.contractTemplateTempIds;
        }
    }

    public onFileInputChange(event, i, isInvestor: boolean = false) {
        const files: File[] = event.files;
        if (files && files.length > 0) {
            const file: File = files[0];
            console.log("event",file?.name);
            if (isInvestor) {
                this.distributionPolicyContractUploads[i].investorFile = file;
                this.distributionPolicyContractUploads[i].investorFileDisplay = file?.name;
            } else {
                this.distributionPolicyContractUploads[i].businessCustomerFile = file;
                this.distributionPolicyContractUploads[i].businessCustomerFileDisplay = file?.name;
            }
        }
    }

    public changeQuantity(quantity) {
        if (quantity) {
            if (this.distributionPolicyContractUploads?.length < quantity) {
                let length = quantity - this.distributionPolicyContractUploads?.length
                for (let i = 0; i < length; i++) {
                    this.distributionPolicyContractUploads.push({ contractType: 1 });
                }
            } else if (this.distributionPolicyContractUploads?.length > quantity) {
                let lengthCut = quantity - this.distributionPolicyContractUploads?.length
                this.distributionPolicyContractUploads = this.distributionPolicyContractUploads.slice(0, lengthCut)
            }
        }
    }

    public removeElement(index) {
        console.log("index", index);
        const ref = this.dialogService.open(FormNotificationComponent, {
			header: "Thông báo",
			width: "600px",
			data: {
				title: "Bạn có chắc chắn muốn xóa hợp đồng này?",
				icon: FormNotificationConst.IMAGE_CLOSE,
			},
		}).onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
                this.distributionPolicyContractUploads.splice(index, 1);
                this.contractDistributionTemplate.quantity -= 1;
			} else {
				this.messageError(this.AppConsts.messageError);
			}
		});
    }

    ngOnInit(): void {
        this.distributionId = this.configDialog?.data?.distributionId;
        if (this.configDialog?.data?.contractDistributionTemplate) {
            this.contractDistributionTemplate = this.configDialog?.data?.contractDistributionTemplate;
            this.contractDistributionTemplate.chooseContract = (this.configDialog?.data?.contractDistributionTemplate?.contractTemplateTempId > 0);
            this.contractDistributionTemplate.contractTemplateTempIds = [this.configDialog?.data?.contractDistributionTemplate?.contractTemplateTempId];
           if(this.configDialog?.data?.contractDistributionTemplate?.fileUploadName) {
               this.distributionPolicyContractUploads = [
                                                           {
                                                               contractType: this.configDialog?.data?.contractDistributionTemplate?.contractType,
                                                               distributionPolicyUploadName: this.configDialog?.data?.contractDistributionTemplate?.fileUploadName,
                                                               investorFile: this.configDialog?.data?.contractDistributionTemplate?.fileUploadInvestorUrl,
                                                               businessCustomerFile: this.configDialog?.data?.contractDistributionTemplate?.fileUploadBusinessCustomerUrl,
                                                               investorFileDisplay: this.configDialog?.data?.contractDistributionTemplate?.fileUploadInvestorUrl,
                                                               businessCustomerFileDisplay: this.configDialog?.data?.contractDistributionTemplate?.fileUploadBusinessCustomerUrl,
                                                           }
                                                       ];
                                                       
           }
            this.formatCalendarDisplay(this.fieldDates, this.contractDistributionTemplate);
            this.contractTemplateChoose = this.contractDistributionTemplate.contractTemplateTemp
        }

        if (this.distributionId) {
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

    public changeContractSource(source) {
        this.getSampleContract()
    }

    public changeContractTemplateTemp(contractTemplateTempId) {
        this.contractTemplateChoose = this.contractTemplates.find(c => c.id == contractTemplateTempId);
    }

    private getSampleContract() {
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

    public save() {
        if (this.validForm()) {
            this.submitted = true;
            this.isLoading = true;
            this.contractDistributionTemplate.distributionPolicyContractUploads = this.distributionPolicyContractUploads;
            let body = this.formatCalendar(this.fieldDates, this.contractDistributionTemplate);
            let formData = this.convertObjectToFormData(body);
            if (this.contractDistributionTemplate.id) {
                if(this.distributionPolicyContractUploads.length > 0) {
                    body.fileUploadName = this.distributionPolicyContractUploads[0].distributionPolicyUploadName ? this.distributionPolicyContractUploads[0].distributionPolicyUploadName: null;
                    body.fileUploadInvestorUrl = this.distributionPolicyContractUploads[0].investorFile ? this.distributionPolicyContractUploads[0].investorFile: null;
                    body.fileUploadBusinessCustomerUrl = this.distributionPolicyContractUploads[0].businessCustomerFile ? this.distributionPolicyContractUploads[0].businessCustomerFile: null;
                    body.contractType = this.distributionPolicyContractUploads[0].contractType ? this.distributionPolicyContractUploads[0].contractType: null;
                }
                let formDataUpdate = this.convertObjectToFormData(this.convertDataSendApi(body, this.fieldsUpdateToKeep));
                
                this._contractFormService.updateContractForm(formDataUpdate).subscribe((response) => {
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
                this._contractFormService.createContractForm(formData).subscribe((response) => {
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

    private validForm() {
        return ((!this.contractDistributionTemplate?.id && this.contractDistributionTemplate?.policyIds) || (this.contractDistributionTemplate?.id && this.contractDistributionTemplate?.policyId))
            && this.contractDistributionTemplate?.contractSource
            && this.contractDistributionTemplate?.configContractId
            && this.contractDistributionTemplate?.displayType
            && this.contractDistributionTemplate?.startDate;
    }

    private fieldsUpdateToKeep: string[] = ['id', 'displayType', 'contractSource', 'contractTemplateTempId',
        'fileUploadName', 'fileUploadInvestorUrl', 'fileUploadBusinessCustomerUrl', 'contractType', 'configContractId', 'startDate'];
    private convertDataSendApi(data: any, fieldsToKeep: string[]): any {
        const filteredData: any = {};
        for (const field of fieldsToKeep) {
            // khong gui len truong null
            if (data.hasOwnProperty(field) && data[field] !== null) {
              filteredData[field] = data[field];
            }
          }
          return filteredData;
    }
}




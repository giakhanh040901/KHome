import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService, ConfirmationService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { PageBondPolicyTemplate } from '@shared/model/pageBondPolicyTemplate';
import { ContractFormConst, DistributionContractTemplateConst, PolicyDetailTemplateConst, PolicyTempConst, SampleContractConst, YesNoConst } from '@shared/AppConsts';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PolicyTemplateService } from '@shared/services/policy-template.service';
import { ContractFormService } from '@shared/services/contract-form.service';
import { DistributionService } from '@shared/services/distribution.service';
import { SampleContractService } from '@shared/services/sample-contract.service';
import { Page } from '@shared/model/page';

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
    public configDialog: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    private _contractFormService: ContractFormService,
    private _distributionService: DistributionService,
    private _sampleContractService: SampleContractService,
  ) {
    super(injector, messageService);
  }

  fieldErrors: any = {};
  isCreateDetail: boolean;
  isCreateContractTemp: boolean;
  rows: any[] = [];
  index = 0;
  //
  YesNoConst = YesNoConst;
  ContractFormConst = ContractFormConst;
  SampleContractConst = SampleContractConst;
  DistributionContractTemplateConst = DistributionContractTemplateConst;
  //
  row: any;
  col: any;

  contractDistributionTemplate: any = {
    'distributionId' : 0,
    'policyId' : 0,
  }

  fieldDates = ['startDate', 'endDate'];

  submitted: boolean;
 
  policies = [];
  contractCodeTemplates = [];
  contractTemplates = [];

  distributionId: any;

  page = new Page();
  offset = 0;

  contractTemplateChoose: any = {};

  ngOnInit(): void {
    this.distributionId = this.configDialog?.data?.distributionId;
    if(this.configDialog?.data?.contractDistributionTemplate) {
      this.contractDistributionTemplate = this.configDialog?.data?.contractDistributionTemplate
      this.formatCalendarDisplay(this.fieldDates, this.contractDistributionTemplate);
    }
    console.log("contractDistributionTemplate",this.contractDistributionTemplate);
    
    if(this.distributionId ) {
      this.isLoading = true;
      forkJoin([
        this._distributionService.getAllPolicy(this.distributionId), 
        this._contractFormService.getAllNoPermisson(this.page),
        this._sampleContractService.getAllNoPermisson(this.page,this.contractDistributionTemplate?.contractSource)
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
          this.contractCodeTemplates = resContractCode?.data;
        }
        //Danh sách mẫu hợp đồng
        if (this.handleResponseInterceptor(resContract, "")) {
          this.contractTemplates = resContract?.data;
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

  getSampleContract(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.pageSize = -1;

    this.page.keyword = this.keyword;
    this.isLoading = true;
    this._sampleContractService.getAllNoPermisson(this.page,this.contractDistributionTemplate.contractSource).subscribe((res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
            this.contractTemplates = res?.data;
          
        }
    }, (err) => {
        this.isLoading = false;
        console.log('Error-------', err);
        
    });
}

  getAllPolicy(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		this.page.keyword = this.keyword;
		this.isLoading = true;
		this._distributionService.getAllPolicy(this.distributionId).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, "")) {
			  this.policies = res?.data;
			}
		  }, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		  });
	}

  getAllContractCodeStructure(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.pageSize =-1;

    this.page.keyword = this.keyword;
    this.isLoading = true;
    this._contractFormService.getAllNoPermisson(this.page).subscribe((res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
            
            this.page.totalItems = res.data.totalItems;
            this.contractCodeTemplates = res?.data;
        }
    }, (err) => {
        this.isLoading = false;
        console.log('Error-------', err);
        
    });
}

  save() {
    this.submitted = true;
    let body = this.formatCalendar(this.fieldDates,this.contractDistributionTemplate);
    if (this.contractDistributionTemplate.id) {
      this._contractFormService.updateContractForm(body).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Cập nhật thành công!")) {
            this.ref.close(true);
          } 
          this.submitted = false;
        }, (err) => {
          console.log('err----', err);
          this.submitted = false;
        }
      );
    } else {
      this._contractFormService.createContractForm(body).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Cập nhật thành công!")) {
            this.ref.close(true);
          }
          this.submitted = false;
        }, (err) => {
          console.log('err----', err);
          this.submitted = false;
        }
      );
    }
  }

  close() {
    this.ref.close();
  }
}



import { Component, Injector, OnInit } from '@angular/core';
import { PolicyDetailTemplateConst, PolicyTempConst, SampleContractConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ISampleContract } from '@shared/interfaces/sample-contract.interface';
import { DistributionService } from '@shared/services/distribution.service';
import { SampleContractService } from '@shared/services/sample-contract.service';
import { AppUtilsService } from '@shared/services/utils.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-create-sample-contract',
  templateUrl: './create-sample-contract.component.html',
  styleUrls: ['./create-sample-contract.component.scss']
})
export class CreateSampleContractComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    public ref: DynamicDialogRef,
    public configDialog: DynamicDialogConfig,
    private _distributionService: DistributionService,
    private _sampleContractService: SampleContractService,
    private _utilsService: AppUtilsService,
  ) {
    super(injector, messageService);
  }

  fieldErrors: any = {};
  SampleContractConst = SampleContractConst;
  sampleContract: ISampleContract = new ISampleContract();
  submitted: boolean;

  ngOnInit(): void {
    if (this.configDialog?.data?.sampleContract) {
      this.sampleContract = this.configDialog?.data?.sampleContract;
    }
  }

  myUploaderInvestor(event) {
    if (event?.files[0]) {
      this._distributionService.uploadFileGetUrl(event?.files[0], "sample-contract").subscribe((response) => {
        if (this.handleResponseInterceptor(response, "Tải file thành công!")) {
          this.sampleContract.fileInvestor = response.data;
        }
      }, (err) => {
        console.log('err__', err);
        this.messageError("Có sự cố khi upload!");
      }
      );
    }
  }

  myUploaderBusiCus(event) {
    if (event?.files[0]) {
      this._distributionService.uploadFileGetUrl(event?.files[0], "sample-contract").subscribe((response) => {
        if (this.handleResponseInterceptor(response, "Tải file thành công!")) {
          this.sampleContract.fileBusinessCustomer = response.data;
        }
      }, (err) => {
        console.log('err__', err);
        this.messageError("Có sự cố khi upload!");
      }
      );
    }
  }

  close() {
    this.ref.close();
  }

  save() {
    this.submitted = true;
    if (this.sampleContract?.id) {
      this._sampleContractService.update(this.sampleContract).subscribe((response) => {
        if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
          this.ref.close(true);
        }
        this.submitted = false;
      }, (err) => {
        console.log('err---', err);
        this.submitted = false;
      }
      );
    } else {
      this._sampleContractService.create(this.sampleContract).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
            this.submitted = false;
            this.ref.close(true);
          }
        }, (err) => {
          console.log('err---', err);
          this.submitted = false;
        }
      );
    }
  }

  validForm(): boolean {
    const validRequired = this.sampleContract?.name;
    return !!validRequired;
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }

}


import { Component, Inject, Injector, OnInit } from "@angular/core";
import { AppConsts } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { ContractTemplateServiceProxy } from "@shared/service-proxies/bond-manager-service";
import { BusinessCustomerServiceProxy } from "@shared/service-proxies/business-customer-service";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";
import { MessageService } from "primeng/api";

@Component({
	selector: "app-create-or-edit-business-license-file",
	templateUrl: "./create-or-edit-business-license-file.component.html",
	styleUrls: ["./create-or-edit-business-license-file.component.scss"],
})
export class CreateOrEditBusinessLicenseFileComponent extends CrudComponentBase {
	constructor(
		injector: Injector, 
		messageService: MessageService,
        private _contractTemplateService: ContractTemplateServiceProxy,
        private _businessCustomerService: BusinessCustomerServiceProxy,
        @Inject(API_BASE_URL) baseUrl?: string,
	) {
		super(injector, messageService);
		this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
	}

    private baseUrl: string;
    businessCustomerId: number;

    AppConsts = AppConsts;

	
    contractFile: any = {
        'juridicalFileId': 0,
        'title': null,
        'url': null,
        'businessCustomerId': 0,
    };

	ngOnInit(): void {}

	save() {
        this.submitted = true;
        //
        if (this.contractFile.id) {
            console.log("this.contractFile",this.contractFile);
            
            this._businessCustomerService.updateBusiCusLicenseFile(this.contractFile).subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
                        this.submitted = false;
                        // this.setPage();
                        // this.hideDialog();
                    } else {
                        this.submitted = false;
                    }
                },
                () => {
                    this.submitted = false;
                }
            );
        } else {
            this._businessCustomerService.createBusiCusLicenseFile(this.contractFile).subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Thêm thành công")) {
                        this.submitted = false;
                        // this.setPage();
                        // this.hideDialog();
                    } else {
                        this.submitted = false;
                    }
                },
                () => {
                    this.submitted = false;
                }
            );
        }
    }

    validForm(): boolean {
        return this.contractFile?.url?.trim() && this.contractFile?.title?.trim() ;
    }

    myUploader(event) {
        if (event?.files[0]) {
            this._contractTemplateService.uploadFileGetUrl(event?.files[0], "business-customer/license-file").subscribe(
                (response) => {
                    console.log({
                        response,
                    });
                    if (response?.code === 0) {
                        switch (response?.status) {
                            case 200:
                                break;
                            case 0:
                                this.messageError(response?.message || "");
                                break;
                            default:
                                this.messageError("Có sự cố khi upload!");
                                break;
                        }
                    } else if (response?.code === 200) {
                        this.contractFile.url = response.data;
                    }
                },
                (err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }
}

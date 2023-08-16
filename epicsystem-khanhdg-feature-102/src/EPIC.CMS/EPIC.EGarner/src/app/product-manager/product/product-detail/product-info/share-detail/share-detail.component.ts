import { Component, Injector, Input, OnInit } from '@angular/core';
import { AppConsts, ProductConst, UnitDateConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ContractTemplateServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-share-detail',
  templateUrl: './share-detail.component.html',
  styleUrls: ['./share-detail.component.scss']
})
export class ShareDetailComponent extends CrudComponentBase  {

  constructor(
    private _contractTemplateService: ContractTemplateServiceProxy,
    _injector: Injector,
		_messageService: MessageService,
  ) { super(_injector, _messageService);
		}

  @Input() productDetail: any;
  @Input() isEdit: boolean;
  AppConsts = AppConsts;
  // CONST
	ProductConst = ProductConst;
	UnitDates = UnitDateConst.list;	 // Đơn vị
	CpsInterestRateTypes = ProductConst.cpsInterestRateTypes;	// Kiêur trả cổ tức
	CountTypes = ProductConst.countTypes;	// Hình thức trả cổ tức
	YesNoList = YesNoConst.list; 	// Yes No
  imageDefault = 'assets/layout/images/image-bg-default.jpg';
  ngOnInit(): void {
    this.productDetail.summary = "Cập nhật đầu tư cổ phần";
  }

  myUploader(event) {
    if (event?.files[0]) {
      this._contractTemplateService.uploadFileGetUrl(event?.files[0], "share-detail").subscribe((response) => {
        if (this.handleResponseInterceptor(response)) {
          console.log("response",response);
          
          this.productDetail.icon = response?.data;
        }
      }, (err) => {
        console.log('err-----', err);
        this.messageError("Có sự cố khi upload!");
      }
      );
    }
  }

}

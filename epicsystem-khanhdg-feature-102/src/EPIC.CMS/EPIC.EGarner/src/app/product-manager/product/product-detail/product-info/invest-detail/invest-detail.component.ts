import { Component, Injector, Input, OnInit } from '@angular/core';
import { AppConsts, ProductConst, UnitDateConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ContractTemplateServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-invest-detail',
  templateUrl: './invest-detail.component.html',
  styleUrls: ['./invest-detail.component.scss']
})
export class InvestDetailComponent extends CrudComponentBase {

  constructor(
    private _contractTemplateService: ContractTemplateServiceProxy,
    _injector: Injector,
		_messageService: MessageService,
  ) { super(_injector, _messageService);
		}

  @Input() productDetail: any;
  @Input() isEdit: boolean;
  imageDefault = 'assets/layout/images/image-bg-default.jpg';
  // CONST
	ProductConst = ProductConst;
	UnitDates = UnitDateConst.list;	 // Đơn vị
	CpsInterestRateTypes = ProductConst.cpsInterestRateTypes;	// Kiêur trả cổ tức
	CountTypes = ProductConst.countTypes;	// Hình thức trả cổ tức
	YesNoList = YesNoConst.list; 	// Yes No
  InvestTypes = ProductConst.investTypes;
  AppConsts = AppConsts;
  ngOnInit(): void {
    this.productDetail.summary = "Cập nhật đầu tư BĐS";
  }

  myUploader(event) {
    if (event?.files[0]) {
      this._contractTemplateService.uploadFileGetUrl(event?.files[0], "invest-detail").subscribe((response) => {
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

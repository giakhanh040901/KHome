import { Component, Injector } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageErrorConst, ProductConst, TradingProviderConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ProductService } from '@shared/services/product.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-create-trading-provider',
  templateUrl: './create-trading-provider.component.html',
  styleUrls: ['./create-trading-provider.component.scss']
})
export class CreateTradingProviderComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
		messageService: MessageService,
    private router: Router,
    private _routeActive: ActivatedRoute,
    private _productService: ProductService,
    public configDialog: DynamicDialogConfig,
    public ref: DynamicDialogRef, 
  ) { 
    super(injector, messageService);
  }

  // CONST
  ProductConst = ProductConst;
  TradingProviderConst = TradingProviderConst;
  MessageErrorConst = MessageErrorConst;
  YesNoList = YesNoConst.list;
 
  modalDialog: boolean;
  isDisable: boolean;

  submitted: boolean;

  page = new Page();
  offset = 0;

  rows = [];

  tradingProvider: any = {
    "productId": null,
    "tradingProviderId": null,
    "hasTotalInvestmentSub" : null,
    "totalInvestmentSub": null,
    "quantity": null,
    "unitPrice": null,
    "isProfitFromPartner": null,
    "distributionDate": new Date(),
  }

  fieldDates = ['distributionDate'];
  // Menu otions thao tác
  actions: any[] = [];
  actionsDisplay: any[] = [];

  product: any;

  isDetail: boolean = false;

  tradingProviderDetail: any;

  tradingProviders = [];
  tradingProviderInfo: any = {};

  ngOnInit() { 
    //
    this.tradingProviderDetail = this.configDialog?.data?.tradingProviderDetail;
    this.tradingProvider.productId = this.configDialog?.data?.productId;
    this.isDetail = this.configDialog?.data?.isDetail;

    if(this.tradingProviderDetail) {
      this.tradingProviderInfo = this.tradingProviderDetail?.tradingProvider?.businessCustomer;
      this.rows = [this.tradingProviderInfo];
      //
      this.tradingProvider = {
        ...this.tradingProviderDetail,
        distributionDate: this.tradingProviderDetail?.distributionDate ? new Date(this.tradingProviderDetail?.distributionDate) : null,
      }
      //
      if(this.tradingProvider.hasTotalInvestmentSub == YesNoConst.YES && this.tradingProvider.totalInvestmentSub && this.tradingProvider.quantity) {
        this.tradingProvider.unitPrice = this.tradingProvider.totalInvestmentSub / this.tradingProvider.quantity;
      }
    } else {
      this.isLoadingPage = true;
      this._productService.getTradingProvider().subscribe((res) => {
        this.isLoadingPage = false;
        if(this.handleResponseInterceptor(res) && res?.data?.length) {
          this.tradingProviders = res?.data;
          this.tradingProviders = this.tradingProviders.map(t => {
              t.name = t.aliasName || t.name;
              return t;
          });
        }
      });
    }
    console.log(" this.tradingProviderDetail ", this.tradingProviderDetail );
  }

  changeTradingProvider(tradingProviderId) {
    let tradingProviderInfo = this.tradingProviders.find(t => t.tradingProviderId == tradingProviderId);
    if(tradingProviderId) this.rows = [tradingProviderInfo];
  }

  save() {
    if(this.validForm()) {

      this.submitted = true;
      let body = this.formatCalendar(this.fieldDates,{...this.tradingProvider});
      let messageSuccess = "Thêm mới thành công";
      body.summary = TradingProviderConst.THEM_MOI;
      //
      let apiCreateOrUpdate = this._productService.createTradingProvider(body);
      //
      if(this.tradingProvider?.id) {
        body.summary = TradingProviderConst.CAP_NHAT;
        messageSuccess = "Cập nhật thành công";
        apiCreateOrUpdate = this._productService.updateTradingProvider(body);
      }
      //
      apiCreateOrUpdate.subscribe((res) => {
          if (this.handleResponseInterceptor(res, messageSuccess)) {				
            this.ref.close(true);
          }
          this.submitted = false;
        },(err) => {
          console.log('err__', err);
        }
      );

    } else {
			this.messageError(MessageErrorConst.message.Validate);
		}


  }
  
  close() {
    this.ref.close();
  }

  changeHasTotalInvestmentSub(value) {
    if(value == YesNoConst.NO) {
      this.tradingProvider.totalInvestmentSub = null;
      this.tradingProvider.quantity = null;
      this.tradingProvider.unitPrice = null;
    }
  }
  
  edit() {
    this.modalDialog = true;
  }

  hideDialog() {
    this.modalDialog = false;
    this.submitted = false;
  }
  
  changeTotalInvestmentSub(value) {
    if(this.tradingProvider?.quantity) {
			this.tradingProvider.unitPrice = value / this.tradingProvider.quantity;
    }
	}
  
  changeQuantity(value) {
    if(this.tradingProvider?.totalInvestmentSub) {
      this.tradingProvider.unitPrice = this.tradingProvider.totalInvestmentSub / value;
    }
  }

  validForm(): boolean {
		const validRequired = this.tradingProvider?.tradingProviderId
                        && this.tradingProvider?.distributionDate
                        && this.tradingProvider?.isProfitFromPartner
                        && ((this.tradingProvider?.hasTotalInvestmentSub == YesNoConst.YES && this.tradingProvider?.totalInvestmentSub && this.tradingProvider?.quantity) || (this.tradingProvider?.hasTotalInvestmentSub == YesNoConst.NO));
		return validRequired;
	}

}

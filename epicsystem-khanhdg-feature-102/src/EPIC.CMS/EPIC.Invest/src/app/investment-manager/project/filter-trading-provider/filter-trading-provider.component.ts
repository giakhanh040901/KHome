import {
  Component,
  EventEmitter,
  Injector,
  OnInit,
  Output,
} from "@angular/core";
import { YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ProjectServiceProxy } from "@shared/services/project-manager-service";
import { MessageService } from "primeng/api";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";

@Component({
  selector: "app-filter-trading-provider",
  templateUrl: "./filter-trading-provider.component.html",
  styleUrls: ["./filter-trading-provider.component.scss"],
})
export class FilterTradingProviderComponent  extends CrudComponentBase implements OnInit {
    constructor(
        injector: Injector,
        messageService: MessageService,
        public ref: DynamicDialogRef, 
        private _projectService: ProjectServiceProxy,
        public configDialog: DynamicDialogConfig,
    ) {
        super(injector, messageService);
    }

    @Output() onCloseDialog = new EventEmitter<any>();

    tradingProviders: any = [];
    hasTotalInvestmentSub: boolean;
    tradingProvider = {
        id: 0,
        tradingProviderId: null,
        tradingProviderName: '',
        totalInvestmentSub: null,
    };
    

    ngOnInit(): void {
        this.hasTotalInvestmentSub = this.configDialog?.data?.hasTotalInvestmentSub;
        if(this.configDialog?.data?.inputData != null) {
            this.tradingProvider = this.configDialog.data.inputData;
        }
        //
        this._projectService.getAllTradingProvider().subscribe((resTradingProvider) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(resTradingProvider, "")) {
                this.tradingProviders = resTradingProvider?.data ?? [];
            }
        },(err) => {
            this.isLoading = false;
        });
    }

    changeTradingProvider(tradingProviderId) {
        let tradingProviderChange = this.tradingProviders.find((item) => item.tradingProviderId == tradingProviderId);
        this.tradingProvider.tradingProviderName = tradingProviderChange?.name;
    }

    cancel() {
        this.ref.close();
    }

    save() {
        this.ref.close(this.tradingProvider);
    }

    validForm(): boolean {
        const validRequired = this.tradingProvider.tradingProviderId 
                            && ((this.hasTotalInvestmentSub && this.tradingProvider.totalInvestmentSub) || !this.hasTotalInvestmentSub);
        // const validRequired = this.tradingProvider.tradingProviderId;
        return !!validRequired;
      }
}

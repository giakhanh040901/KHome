import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { APP_BASE_HREF, CommonModule, HashLocationStrategy, LocationStrategy } from '@angular/common';

// PrimeNG Components for demos
import { AccordionModule } from 'primeng/accordion';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { AvatarModule } from 'primeng/avatar';
import { AvatarGroupModule } from 'primeng/avatargroup';
import { BadgeModule } from 'primeng/badge';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { CardModule } from 'primeng/card';
import { CarouselModule } from 'primeng/carousel';
import { CascadeSelectModule } from 'primeng/cascadeselect';
import { ChartModule } from 'primeng/chart';
import { CheckboxModule } from 'primeng/checkbox';
import { ChipModule } from 'primeng/chip';
import { ChipsModule } from 'primeng/chips';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ColorPickerModule } from 'primeng/colorpicker';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DataViewModule } from 'primeng/dataview';
import { DialogModule } from 'primeng/dialog';
import { DividerModule } from 'primeng/divider';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
import { FileUploadModule } from 'primeng/fileupload';
import { FullCalendarModule } from '@fullcalendar/angular';
import { GalleriaModule } from 'primeng/galleria';
import { ImageModule } from 'primeng/image';
import { InplaceModule } from 'primeng/inplace';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputMaskModule } from 'primeng/inputmask';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { KnobModule } from 'primeng/knob';
import { LightboxModule } from 'primeng/lightbox';
import { ListboxModule } from 'primeng/listbox';
import { MegaMenuModule } from 'primeng/megamenu';
import { MenuModule } from 'primeng/menu';
import { MenubarModule } from 'primeng/menubar';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { MultiSelectModule } from 'primeng/multiselect';
import { OrderListModule } from 'primeng/orderlist';
import { OrganizationChartModule } from 'primeng/organizationchart';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { PanelMenuModule } from 'primeng/panelmenu';
import { PasswordModule } from 'primeng/password';
import { PickListModule } from 'primeng/picklist';
import { ProgressBarModule } from 'primeng/progressbar';
import { RadioButtonModule } from 'primeng/radiobutton';
import { RatingModule } from 'primeng/rating';
import { RippleModule } from 'primeng/ripple';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { ScrollTopModule } from 'primeng/scrolltop';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SidebarModule } from 'primeng/sidebar';
import { SkeletonModule } from 'primeng/skeleton';
import { SlideMenuModule } from 'primeng/slidemenu';
import { SliderModule } from 'primeng/slider';
import { SplitButtonModule } from 'primeng/splitbutton';
import { SplitterModule } from 'primeng/splitter';
import { StepsModule } from 'primeng/steps';
import { TabMenuModule } from 'primeng/tabmenu';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { TagModule } from 'primeng/tag';
import { TerminalModule } from 'primeng/terminal';
import { TieredMenuModule } from 'primeng/tieredmenu';
import { TimelineModule } from 'primeng/timeline';
import { ToastModule } from 'primeng/toast';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { ToolbarModule } from 'primeng/toolbar';
import { TooltipModule } from 'primeng/tooltip';
import { TreeModule } from 'primeng/tree';
import { TreeTableModule } from 'primeng/treetable';
import { VirtualScrollerModule } from 'primeng/virtualscroller';
import {MenuItem} from 'primeng/api';
import { MarkdownModule } from 'ngx-markdown';

// Application Components
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AppMainComponent } from './layout/main/app.main.component';
import { AppMenuComponent } from './layout/menu/app.menu.component';
import { AppMenuitemComponent } from './layout/menu/app.menuitem.component';
import { AppBreadcrumbComponent } from './layout/breadcrumb/app.breadcrumb.component';
import { AppTopBarComponent } from './layout/top-bar/app.topbar.component';
import { AppFooterComponent } from './layout/footer/app.footer.component';


import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ReactiveFormsModule } from '@angular/forms';

// Application services
import { BreadcrumbService } from './layout/breadcrumb/breadcrumb.service';
import { MenuService } from './layout/menu/app.menu.service';

import { AngularEditorModule } from '@kolkov/angular-editor';


import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import { HomeComponent } from './home/home.component';
import { SharedModule } from '@shared/shared.module';
import { OrderComponent } from './trading-contract/order/order.component';
import { OrderShareService } from '@shared/service-proxies/shared-data-service';
import { CreateOrderComponent } from './trading-contract/order/create-order/create-order.component';

import { OrderDetailComponent } from './trading-contract/order/order-detail/order-detail.component';
import { OrderFilterCustomerComponent } from './trading-contract/order/create-order/order-filter-customer/order-filter-customer.component';
import { OrderViewComponent } from './trading-contract/order/create-order/order-view/order-view.component';
import { UploadImageComponent } from './components/upload-image/upload-image.component';
import { DialogService, DynamicDialogModule } from 'primeng/dynamicdialog';

import { KeyFilterModule } from 'primeng/keyfilter';

import { ApproveComponent } from './approve-manager/approve/approve.component';
import { CalendarComponent } from './setting/calendar/calendar.component';
import { ContractProcessingComponent } from './trading-contract/contract-processing/contract-processing.component';
import { ContractActiveComponent } from './trading-contract/contract-active/contract-active.component';
import { ContractBlockageComponent } from './trading-contract/contract-blockage/contract-blockage.component';
import { OrderProfitComponent } from './trading-contract/order/order-detail/order-profit/order-profit.component';
import { LoginUrlComponent } from './login-url/login-url.component';
import { OrderPaymentComponent } from './trading-contract/order/order-detail/order-payment/order-payment.component';
import { OrderContractComponent } from './trading-contract/order/order-detail/order-contract/order-contract.component';
import { PolicyTemplateComponent } from './setting/policy-template/policy-template.component';
import { CreatePolicyTemplateComponent } from './setting/policy-template/create-policy-template/create-policy-template.component';
import { CreatePolicyTemplateDetailComponent } from './setting/policy-template/create-policy-template/create-policy-template-detail/create-policy-template-detail.component';

import { OwnerComponent } from './setting/owner/owner.component';

import { GeneralContractorComponent } from './setting/general-contractor/general-contractor.component';
import { FilterBusinessCustomerComponent } from './setting/general-contractor/filter-business-customer/filter-business-customer.component';
import { GeneralContractorDetailComponent } from './setting/general-contractor/general-contractor-detail/general-contractor-detail.component';
import { OwnerDetailComponent } from './setting/owner/owner-detail/owner-detail.component';
import { OrderFilterProjectComponent } from './trading-contract/order/create-order/order-filter-project/order-filter-project.component';
import { TradingProviderComponent } from './setting/trading-provider/trading-provider.component';
import { TradingProviderDetailComponent } from './setting/trading-provider/trading-provider-detail/trading-provider-detail.component';
import { MediaComponent } from './setting/media/media.component';
import { AddMediaComponent } from './setting/media/add-media/add-media.component';
import { FilterSaleComponent } from './trading-contract/order/create-order/order-filter-customer/filter-sale/filter-sale.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';

import { SystemTemplateComponent } from './setting/system-template/system-template.component';

import { DeliveryContractComponent } from './trading-contract/delivery-contract/delivery-contract.component';
import { ContractBlockageDetailComponent } from './trading-contract/contract-blockage/contract-blockage-detail/contract-blockage-detail.component';
import { DeliveryContractDetailComponent } from './trading-contract/delivery-contract/delivery-contract-detail/delivery-contract-detail.component';
import { OperationalReportComponent } from './export-report/operational-report/operational-report.component';
import { BusinessReportComponent } from './export-report/business-report/business-report.component';
import { ManagementReportComponent } from './export-report/management-report/management-report.component';
import { InterestContractComponent } from './trading-contract/interest-contract/interest-contract.component';
import { ReinstatementRequestComponent } from './trading-contract/contract-active/reinstatement-request/reinstatement-request.component';
import { WithdrawalRequestComponent } from './trading-contract/contract-active/withdrawal-request/withdrawal-request.component';
import { ApproveReinstatementComponent } from './approve-manager/approve-reinstatement/approve-reinstatement.component';
import { FormApproveRequestComponent } from './approve-manager/form-approve-request/form-approve-request.component';
import { ApproveWithdrawalComponent } from './approve-manager/approve-withdrawal/approve-withdrawal.component';
import { OrderHistoryUpdateComponent } from './trading-contract/order/order-detail/order-history-update/order-history-update.component';
import { ExpireContractComponent } from './trading-contract/expire-contract/expire-contract.component';
import { ContractCodeStructureComponent } from './setting/contract-code-structure/contract-code-structure.component';
import { CreateContractCodeComponent } from './setting/contract-code-structure/create-contract-code/create-contract-code.component';
import { SampleContractComponent } from './setting/sample-contract/sample-contract.component';
import { CreateSampleContractComponent } from './setting/sample-contract/create-sample-contract/create-sample-contract.component';
import { ManagerWithdrawComponent } from './trading-contract/manager-withdraw/manager-withdraw.component';
import { OTPWithdrawComponent } from './trading-contract/manager-withdraw/otpwithdraw/otpwithdraw.component';
import { ListRequestBankComponent } from './trading-contract/manager-withdraw/list-request-bank/list-request-bank.component';
import { DetailRequestComponent } from './trading-contract/manager-withdraw/detail-request/detail-request.component';
import { NgOtpInputModule } from  'ng-otp-input';
import { FormRequestComponent } from './form-general/form-request-approve-cancel/form-request/form-request.component';
import { FormApproveComponent } from './form-general/form-request-approve-cancel/form-approve/form-approve.component';
import { FormCancelComponent } from './form-general/form-request-approve-cancel/form-cancel/form-cancel.component';
import { FormCloseComponent } from './form-general/form-request-approve-cancel/form-close/form-close.component';
import { DistributionComponent } from './investment-manager/distribution/distribution.component';
import { CreateDistributionComponent } from './investment-manager/distribution/create-distribution/create-distribution.component';
import { DistributionDetailComponent } from './investment-manager/distribution/distribution-detail/distribution-detail.component';
import { DistributionPolicyComponent } from './investment-manager/distribution/distribution-detail/distribution-policy/distribution-policy.component';
import { CreateDistributionPolicyComponent } from './investment-manager/distribution/distribution-detail/distribution-policy/create-distribution-policy/create-distribution-policy.component';
import { CreateDistributionPolicyDetailComponent } from './investment-manager/distribution/distribution-detail/distribution-policy/create-distribution-policy/create-distribution-policy-detail/create-distribution-policy-detail.component';
import { ProjectComponent } from './investment-manager/project/project.component';
import { ProjectDetailComponent } from './investment-manager/project/project-detail/project-detail.component';
import { FilterGeneralContractorComponent } from './investment-manager/project/filter-general-contractor/filter-general-contractor.component';
import { FilterOwnerComponent } from './investment-manager/project/filter-owner/filter-owner.component';
import { DistributionContractTemplateComponent } from './investment-manager/distribution/distribution-detail/distribution-contract-template/distribution-contract-template.component';
import { ProjectFilterComponent } from './investment-manager/distribution/create-distribution/project-filter/project-filter.component';
import { DistributionFileComponent } from './investment-manager/distribution/distribution-detail/distribution-file/distribution-file.component';
import { ProjectFileComponent } from './investment-manager/project/project-detail/project-file/project-file.component';
import { ProjectMediaComponent } from './investment-manager/project/project-detail/project-media/project-media.component';
import { AddProjectMediaComponent } from './investment-manager/project/project-detail/project-media/add-project-media/add-project-media.component';
import { DistributionFileTemplateComponent } from './investment-manager/distribution/distribution-detail/distribution-file-template/distribution-file-template.component';
import { FormViewPdfFileComponent } from './form-general/form-view-pdf-file/form-view-pdf-file.component';
import { ProjectImageComponent } from './investment-manager/project/project-detail/project-image/project-image.component';
import { AddProjectImageComponent } from './investment-manager/project/project-detail/project-image/add-project-image/add-project-image.component';
import { FormNotificationComponent } from './form-general/form-notification/form-notification.component';
import { ReceiveContractTemplateComponent } from './investment-manager/distribution/distribution-detail/receive-contract-template/receive-contract-template.component';
import { DistributionOverViewComponent } from './investment-manager/distribution/distribution-detail/distribution-over-view/distribution-over-view.component';
import { FilterTradingProviderComponent } from './investment-manager/project/filter-trading-provider/filter-trading-provider.component';
import { CreateDistributionContractComponent } from './investment-manager/distribution/distribution-detail/distribution-contract-template/create-distribution-contract/create-distribution-contract.component';
import { FormSetDisplayColumnComponent } from './form-general/form-set-display-column/form-set-display-column.component';
import { ContractRenewalComponent } from './trading-contract/contract-renewal/contract-renewal.component';
import { InvestmentHistoryComponent } from './trading-contract/investment-history/investment-history.component';
import { OrderPaymentViewComponent } from './trading-contract/order/order-detail-view/order-payment-view/order-payment-view.component';
import { OrderContractViewComponent } from './trading-contract/order/order-detail-view/order-contract-view/order-contract-view.component';
import { OrderDetailViewComponent } from './trading-contract/order/order-detail-view/order-detail-view.component';
import { CollectMoneyBankComponent } from './query-manager/collect-money-bank/collect-money-bank.component';
import { PayMoneyBankComponent } from './query-manager/pay-money-bank/pay-money-bank.component';
import { FunctionForDevComponent } from './trading-contract/order/order-detail/order-payment/function-for-dev/function-for-dev.component';
import { ViewPayMoneyComponent } from './query-manager/pay-money-bank/view-pay-money/view-pay-money.component';
import { ProjectShareComponent } from './investment-manager/project/project-detail/project-share/project-share.component';
import { ProjectShareDetailComponent } from './investment-manager/project/project-detail/project-share/project-share-detail/project-share-detail.component';
import { UploadMediaComponent } from './components/upload-media/upload-media.component';
import { CreateMultipleDistributionContractTemplateComponent } from './investment-manager/distribution/distribution-detail/distribution-contract-template/create-multiple-distribution-contract-template/create-multiple-distribution-contract-template.component';

FullCalendarModule.registerPlugins([
    dayGridPlugin,
    timeGridPlugin,
    interactionPlugin
]);

@NgModule({
    imports: [
        SharedModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AppRoutingModule,
        HttpClientModule,
        AccordionModule,
        AutoCompleteModule,
        AvatarModule,
        AvatarGroupModule,
        BadgeModule,
        BreadcrumbModule,
        ButtonModule,
        CalendarModule,
        CardModule,
        CarouselModule,
        CascadeSelectModule,
        ChartModule,
        CheckboxModule,
        ChipModule,
        ChipsModule,
        CodeHighlighterModule,
        ConfirmDialogModule,
        ConfirmPopupModule,
        ColorPickerModule,
        ContextMenuModule,
        DataViewModule,
        DialogModule,
        DividerModule,
        DropdownModule,
        FieldsetModule,
        FileUploadModule,
        FullCalendarModule,
        GalleriaModule,
        ImageModule,
        InplaceModule,
        InputNumberModule,
        InputMaskModule,
        InputSwitchModule,
        InputTextModule,
        InputTextareaModule,
        KnobModule,
        LightboxModule,
        ListboxModule,
        MegaMenuModule,
        MenuModule,
        MenubarModule,
        MessageModule,
        MessagesModule,
        MultiSelectModule,
        OrderListModule,
        OrganizationChartModule,
        OverlayPanelModule,
        PaginatorModule,
        PanelModule,
        PanelMenuModule,
        PasswordModule,
        PickListModule,
        ProgressBarModule,
        RadioButtonModule,
        RatingModule,
        RippleModule,
        ScrollPanelModule,
        ScrollTopModule,
        SelectButtonModule,
        SidebarModule,
        SkeletonModule,
        SlideMenuModule,
        SliderModule,
        SplitButtonModule,
        SplitterModule,
        StepsModule,
        TableModule,
        TabMenuModule,
        TabViewModule,
        TagModule,
        TerminalModule,
        TimelineModule,
        TieredMenuModule,
        ToastModule,
        ToggleButtonModule,
        ToolbarModule,
        TooltipModule,
        TreeModule,
        TreeTableModule,
        VirtualScrollerModule,
        ProgressSpinnerModule,
        AngularEditorModule,
        DynamicDialogModule,
        KeyFilterModule,
        PdfViewerModule,
        MarkdownModule.forRoot(),
        NgOtpInputModule,
    ],

    declarations: [
        AppComponent,
        AppMainComponent,
        AppMenuComponent,
        AppMenuitemComponent,
        AppTopBarComponent,
        AppFooterComponent,
        AppBreadcrumbComponent,
        HomeComponent,
        FilterBusinessCustomerComponent,
        FilterBusinessCustomerComponent,
        OrderComponent,
        CreateOrderComponent,
        OrderFilterCustomerComponent,
        OrderViewComponent,
        OrderDetailComponent,
        OrderPaymentComponent,
        OrderContractComponent,
        UploadImageComponent,
        ApproveComponent,
        CalendarComponent,
        FormRequestComponent,
        FormApproveComponent,
        FormCancelComponent,
        ContractProcessingComponent,
        ContractActiveComponent,
        ContractBlockageComponent,
        OrderProfitComponent,
        FormCloseComponent,
        LoginUrlComponent,
        FormSetDisplayColumnComponent,
        PolicyTemplateComponent,
        CreatePolicyTemplateComponent,
        CreatePolicyTemplateDetailComponent,
        DistributionComponent,
        CreateDistributionComponent,
        DistributionDetailComponent,
        DistributionPolicyComponent,
        CreateDistributionPolicyComponent,
        CreateDistributionPolicyDetailComponent,
        OwnerComponent,
        ProjectComponent,
        ProjectDetailComponent,
        GeneralContractorComponent,
        FilterBusinessCustomerComponent,
        GeneralContractorDetailComponent,
        OwnerDetailComponent,
        FilterOwnerComponent,
        FilterGeneralContractorComponent,
        DistributionContractTemplateComponent,
        ProjectFilterComponent,
        DistributionFileComponent,
        OrderFilterProjectComponent,
        ProjectFileComponent,
        TradingProviderComponent,
        TradingProviderDetailComponent,
        MediaComponent,
        AddMediaComponent,
        ProjectMediaComponent,
        AddProjectMediaComponent,
        DistributionFileTemplateComponent,
        FilterSaleComponent,
        SystemTemplateComponent,
        FormViewPdfFileComponent,
        ProjectImageComponent,
        AddProjectImageComponent,
        FormNotificationComponent,
        DeliveryContractComponent,
        ContractBlockageDetailComponent,
        ReceiveContractTemplateComponent,
        DeliveryContractDetailComponent,
        OperationalReportComponent,
        BusinessReportComponent,
        ManagementReportComponent,
        InterestContractComponent,
        ReinstatementRequestComponent,
        WithdrawalRequestComponent,
        ApproveReinstatementComponent,
        FormApproveRequestComponent,
        ApproveWithdrawalComponent,
        DistributionOverViewComponent,
        FilterTradingProviderComponent,
        OrderHistoryUpdateComponent,
        ExpireContractComponent,
        ContractCodeStructureComponent,
        CreateContractCodeComponent,
        SampleContractComponent,
        CreateSampleContractComponent,
        CreateDistributionContractComponent,
        ExpireContractComponent,
        ManagerWithdrawComponent,
        OTPWithdrawComponent,
        ListRequestBankComponent,
        DetailRequestComponent,
        ContractRenewalComponent,
        InvestmentHistoryComponent,
        OrderPaymentViewComponent,
        OrderContractViewComponent,
        OrderDetailViewComponent,
        CollectMoneyBankComponent,
        PayMoneyBankComponent,
        FunctionForDevComponent,
        ViewPayMoneyComponent,
        ProjectShareComponent,
        ProjectShareDetailComponent,
        UploadMediaComponent,
        CreateMultipleDistributionContractTemplateComponent,
    ],
    
    entryComponents: [
    ],
    providers: [
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        MenuService,
        BreadcrumbService,
        OrderShareService,
        DialogService,
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}

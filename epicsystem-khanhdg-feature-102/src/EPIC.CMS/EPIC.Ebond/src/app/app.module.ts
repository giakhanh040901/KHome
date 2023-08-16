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
import { MarkdownModule } from 'ngx-markdown';

import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import { HomeComponent } from './home/home.component';
import { UserComponent } from './user/user.component';
import { IssuerComponent } from './setting/issuer/issuer.component';
import { DepositProviderComponent } from './setting/deposit-provider/deposit-provider.component';
import { ProductCategoryComponent } from './setting/product-category/product-category.component';
import { ProductBondTypeComponent } from './setting/product-bond-type/product-bond-type.component';
import { ProductBondDetailComponent } from './bond-manager/product-bond-detail/product-bond-detail.component';
import { ProductBondInfoComponent } from './bond-manager/product-bond-info/product-bond-info.component';
import { ContractTemplateComponent } from './bond-manager/contract-template/contract-template.component';

import { ProductBondPrimaryComponent } from './bond-manager/product-bond-primary/product-bond-primary.component';

import { ProductBondPolicyTemplateComponent } from './setting/product-bond-policy-template/product-bond-policy-template.component';

import { ProductBondPrimaryDetailComponent } from './bond-manager/product-bond-primary/product-bond-primary-detail/product-bond-primary-detail.component';
import { DistributionContractComponent } from './bond-manager/distribution-contract/distribution-contract.component';
import { FilterTradingProviderComponent } from './bond-manager/distribution-contract/filter-trading-provider/filter-trading-provider.component';
import { DistributionContractDetailComponent } from './bond-manager/distribution-contract/distribution-contract-detail/distribution-contract-detail.component';
import { DistributionContractPaymentComponent } from './bond-manager/distribution-contract/distribution-contract-payment/distribution-contract-payment.component';
import { DistributionContractFileComponent } from './bond-manager/distribution-contract/distribution-contract-file/distribution-contract-file.component';
import { ProductBondSecondaryComponent } from './bond-manager/product-bond-secondary/product-bond-secondary.component';
import { ProductBondSecondaryUpdateComponent } from './bond-manager/product-bond-secondary/product-bond-secondary-update/product-bond-secondary-update.component';
import { SharedModule } from '@shared/shared.module';
import { DistributionContractInterestPeriodComponent } from './bond-manager/distribution-contract/distribution-contract-interest-period/distribution-contract-interest-period.component';
import { ProductBondPrimaryInterestPolicyComponent } from './bond-manager/product-bond-primary/product-bond-primary-interest-policy/product-bond-primary-interest-policy.component';
import { ProductBondInfoDetailComponent } from './bond-manager/product-bond-info/product-bond-info-detail/product-bond-info-detail.component';
import { ProductBondInfoManagerBankComponent } from './bond-manager/product-bond-info/product-bond-info-manager-bank/product-bond-info-manager-bank.component';
import { CreateOrEditPolicyComponent } from './bond-manager/product-bond-secondary/create-or-edit-policy/create-or-edit-policy.component';
import { CreateOrEditPolicyDetailComponent } from './bond-manager/product-bond-secondary/create-or-edit-policy-detail/create-or-edit-policy-detail.component';
import { CreateOrEditSecondaryComponent } from './bond-manager/product-bond-secondary/create-or-edit-secondary/create-or-edit-secondary.component';
import { ProductBondInfoCollateralComponent } from './bond-manager/product-bond-info/product-bond-info-collateral/product-bond-info-collateral.component';
import { OrderComponent } from './trading-contract/order/order.component';
import { OrderService } from '@shared/service-proxies/shared-data-service';
import { CreateOrderComponent } from './trading-contract/order/create-order/create-order.component';

import { FilterBusinessCustomerComponent } from './setting/issuer/filter-business-customer/filter-business-customer.component';
import { IssuerDetailComponent } from './setting/issuer/issuer-detail/issuer-detail.component';
// import { SimpleModalModule } from 'ngx-simple-modal'; 

import { DepositProviderDetailComponent } from './setting/deposit-provider/deposit-provider-detail/deposit-provider-detail.component';

import { OrderDetailComponent } from './trading-contract/order/order-detail/order-detail.component';
import { OrderFilterCustomerComponent } from './trading-contract/order/create-order/order-filter-customer/order-filter-customer.component';
import { OrderFilterBondComponent } from './trading-contract/order/create-order/order-filter-bond/order-filter-bond.component';
import { OrderViewComponent } from './trading-contract/order/create-order/order-view/order-view.component';
import { ProductBondInfoFileComponent } from './bond-manager/product-bond-info/product-bond-info-file/product-bond-info-file.component';
import { UploadImageComponent } from './components/upload-image/upload-image.component';
import { DialogService, DynamicDialogModule } from 'primeng/dynamicdialog';
import { ProductBondInfoCouponComponent } from './bond-manager/product-bond-info/product-bond-info-coupon/product-bond-info-coupon.component';

import { KeyFilterModule } from 'primeng/keyfilter';

import { ProductBondSecondPriceComponent } from './bond-manager/product-bond-secondary/product-bond-second-price/product-bond-second-price.component';
import { ProductBondSecondaryFileComponent } from './bond-manager/product-bond-secondary/product-bond-secondary-file/product-bond-secondary-file.component';
import { ApproveComponent } from './approve-manager/approve/approve.component';
import { CalendarComponent } from './setting/calendar/calendar.component';
import { FormRequestComponent } from './form-request-approve-cancel/form-request/form-request.component';
import { FormApproveComponent } from './form-request-approve-cancel/form-approve/form-approve.component';
import { FormCancelComponent } from './form-request-approve-cancel/form-cancel/form-cancel.component';
import { ContractProcessingComponent } from './trading-contract/contract-processing/contract-processing.component';
import { ContractActiveComponent } from './trading-contract/contract-active/contract-active.component';
import { ContractBlockageComponent } from './trading-contract/contract-blockage/contract-blockage.component';
import { OrderCouponComponent } from './trading-contract/order/order-detail/order-coupon/order-coupon.component';
import { OrderProfitComponent } from './trading-contract/order/order-detail/order-profit/order-profit.component';
import { FormCloseComponent } from './form-request-approve-cancel/form-close/form-close.component';
import { LoginUrlComponent } from './login-url/login-url.component';
import { FormSetDisplayColumnComponent } from './form-set-display-column/form-set-display-column/form-set-display-column.component';
import { TradingProviderComponent } from './setting/trading-provider/trading-provider.component';
import { TradingProviderDetailComponent } from './setting/trading-provider/trading-provider-detail/trading-provider-detail.component';
import { CreateTradingProviderAccountComponent } from './setting/trading-provider/trading-provider-detail/trading-provider-account/create-trading-provider-account/create-trading-provider-account.component';
import { TradingProviderAccountComponent } from './setting/trading-provider/trading-provider-detail/trading-provider-account/trading-provider-account.component';
import { OrderPaymentComponent } from './trading-contract/order/order-detail/order-payment/order-payment.component';
import { OrderContractComponent } from './trading-contract/order/order-detail/order-contract/order-contract.component';
import { SystemTemplateComponent } from './notification/system-template/system-template.component';
import { MediaComponent } from './setting/media/media.component';
import { AddMediaComponent } from './setting/media/add-media/add-media.component';
import { FilterSaleComponent } from './trading-contract/order/create-order/order-filter-customer/filter-sale/filter-sale.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { FormViewPdfFileComponent } from './form-view-pdf-file/form-view-pdf-file.component';
import { CalendarPartnerComponent } from './setting/calendar-partner/calendar-partner.component';
import { FormNotificationComponent } from './form-notification/form-notification.component';
import { DeliveryContractComponent } from './trading-contract/delivery-contract/delivery-contract.component';
import { FormBondInfoApproveComponent } from './approve-manager/approve/form-bond-info-approve/form-bond-info-approve.component';
import { ContractBlockageDetailComponent } from './trading-contract/contract-blockage/contract-blockage-detail/contract-blockage-detail.component';
import { ReceiveContractTemplateComponent } from './bond-manager/product-bond-secondary/product-bond-secondary-update/receive-contract-template/receive-contract-template.component';
import { DeliveryContractDetailComponent } from './trading-contract/delivery-contract/delivery-contract-detail/delivery-contract-detail.component';
import { ManagementReportComponent } from './export-report/management-report/management-report.component';
import { OperationalReportComponent } from './export-report/operational-report/operational-report.component';
import { BusinessReportComponent } from './export-report/business-report/business-report.component';
import { InterestContractComponent } from './trading-contract/interest-contract/interest-contract.component';
import { InterestContractDetailComponent } from './trading-contract/interest-contract/interest-contract-detail/interest-contract-detail.component';
import { ReinstatementRequestComponent } from './trading-contract/contract-active/reinstatement-request/reinstatement-request.component';
import { ApproveReinstatementComponent } from './approve-manager/approve-reinstatement/approve-reinstatement.component';
import { FormApproveRequestComponent } from './approve-manager/form-approve-request/form-approve-request.component';
import { SecondaryOverviewComponent } from './bond-manager/product-bond-secondary/secondary-overview/secondary-overview.component';



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
        UserComponent,
        TradingProviderAccountComponent,
        IssuerComponent,
        ProductBondSecondaryComponent,
        ProductBondSecondaryUpdateComponent,
        CreateOrEditPolicyComponent,
        CreateOrEditPolicyDetailComponent,
        CreateOrEditSecondaryComponent,
        TradingProviderComponent,
        TradingProviderDetailComponent,
        DepositProviderComponent,
        DepositProviderDetailComponent,
        ProductCategoryComponent,
        ProductBondTypeComponent,
        ProductBondDetailComponent,
        ProductBondInfoComponent,
        ContractTemplateComponent,
        ProductBondPrimaryComponent,
        ProductBondPolicyTemplateComponent,
        ProductBondPrimaryDetailComponent,
        DistributionContractComponent,
        FilterTradingProviderComponent,
        DistributionContractDetailComponent,
        DistributionContractPaymentComponent,
        DistributionContractFileComponent,
        DistributionContractInterestPeriodComponent,
        ProductBondPrimaryInterestPolicyComponent,
        ProductBondInfoDetailComponent,
        ProductBondInfoManagerBankComponent,
        FilterBusinessCustomerComponent,
        ProductBondInfoCollateralComponent,
        IssuerDetailComponent,
        ProductBondInfoCollateralComponent,
        FilterBusinessCustomerComponent,
        TradingProviderDetailComponent,
        DepositProviderDetailComponent,
        OrderComponent,
        CreateOrderComponent,
        OrderFilterCustomerComponent,
        OrderFilterBondComponent,
        OrderViewComponent,
        OrderDetailComponent,
        ProductBondInfoFileComponent,
        OrderPaymentComponent,
        ProductBondSecondPriceComponent,
        OrderContractComponent,
        UploadImageComponent,
        ProductBondInfoCouponComponent,
        ProductBondSecondaryFileComponent,
        ApproveComponent,
        CalendarComponent,
        FormRequestComponent,
        FormApproveComponent,
        FormCancelComponent,
        ContractProcessingComponent,
        ContractActiveComponent,
        ContractBlockageComponent,
        OrderCouponComponent,
        OrderProfitComponent,
        FormCloseComponent,
        LoginUrlComponent,
        FormSetDisplayColumnComponent,
        CreateTradingProviderAccountComponent,
        SystemTemplateComponent,
        MediaComponent,
        AddMediaComponent,
        FilterSaleComponent,
        FormViewPdfFileComponent,
        CalendarPartnerComponent,
        FormNotificationComponent,
        DeliveryContractComponent,
        FormBondInfoApproveComponent,
        ContractBlockageDetailComponent,
        ReceiveContractTemplateComponent,
        DeliveryContractDetailComponent,
        ManagementReportComponent,
        OperationalReportComponent,
        BusinessReportComponent,
        InterestContractComponent,
        InterestContractDetailComponent,
        ReinstatementRequestComponent,
        ApproveReinstatementComponent,
        FormApproveRequestComponent,
        SecondaryOverviewComponent,
    ],
    entryComponents: [
        UploadImageComponent,
    ],
    providers: [
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        MenuService,
        BreadcrumbService,
        OrderService,
        DialogService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}

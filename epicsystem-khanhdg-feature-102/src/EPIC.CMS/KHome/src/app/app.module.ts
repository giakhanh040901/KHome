import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { CommonModule, HashLocationStrategy, LocationStrategy } from "@angular/common";

// PrimeNG Components for demos
import { AccordionModule } from "primeng/accordion";
import { AutoCompleteModule } from "primeng/autocomplete";
import { AvatarModule } from "primeng/avatar";
import { AvatarGroupModule } from "primeng/avatargroup";
import { BadgeModule } from "primeng/badge";
import { BreadcrumbModule } from "primeng/breadcrumb";
import { ButtonModule } from "primeng/button";
import { CalendarModule } from "primeng/calendar";
import { CardModule } from "primeng/card";
import { CarouselModule } from "primeng/carousel";
import { CascadeSelectModule } from "primeng/cascadeselect";
import { ChartModule } from "primeng/chart";
import { CheckboxModule } from "primeng/checkbox";
import { ChipModule } from "primeng/chip";
import { ChipsModule } from "primeng/chips";
import { CodeHighlighterModule } from "primeng/codehighlighter";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmPopupModule } from "primeng/confirmpopup";
import { ColorPickerModule } from "primeng/colorpicker";
import { ContextMenuModule } from "primeng/contextmenu";
import { DataViewModule } from "primeng/dataview";
import { DialogModule } from "primeng/dialog";
import { DividerModule } from "primeng/divider";
import { DropdownModule } from "primeng/dropdown";
import { FieldsetModule } from "primeng/fieldset";
import { FileUploadModule } from "primeng/fileupload";
import { FullCalendarModule } from "@fullcalendar/angular";
import { GalleriaModule } from "primeng/galleria";
import { ImageModule } from "primeng/image";
import { InplaceModule } from "primeng/inplace";
import { InputNumberModule } from "primeng/inputnumber";
import { InputMaskModule } from "primeng/inputmask";
import { InputSwitchModule } from "primeng/inputswitch";
import { InputTextModule } from "primeng/inputtext";
import { InputTextareaModule } from "primeng/inputtextarea";
import { KnobModule } from "primeng/knob";
import { LightboxModule } from "primeng/lightbox";
import { ListboxModule } from "primeng/listbox";
import { MegaMenuModule } from "primeng/megamenu";
import { MenuModule } from "primeng/menu";
import { MenubarModule } from "primeng/menubar";
import { MessagesModule } from "primeng/messages";
import { MessageModule } from "primeng/message";
import { MultiSelectModule } from "primeng/multiselect";
import { OrderListModule } from "primeng/orderlist";
import { OrganizationChartModule } from "primeng/organizationchart";
import { OverlayPanelModule } from "primeng/overlaypanel";
import { PaginatorModule } from "primeng/paginator";
import { PanelModule } from "primeng/panel";
import { PanelMenuModule } from "primeng/panelmenu";
import { PasswordModule } from "primeng/password";
import { PickListModule } from "primeng/picklist";
import { ProgressBarModule } from "primeng/progressbar";
import { RadioButtonModule } from "primeng/radiobutton";
import { RatingModule } from "primeng/rating";
import { RippleModule } from "primeng/ripple";
import { ScrollPanelModule } from "primeng/scrollpanel";
import { ScrollTopModule } from "primeng/scrolltop";
import { SelectButtonModule } from "primeng/selectbutton";
import { SidebarModule } from "primeng/sidebar";
import { SkeletonModule } from "primeng/skeleton";
import { SlideMenuModule } from "primeng/slidemenu";
import { SliderModule } from "primeng/slider";
import { SplitButtonModule } from "primeng/splitbutton";
import { SplitterModule } from "primeng/splitter";
import { StepsModule } from "primeng/steps";
import { TabMenuModule } from "primeng/tabmenu";
import { TableModule } from "primeng/table";
import { TabViewModule } from "primeng/tabview";
import { TagModule } from "primeng/tag";
import { TerminalModule } from "primeng/terminal";
import { TieredMenuModule } from "primeng/tieredmenu";
import { TimelineModule } from "primeng/timeline";
import { ToastModule } from "primeng/toast";
import { ToggleButtonModule } from "primeng/togglebutton";
import { ToolbarModule } from "primeng/toolbar";
import { TooltipModule } from "primeng/tooltip";
import { TreeModule } from "primeng/tree";
import { TreeTableModule } from "primeng/treetable";
import { VirtualScrollerModule } from "primeng/virtualscroller";
import { MenuItem } from "primeng/api";
import { MarkdownModule } from "ngx-markdown";

// Application Components
import { AppComponent } from "./app.component";
import { AppRoutingModule } from "./app-routing.module";
import { AppMainComponent } from "./layout/main/app.main.component";
import { AppMenuComponent } from "./layout/menu/app.menu.component";
import { AppMenuitemComponent } from "./layout/menu/app.menuitem.component";
import { AppBreadcrumbComponent } from "./layout/breadcrumb/app.breadcrumb.component";
import { AppTopBarComponent } from "./layout/top-bar/app.topbar.component";
import { AppFooterComponent } from "./layout/footer/app.footer.component";

import { ProgressSpinnerModule } from "primeng/progressspinner";
import { ReactiveFormsModule } from "@angular/forms";

// Application services
import { BreadcrumbService } from "./layout/breadcrumb/breadcrumb.service";
import { MenuService } from "./layout/menu/app.menu.service";

import { AngularEditorModule } from "@kolkov/angular-editor";

import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";
import { HomeComponent } from "./home/home.component";
import { UserComponent } from "./user/user.component";
import { SharedModule } from "@shared/shared.module";
import { OrderService } from "@shared/service-proxies/shared-data-service";

import { UploadImageComponent } from "./components/upload-image/upload-image.component";
import {
	DialogService,
	DynamicDialogConfig,
	DynamicDialogModule,
	DynamicDialogRef,
} from "primeng/dynamicdialog";
import { ConfirmationService } from "primeng/api";

import { KeyFilterModule } from "primeng/keyfilter";

import { LoginUrlComponent } from "./login-url/login-url.component";
import { ContractTemplateServiceProxy } from "@shared/service-proxies/trading-contract-service";
import { PdfViewerModule } from "ng2-pdf-viewer";
import { OperationalReportComponent } from "./export-report/operational-report/operational-report.component";
import { BusinessReportComponent } from "./export-report/business-report/business-report.component";
import { ManagementReportComponent } from "./export-report/management-report/management-report.component";
import { NgOtpInputModule } from "ng-otp-input";
import { FormRequestComponent } from "./form-general/form-request-approve-cancel/form-request/form-request.component";
import { FormApproveComponent } from "./form-general/form-request-approve-cancel/form-approve/form-approve.component";
import { FormCancelComponent } from "./form-general/form-request-approve-cancel/form-cancel/form-cancel.component";
import { FormCloseComponent } from "./form-general/form-request-approve-cancel/form-close/form-close.component";
import { FormViewPdfFileComponent } from "./form-general/form-view-pdf-file/form-view-pdf-file.component";
import { FormNotificationComponent } from "./form-general/form-notification/form-notification.component";
import { FormSetDisplayColumnComponent } from "./form-general/form-set-display-column/form-set-display-column.component";
import { UploadMediaComponent } from "./components/upload-media/upload-media.component";
import { SelectIconComponent } from "./components/select-icon/select-icon.component";
import { InvestorImageComponent } from "./components/investor-image/investor-image.component";
import { CountdownModule } from "@ciri/ngx-countdown";
import { DragDropModule } from "@angular/cdk/drag-drop";
import { CardDashboardOverviewComponent } from "./home/card-dashboard-overview/card-dashboard-overview.component";
import { CardDashboardGalleriaComponent } from "./home/card-dashboard-galleria/card-dashboard-galleria.component";
import { RecentActivityComponent } from "./home/recent-activity/recent-activity.component";
import { NgxSpinnerModule } from "ngx-spinner";
import { NgxSkeletonLoaderModule } from "ngx-skeleton-loader";
import { FilterBusinessCustomerComponent } from './components/filter-business-customer/filter-business-customer.component';

FullCalendarModule.registerPlugins([
	dayGridPlugin,
	timeGridPlugin,
	interactionPlugin,
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
		CountdownModule,
		DragDropModule,
		NgxSpinnerModule,
		NgxSkeletonLoaderModule.forRoot(),
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
		UploadImageComponent,
		FormRequestComponent,
		FormApproveComponent,
		FormCancelComponent,
		FormCloseComponent,
		LoginUrlComponent,
		FormSetDisplayColumnComponent,
		FormViewPdfFileComponent,
		FormNotificationComponent,
		OperationalReportComponent,
		BusinessReportComponent,
		ManagementReportComponent,
		UploadMediaComponent,
		SelectIconComponent,
		InvestorImageComponent,
		CardDashboardOverviewComponent,
		CardDashboardGalleriaComponent,
		RecentActivityComponent,
  		FilterBusinessCustomerComponent,
	],

	providers: [
		{ provide: LocationStrategy, useClass: HashLocationStrategy },
		MenuService,
		BreadcrumbService,
		OrderService,
		DialogService,
		ContractTemplateServiceProxy,
		DynamicDialogRef,
		DynamicDialogConfig,
		ConfirmationService
	],
	bootstrap: [AppComponent],
})
export class AppModule {}

import { FormButtonComponent } from './components/form-button/form-button.component';
import { FormCardInforComponent } from './components/form-card-infor/form-card-infor.component';
import { FormCheckboxComponent } from './components/form-checkbox/form-checkbox.component';
import { FormDescriptionContentComponent } from './components/form-description-content/form-description-content.component';
import { FormDownloadTemplateComponent } from './components/form-download-template/form-download-template.component';
import { FormDropdownComponent } from './components/form-dropdown/form-dropdown.component';
import { FormEditorComponent } from './components/form-editor/form-editor.component';
import { FormImageComponent } from './components/form-image/form-image.component';
import { FormInputCalendarComponent } from './components/form-input-calendar/form-input-calendar.component';
import { FormInputMaskComponent } from './components/form-input-mask/form-input-mask.component';
import { FormInputNumberComponent } from './components/form-input-number/form-input-number.component';
import { FormInputSearchComponent } from './components/form-input-search/form-input-search.component';
import { FormInputSwitchComponent } from './components/form-input-switch/form-input-switch.component';
import { FormInputComponent } from './components/form-input/form-input.component';
import { FormMarkdownComponent } from './components/form-markdown/form-markdown.component';
import { FormMenuComponent } from './components/form-menu/form-menu.component';
import { FormMultiSelectComponent } from './components/form-multi-select/form-multi-select.component';
import { FormNotificationDescriptionComponent } from './components/form-notification-description/form-notification-description.component';
import { FormRadioButtonComponent } from './components/form-radio-button/form-radio-button.component';
import { FormSearchDataComponent } from './components/form-search-data/form-search-data.component';
import { FormSelectButtonComponent } from './components/form-select-button/form-select-button.component';
import { FormSetDisplayColumnComponent } from './components/form-set-display-column/form-set-display-column.component';
import { FormStepsComponent } from './components/form-steps/form-steps.component';
import { FormTabViewComponent } from './components/form-tab-view/form-tab-view.component';
import { FormTableComponent } from './components/form-table/form-table.component';
import { FormTextareaComponent } from './components/form-textarea/form-textarea.component';
import { FormUploadFileComponent } from './components/form-upload-file/form-upload-file.component';
import { HoldOrLockTicketCustomerComponent } from './components/hold-or-lock-ticket-customer/hold-or-lock-ticket-customer.component';
import { SpinnerLoadingComponent } from './components/spinner-loading/spinner-loading.component';
import { UploadImageComponent } from './components/upload-image/upload-image.component';
import { GenerateComponentDirective } from './directives/generate-component.directive';
import { BaseApiUrlPipe } from './pipes/baseApiUrl.pipe';
import { DateTimeViewPipe } from './pipes/datetimeview.pipe';
import { DateViewPipe } from './pipes/dateview.pipe';
import { FormatCurrencyPipe } from './pipes/formatCurrency.pipe';
import { TruncatePipe } from './pipes/truncate.pipe';
import { ValueFormatterPipe } from './pipes/valueFormatter.pipe';
import { BillRequestListService } from './services/bill-request-list.service';
import { BroadcastService } from './services/broadcast.service';
import { EventOverviewService } from './services/event-overview.service';
import { RouterService } from './services/router.service';
import { SaleTicketOrderService } from './services/sale-ticket-order.service';
import { SettingContractCodeService } from './services/setting-contract-code-service';
import { SpinnerService } from './services/spinner.service';
import { TicketRequestListService } from './services/ticket-request-list.service';

export const commonComponents: any[] = [
  FormDropdownComponent,
  FormInputComponent,
  FormTabViewComponent,
  FormInputCalendarComponent,
  FormInputNumberComponent,
  FormImageComponent,
  FormSelectButtonComponent,
  FormButtonComponent,
  FormTextareaComponent,
  FormEditorComponent,
  FormMarkdownComponent,
  FormDescriptionContentComponent,
  FormMenuComponent,
  FormUploadFileComponent,
  FormDownloadTemplateComponent,
  FormTableComponent,
  FormNotificationDescriptionComponent,
  FormRadioButtonComponent,
  FormStepsComponent,
  FormInputSwitchComponent,
  UploadImageComponent,
  FormSetDisplayColumnComponent,
  FormMultiSelectComponent,
  FormCheckboxComponent,
  HoldOrLockTicketCustomerComponent,
  FormCardInforComponent,
  FormInputMaskComponent,
  SpinnerLoadingComponent,
  FormSearchDataComponent,
  FormInputSearchComponent,
];

export const services: any[] = [
  BroadcastService,
  RouterService,
  SpinnerService,
  SettingContractCodeService,
  EventOverviewService,
  SaleTicketOrderService,
  TicketRequestListService,
  BillRequestListService,
];

export const directives: any[] = [GenerateComponentDirective];

export const pipes: any[] = [
  DateViewPipe,
  DateTimeViewPipe,
  TruncatePipe,
  FormatCurrencyPipe,
  BaseApiUrlPipe,
  ValueFormatterPipe,
];

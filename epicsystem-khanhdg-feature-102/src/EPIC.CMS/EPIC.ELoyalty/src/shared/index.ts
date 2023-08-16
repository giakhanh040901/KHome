import { ECalendarComponent } from './components/e-calendar/e-calendar.component';
import { FormButtonComponent } from './components/form-button/form-button.component';
import { FormChangeVoucherRequestItemComponent } from './components/form-change-voucher-request-item/form-change-voucher-request-item.component';
import { FormDescriptionContentComponent } from './components/form-description-content/form-description-content.component';
import { FormDownloadTemplateComponent } from './components/form-download-template/form-download-template.component';
import { FormDropdownComponent } from './components/form-dropdown/form-dropdown.component';
import { FormEditorComponent } from './components/form-editor/form-editor.component';
import { FormImageComponent } from './components/form-image/form-image.component';
import { FormInputCalendarComponent } from './components/form-input-calendar/form-input-calendar.component';
import { FormInputNumberComponent } from './components/form-input-number/form-input-number.component';
import { FormInputSearchComponent } from './components/form-input-search/form-input-search.component';
import { FormInputSwitchComponent } from './components/form-input-switch/form-input-switch.component';
import { FormInputComponent } from './components/form-input/form-input.component';
import { FormMarkdownComponent } from './components/form-markdown/form-markdown.component';
import { FormMenuComponent } from './components/form-menu/form-menu.component';
import { FormNotificationDescriptionComponent } from './components/form-notification-description/form-notification-description.component';
import { FormRadioButtonComponent } from './components/form-radio-button/form-radio-button.component';
import { FormSearchCustomerComponent } from './components/form-search-customer/form-search-customer.component';
import { FormSelectButtonComponent } from './components/form-select-button/form-select-button.component';
import { FormStepsComponent } from './components/form-steps/form-steps.component';
import { FormTabViewComponent } from './components/form-tab-view/form-tab-view.component';
import { FormTableComponent } from './components/form-table/form-table.component';
import { FormTextareaComponent } from './components/form-textarea/form-textarea.component';
import { FormUploadFileComponent } from './components/form-upload-file/form-upload-file.component';
import { GenerateComponentDirective } from './directives/generate-component.directive';
import { BaseApiUrlPipe } from './pipes/baseApiUrl.pipe';
import { DateTimeViewPipe } from './pipes/datetimeview.pipe';
import { DateViewPipe } from './pipes/dateview.pipe';
import { FormatCurrencyPipe } from './pipes/formatCurrency.pipe';
import { SexNamePipe } from './pipes/sexName.pipe';
import { TruncatePipe } from './pipes/truncate.pipe';
import { ValueFormatterPipe } from './pipes/valueFormatter.pipe';
import { AccumulatePointManagementService } from './services/accumulate-point-management-service';
import { BroadcastService } from './services/broadcast.service';
import { ChangeVoucherRequestService } from './services/change-voucher-request-service';
import { ExportReportService } from './services/export-report.service';
import { GetCustomerService } from './services/get-customer-service';
import { HelpersService } from './services/helpers.service';
import { IndividualCustomerService } from './services/individual-customer-service';
import { LuckyProgramInvestorService } from './services/lucky-program-investor.service';
import { LuckyScenarioService } from './services/lucky-scenario-service';
import { MembershipLevelManagementService } from './services/membership-level-management-service';
import { NotificationExtendService } from './services/notification-extend.service';
import { NotificationService } from './services/notification.service';
import { PrizeDrawService } from './services/prize-draw.service';
import { VoucherManagementService } from './services/voucher-management-service';

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
  FormSearchCustomerComponent,
  FormNotificationDescriptionComponent,
  FormRadioButtonComponent,
  FormStepsComponent,
  FormInputSwitchComponent,
  FormChangeVoucherRequestItemComponent,
  FormInputSearchComponent,
  ECalendarComponent
];

export const services: any[] = [
  IndividualCustomerService,
  VoucherManagementService,
  AccumulatePointManagementService,
  MembershipLevelManagementService,
  ChangeVoucherRequestService,
  GetCustomerService,
  BroadcastService,
  NotificationExtendService,
  NotificationService,
  ExportReportService,
  HelpersService,
  PrizeDrawService,
  LuckyScenarioService,
  LuckyProgramInvestorService
];

export const directives: any[] = [GenerateComponentDirective];

export const pipes: any[] = [
  DateViewPipe,
  DateTimeViewPipe,
  TruncatePipe,
  FormatCurrencyPipe,
  BaseApiUrlPipe,
  ValueFormatterPipe,
  SexNamePipe
];

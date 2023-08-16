// import { SharedModule } from './../shared/shared.module';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { APP_BASE_HREF, CommonModule, HashLocationStrategy, LocationStrategy } from '@angular/common';

// PrimeNG Components for demos
// import { AccordionModule } from 'primeng/accordion';
import { AutoCompleteModule } from 'primeng/autocomplete';
// import { AvatarModule } from 'primeng/avatar';
// import { AvatarGroupModule } from 'primeng/avatargroup';
// import { BadgeModule } from 'primeng/badge';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { CardModule } from 'primeng/card';
// import { CarouselModule } from 'primeng/carousel';
// import { CascadeSelectModule } from 'primeng/cascadeselect';
// import { ChartModule } from 'primeng/chart';
import { CheckboxModule } from 'primeng/checkbox';
// import { ChipModule } from 'primeng/chip';
// import { ChipsModule } from 'primeng/chips';
// import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
// import { ColorPickerModule } from 'primeng/colorpicker';
// import { ContextMenuModule } from 'primeng/contextmenu';
// import { DataViewModule } from 'primeng/dataview';
import { DialogModule } from 'primeng/dialog';
// import { DividerModule } from 'primeng/divider';
import { DropdownModule } from 'primeng/dropdown';
// import { FieldsetModule } from 'primeng/fieldset';
// import { FileUploadModule } from 'primeng/fileupload';
import { FullCalendarModule } from '@fullcalendar/angular';
// import { GalleriaModule } from 'primeng/galleria';
import { ImageModule } from 'primeng/image';
// import { InplaceModule } from 'primeng/inplace';
import { InputNumberModule } from 'primeng/inputnumber';
// import { InputMaskModule } from 'primeng/inputmask';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
// import { KnobModule } from 'primeng/knob';
// import { LightboxModule } from 'primeng/lightbox';
// import { ListboxModule } from 'primeng/listbox';
// import { MegaMenuModule } from 'primeng/megamenu';
import { MenuModule } from 'primeng/menu';
// import { MenubarModule } from 'primeng/menubar';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { MultiSelectModule } from 'primeng/multiselect';
// import { OrderListModule } from 'primeng/orderlist';
// import { OrganizationChartModule } from 'primeng/organizationchart';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { PaginatorModule } from 'primeng/paginator';
// import { PanelModule } from 'primeng/panel';
// import { PanelMenuModule } from 'primeng/panelmenu';
import { PasswordModule } from 'primeng/password';
// import { PickListModule } from 'primeng/picklist';
import { ProgressBarModule } from 'primeng/progressbar';
import { RadioButtonModule } from 'primeng/radiobutton';
// import { RatingModule } from 'primeng/rating';
// import { RippleModule } from 'primeng/ripple';
// import { ScrollPanelModule } from 'primeng/scrollpanel';
// import { ScrollTopModule } from 'primeng/scrolltop';
import { SelectButtonModule } from 'primeng/selectbutton';
// import { SidebarModule } from 'primeng/sidebar';
// import { SkeletonModule } from 'primeng/skeleton';
// import { SlideMenuModule } from 'primeng/slidemenu';
// import { SliderModule } from 'primeng/slider';
// import { SplitButtonModule } from 'primeng/splitbutton';
// import { SplitterModule } from 'primeng/splitter';
// import { StepsModule } from 'primeng/steps';
// import { TabMenuModule } from 'primeng/tabmenu';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { TagModule } from 'primeng/tag';
// import { TerminalModule } from 'primeng/terminal';
// import { TieredMenuModule } from 'primeng/tieredmenu';
// import { TimelineModule } from 'primeng/timeline';
import { ToastModule } from 'primeng/toast';
// import { ToggleButtonModule } from 'primeng/togglebutton';
// import { ToolbarModule } from 'primeng/toolbar';
import { TooltipModule } from 'primeng/tooltip';
import { TreeModule } from 'primeng/tree';
// import { TreeTableModule } from 'primeng/treetable';
// import { VirtualScrollerModule } from 'primeng/virtualscroller';

// Application Components
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AppMainComponent } from './layout/main/app.main.component';
import { AppMenuComponent } from './layout/menu/app.menu.component';
import { AppMenuitemComponent } from './layout/menu/app.menuitem.component';
import { AppBreadcrumbComponent } from './layout/breadcrumb/app.breadcrumb.component';
import { AppTopBarComponent } from './layout/top-bar/app.topbar.component';
import { AppFooterComponent } from './layout/footer/app.footer.component';

// import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ReactiveFormsModule } from '@angular/forms';

// Application services
import { BreadcrumbService } from './layout/breadcrumb/breadcrumb.service';
import { MenuService } from './layout/menu/app.menu.service';

import { SharedModule } from '@shared/shared.module';

import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import { HomeComponent } from './home/home.component';
import { DialogService, DynamicDialogModule } from 'primeng/dynamicdialog';
import { KeyFilterModule } from 'primeng/keyfilter';
import { WebsiteComponent } from './website/website.component';
import { WebDetailComponent } from './website/web-detail/web-detail.component';
import { CreateOrUpdateRoleComponent } from './website/web-detail/create-or-update-role/create-or-update-role.component';
import { TradingProviderDetailComponent } from './trading-provider/trading-provider-detail/trading-provider-detail.component';
import { FormSetDisplayColumnComponent } from './form-set-display-column/form-set-display-column.component';
import { UserManagerComponent } from './user-manager/user-manager.component';
import { CreateUserComponent } from './user-manager/create-user/create-user.component';
import { FormNotificationComponent } from './form-notification/form-notification.component';
import { LoginUrlComponent } from './login-url/login-url.component';
import { PartnerDetailComponent } from './partner-manager/partner-detail/partner-detail.component';
import { AddRoleUserComponent } from './user-manager/add-role-user/add-role-user.component';
import { SettingPermissionComponent } from './user-manager/setting-permission/setting-permission.component';
import { WebsitesComponent } from './user-manager/setting-permission/websites/websites.component';
import { WebsiteDetailComponent } from './user-manager/setting-permission/website-detail/website-detail.component';

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
        //PrimengModule
         // AccordionModule,
        AutoCompleteModule,
        // AvatarModule,
        // AvatarGroupModule,
        // BadgeModule,
        // CodeHighlighterModule,
        // FullCalendarModule,
        // CarouselModule,
        // CascadeSelectModule,
        // ChipModule,
        // ChipsModule,
        // ColorPickerModule,
        // ContextMenuModule,
        // DataViewModule,
        // FieldsetModule,
        // GalleriaModule,
        // InplaceModule,
        // InputMaskModule,
        // KnobModule,
        // LightboxModule,
        // MegaMenuModule,
        // MenubarModule,
        // OrderListModule,
        // OrganizationChartModule,
        // PanelModule,
        // PanelMenuModule,
        PasswordModule,
        // PickListModule,
        // RatingModule,
        // RippleModule,
        // ScrollPanelModule,
        // ScrollTopModule,
        // SidebarModule,
        // SkeletonModule,
        // SlideMenuModule,
        // SplitButtonModule,
        // SplitterModule,
        // TabMenuModule,
        // TerminalModule,
        // TimelineModule,
        // TieredMenuModule,
        // ToggleButtonModule,
        // ToolbarModule,
        TreeModule,
        // TreeTableModule,
        // VirtualScrollerModule,
        // ProgressSpinnerModule,
        OverlayPanelModule,
        TagModule,
        CardModule,
        BreadcrumbModule,
        ButtonModule,
        CalendarModule,
        // ChartModule,
        CheckboxModule,
        ConfirmDialogModule,
        ConfirmPopupModule,
        DialogModule,
        // DividerModule,
        DropdownModule,
        // FileUploadModule,
        ImageModule,
        InputNumberModule,
        InputSwitchModule,
        InputTextModule,
        InputTextareaModule,
        // ListboxModule,
        MenuModule,
        MessageModule,
        MessagesModule,
        MultiSelectModule,
        PaginatorModule,
        ProgressBarModule,
        RadioButtonModule,
        SelectButtonModule,
        // SliderModule,
        // StepsModule,
        TableModule,
        TabViewModule,
        ToastModule,
        TooltipModule,
        DynamicDialogModule,
        KeyFilterModule,
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
        WebsiteComponent,
        WebDetailComponent,
        CreateOrUpdateRoleComponent,
        TradingProviderDetailComponent,
        FormSetDisplayColumnComponent,
        UserManagerComponent,
        CreateUserComponent,
        FormNotificationComponent,
        LoginUrlComponent,
        PartnerDetailComponent,
        AddRoleUserComponent,
        SettingPermissionComponent,
        WebsitesComponent,
        WebsiteDetailComponent,
    ],
    providers: [
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        MenuService,
        BreadcrumbService,
        DialogService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}

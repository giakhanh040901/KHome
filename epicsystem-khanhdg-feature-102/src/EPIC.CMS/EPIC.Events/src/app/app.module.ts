import { CommonModule, HashLocationStrategy, LocationStrategy } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

// Application Components
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppBreadcrumbComponent } from './layout/breadcrumb/app.breadcrumb.component';
import { AppFooterComponent } from './layout/footer/app.footer.component';
import { AppMainComponent } from './layout/main/app.main.component';
import { AppMenuComponent } from './layout/menu/app.menu.component';
import { AppMenuitemComponent } from './layout/menu/app.menuitem.component';
import { AppTopBarComponent } from './layout/top-bar/app.topbar.component';

import { ReactiveFormsModule } from '@angular/forms';

// Application services
import { BreadcrumbService } from './layout/breadcrumb/breadcrumb.service';
import { MenuService } from './layout/menu/app.menu.service';

import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import timeGridPlugin from '@fullcalendar/timegrid';
import { SharedModule } from '@shared/shared.module';
import { HomeComponent } from './home/home.component';

import { DialogService } from 'primeng/dynamicdialog';

import { LoginUrlComponent } from './login-url/login-url.component';

import { FullCalendarModule } from '@fullcalendar/angular';
import * as index from './index';
import { SystemNotificationTemplateComponent } from './modules/setting/system-notification-template/system-notification-template.component';

FullCalendarModule.registerPlugins([dayGridPlugin, timeGridPlugin, interactionPlugin]);

@NgModule({
  imports: [SharedModule, CommonModule, FormsModule, AppRoutingModule, HttpClientModule, ...index.modules],

  declarations: [
    AppComponent,
    AppMainComponent,
    AppMenuComponent,
    AppMenuitemComponent,
    AppTopBarComponent,
    AppFooterComponent,
    AppBreadcrumbComponent,
    HomeComponent,
    LoginUrlComponent,
    SystemNotificationTemplateComponent,
  ],

  entryComponents: [],

  providers: [
    { provide: LocationStrategy, useClass: HashLocationStrategy },
    MenuService,
    BreadcrumbService,
    DialogService,
  ],

  bootstrap: [AppComponent],
})
export class AppModule {}

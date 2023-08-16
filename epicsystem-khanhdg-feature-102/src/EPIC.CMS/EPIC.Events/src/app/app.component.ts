import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService, PrimeNGConfig } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [DialogService, ConfirmationService, MessageService]
})
export class AppComponent implements OnInit {
  horizontalMenu: boolean;

  darkMode = false;

  menuColorMode = 'light';

  menuColor = 'layout-menu-light';

  themeColor = 'blue';

  layoutColor = 'blue';

  ripple = true;

  inputStyle = 'outlined';

  constructor(private primengConfig: PrimeNGConfig) {}

  ngOnInit() {
    this.primengConfig.ripple = true;
  }
}

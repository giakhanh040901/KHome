import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { CookieManagerService } from '@shared/services/cookie.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  submitting = false;
  dark: boolean;

  constructor(
      private injector: Injector,
      messageService: MessageService,
      private router?: Router,
  ) {
      // super(injector, messageService);
  }

  ngOnInit(): void {
  }

}

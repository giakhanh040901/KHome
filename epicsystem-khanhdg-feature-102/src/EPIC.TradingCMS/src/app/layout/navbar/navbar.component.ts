import { Component, Injector, OnInit } from '@angular/core';
import { CreateOrUpdateUserComponent } from '@app/user/create-or-update-user/create-or-update-user.component';
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';
import { AppComponentBase } from '@shared/app-component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { SessionServiceProxy, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppSessionService } from '@shared/session/app-session.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  providers: [NgbDropdownConfig]
})
export class NavbarComponent extends AppComponentBase  implements OnInit {

  public iconOnlyToggled = false;
  public sidebarToggled = false;
  userInfo :any = {};
  toggleSidebar() {
    let body = document.querySelector('body');
    if((!body.classList.contains('sidebar-toggle-display')) && (!body.classList.contains('sidebar-absolute'))) {
      this.iconOnlyToggled = !this.iconOnlyToggled;
      if(this.iconOnlyToggled) {
        body.classList.add('sidebar-icon-only');
      } else {
        body.classList.remove('sidebar-icon-only');
      }
    } else {
      this.sidebarToggled = !this.sidebarToggled;
      if(this.sidebarToggled) {
        body.classList.add('sidebar-hidden');
      } else {
        body.classList.remove('sidebar-hidden');
      }
    }
  }

  constructor(
    private injector: Injector,
    private config: NgbDropdownConfig,
    private _authService: AppAuthService,
    private _appSessionService: AppSessionService,
    private _modalService: BsModalService,
    private _userService: UserServiceProxy,
   )
    {
        super(injector);
        config.placement = 'bottom-right';

    }



  ngOnInit() {
    if(!abp.auth.getToken()) {
      this.logout();
    }
    this.userInfo = this._appSessionService.user;
  }

  closeSettingsSidebar() {
    document.querySelector('#right-sidebar').classList.toggle('open');
  }

  focusInput() {
    const navbarSearchInput = <HTMLElement>document.querySelector('#navbar-search-input');
    navbarSearchInput.focus();
  }

  toggleRightSidebar() {
    document.querySelector('.sidebar-offcanvas').classList.toggle('active');
  }

  // Update User
  update(userId: number) {
    this.showEditDialog(userId);
  }

  private showEditDialog(userId: number): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateUserComponent,
        {
            class: 'modal-sm',
            initialState: {
                user: {
                  userId: userId,
                },
                title: 'Cập nhật tài khoản',
            },
        }
    );
    dialog.content.onSave.subscribe(() => {
      this._userService.getById(this.userInfo.userId).subscribe((res) => {
        this.userInfo = res.data;
        console.log(res.data);
      },(err) => {
        console.log({ errorUpdateUser: err });
      });
    });
  }

  logout() {
    this._authService.logout();
  }
}

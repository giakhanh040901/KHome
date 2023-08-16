import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {

  public uiBondManager = false;
  public uiSetting = false;

  public urlUi = {
    'setting' : 'uiSetting',
    'bond-manager' : 'uiBondManager',
  };
  constructor() { }

  ngOnInit() {
    // open dropDownMenu đang avtive khi refresh trang
    for (const [key, value] of Object.entries(this.urlUi)) {
      if(location.pathname.includes(key)) {
        this[value] = true;
      }
    }

    const body = document.querySelector('body');

    // add class 'hover-open' to sidebar navitem while hover in sidebar-icon-only menu
    document.querySelectorAll('.sidebar .nav-item').forEach(function (el) {
      el.addEventListener('mouseover', function() {
        if(body.classList.contains('sidebar-icon-only')) {
          el.classList.add('hover-open');
        }
      });
      el.addEventListener('mouseout', function() {
        if(body.classList.contains('sidebar-icon-only')) {
          el.classList.remove('hover-open');
        }
      });
    });

  }
  // Đóng các dropdownMenu đang mở khi thay đổi menu
  hideOtherDropdownMenu(uiName?: string) {
    for (const [key, value] of Object.entries(this.urlUi)) {
      if(uiName !== key) {
        this[value] = false;
      }
    }
  }
}

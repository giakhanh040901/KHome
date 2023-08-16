import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
   
  }
  uploadDone() {
    var authToken = 'YmVteN9Fv1lj3-fc8Yz6A49vn8617-ovPtCQCDGbpEQ';
    
  var iframe = document.getElementById('myiframe');
    if (iframe == null) return;
    var iWindow = (<HTMLIFrameElement>iframe).contentWindow;

    window.parent.postMessage({
      externalCommand: 'login-with-token',
      loginToken: authToken
  }, 'http://epic-home.stecom.vn:3000');
  console.log("OK")
  }

}

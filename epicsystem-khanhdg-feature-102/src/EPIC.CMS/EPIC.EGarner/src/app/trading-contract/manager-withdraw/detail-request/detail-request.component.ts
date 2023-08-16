import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-detail-request',
  templateUrl: './detail-request.component.html',
  styleUrls: ['./detail-request.component.scss']
})
export class DetailRequestComponent implements OnInit {

  constructor(
    public configDialog: DynamicDialogConfig,
  ) { }

  request: any = {};

  ngOnInit(): void {

    this.request = this.configDialog.data.info;
    console.log('info___', this.request);
    
  }

}

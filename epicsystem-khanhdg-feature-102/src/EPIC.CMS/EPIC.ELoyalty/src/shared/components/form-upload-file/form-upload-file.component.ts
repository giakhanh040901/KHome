import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'form-upload-file',
  templateUrl: './form-upload-file.component.html',
  styleUrls: ['./form-upload-file.component.scss'],
})
export class FormUploadFileComponent implements OnInit {
  @Input()
  public classFileUpload: string = '';
  @Input()
  public type: number;
  @Input()
  public icon: string = 'pi pi-upload';
  @Input()
  public label: string = '';
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {}

  public handleUpload(event: any) {
    if (event && event.files && event.files.length && event.files[0]) {
      this._onChange.emit(event);
    }
  }
}

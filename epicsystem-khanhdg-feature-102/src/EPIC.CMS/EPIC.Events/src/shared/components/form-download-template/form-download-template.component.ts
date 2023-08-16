import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'form-download-template',
  templateUrl: './form-download-template.component.html',
  styleUrls: ['./form-download-template.component.scss'],
})
export class FormDownloadTemplateComponent implements OnInit {
  @Input()
  public href: string = '';
  @Input()
  public download: string = '';
  @Input()
  public label: string = '';
  @Input()
  public icon: string = 'pi pi-download';

  constructor() {}

  ngOnInit() {}
}

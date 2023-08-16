import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'form-markdown',
  templateUrl: './form-markdown.component.html',
  styleUrls: ['./form-markdown.component.scss'],
})
export class FormMarkdownComponent implements OnInit {
  @Input()
  public classContainer: string = '';
  @Input()
  public classLabel: string = '';
  @Input()
  public showLabel: boolean = true;
  @Input()
  public label: string = '';
  @Input()
  public isRequired: boolean = false;
  @Input()
  public classMarkdown: string = '';
  @Input()
  public data: string = '';
  constructor() {}

  ngOnInit() {}
}

import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AppConsts, HTML_MARKDOWN_OPTIONS, MARKDOWN_OPTIONS, SearchConst } from '@shared/AppConsts';
import { IDescriptionContent, ISelectButton } from '@shared/interface/InterfaceConst.interface';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { UploadImageComponent } from '../upload-image/upload-image.component';

@Component({
  selector: 'form-description-content',
  templateUrl: './form-description-content.component.html',
  styleUrls: ['./form-description-content.component.scss'],
})
export class FormDescriptionContentComponent implements OnInit {
  @Input()
  public title: string = '';
  @Input()
  public classTitle: string = '';
  public htmlMarkdownOptions: ISelectButton[] = HTML_MARKDOWN_OPTIONS;
  @Input()
  public contentType: string = '';
  @Input()
  public content: string = '';
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public showBtnInsertImage: boolean = true;
  @Input()
  public classMarkdown: string = '';
  @Input()
  public rowsTextarea: number = 11;
  @Input()
  public heightEditor: string = '';
  private caretPos: number = 0;
  public baseUrl: string = '';
  @Output()
  public _onChangeData: EventEmitter<IDescriptionContent | undefined> = new EventEmitter<
    IDescriptionContent | undefined
  >();
  private subjectChangeInput: Subject<any> = new Subject();

  constructor(private dialogService: DialogService) {}

  public get MARKDOWN_OPTIONS() {
    return MARKDOWN_OPTIONS;
  }

  ngOnInit() {
    this.baseUrl = AppConsts.remoteServiceBaseUrl;
  }

  ngAfterViewInit() {
    this.subjectChangeInput.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe((res) => {
      if (res) {
        this.emitData();
      }
    });
  }

  public getCaretPos(oField) {
    if (oField.selectionStart || oField.selectionStart == '0') {
      this.caretPos = oField.selectionStart;
    }
  }

  public insertImageContent(event: any) {
    if (event) {
      const ref = this.dialogService.open(UploadImageComponent, {
        data: {
          inputData: [],
          showOrder: false,
        },
        header: 'Chèn hình ảnh',
        width: '600px',
        footer: '',
      });
      ref.onClose.subscribe((images) => {
        let imagesUrl = '';
        images?.forEach((image) => {
          imagesUrl += `![](${this.baseUrl}/${image.data}) \n`;
        });

        let oldContentValue = this.content;
        let a = oldContentValue.slice(0, this.caretPos) + imagesUrl + oldContentValue.slice(this.caretPos);
        this.content = a;
        this.emitData();
      });
    }
  }

  public get displayContent() {
    if (this.content) return this.content;
    return 'Nội dung hiển thị';
  }

  public onChangeContentType(event: any) {
    if (event) {
      this.emitData();
    }
  }

  public emitData() {
    this._onChangeData.emit({
      contentType: this.contentType,
      content: this.content,
    } as IDescriptionContent);
  }

  public handleInput(event: any) {
    this.subjectChangeInput.next(event);
  }
}

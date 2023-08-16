import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AbstractControl } from '@angular/forms';
import { HelpersService } from '@shared/services/helpers.service';
import { ContentTypeEView } from '@shared/consts/base.const';

@Component({
  selector: 'e-markdown-html',
  templateUrl: './e-markdown-html.component.html',
  styleUrls: ['./e-markdown-html.component.scss']
})
export class EMarkdownHtmlComponent implements OnInit {

    constructor(
        private _helpersService: HelpersService,
        private ref: ChangeDetectorRef,
    ) { }

    htmlMarkdownOptions: any = [
        {
            value: ContentTypeEView.MARKDOWN,
            name: ContentTypeEView.MARKDOWN,
        },
        {
            value: ContentTypeEView.HTML,
            name: ContentTypeEView.HTML
        }
    ]
    
    @Input() disabled: boolean = false;
    @Input() id: string = 'markdowm-html';
    @Input() title: string = 'Loại nội dung';
    @Input() labelContent: string = 'Nội dung';
    @Input() labelContentView: string = 'Nội dung xem trước';
    @Input() height: number;

    @Input() contentType: ContentTypeEView;
    @Output() contentTypeChange = new EventEmitter<string>();

    @Input() content: string;
    @Output() contentChange = new EventEmitter<string>();

    @Input() formControlValue: AbstractControl;
    @Input() formControlValueChange = new EventEmitter<AbstractControl>();

    heightHtml: number;
    heightMarkdown: number;
    textareaRow: number = 13;

    baseUrl: string;
    caretPos: number = 0;

    ngOnInit(): void {
        this.baseUrl = AppConsts.remoteServiceBaseUrl;
    }

    ngAfterViewInit() {
        this.setHeightContent();
    }

    setHeightContent() {
        const elementMarkdownHtml: any = document.getElementById(this.id);
        // TÍNH CHIỀU CAO CỦA MARKDOWN VÀ SET CHO EDITOR_HTML
        // NẾU HIỆN HTML TRƯỚC THÌ SET HEIGHT VÀ DÙNG HEIGHT SET CHO CẢ MARK_DOWN_EDITOR
        let elementTextarea: HTMLElement = elementMarkdownHtml.querySelector(".p-inputtextarea");
        if(elementTextarea) {
            if(this.height) elementTextarea.style.height = this.height+'px';
            this.heightMarkdown = this.height || elementTextarea.offsetHeight;
        }
        const angularEditorToolbar: HTMLElement = elementMarkdownHtml.querySelector(".angular-editor-toolbar");
        if(angularEditorToolbar) {
            this.heightHtml = (this.height || this.heightMarkdown) - angularEditorToolbar.offsetHeight;
        }
        // console.log('heightMarkdown', elementTextarea, this.id, angularEditorToolbar?.offsetHeight);
        this.ref.detectChanges();
    }

    onChange(contentType) {
        this.contentTypeChange.emit(contentType);
        setTimeout(() => {
            this.setHeightContent();
        }, 0);
        
    }

    insertImage() {
        const ref= this._helpersService.dialogUploadImagesRef('project-media');
        ref.onClose.subscribe((images: string[]) => {
            if(images && images?.length) {
                let imagesUrl = "";
                images.forEach(image => {
                    imagesUrl +=  `![](${this.baseUrl}/${image}) \n`;
                });
                let currentContent = this.content;
                let newContent = currentContent.slice(0, this.caretPos) 
                                + imagesUrl 
                                + currentContent.slice(this.caretPos); 
                this.content = newContent;
                this.emitContent();
            }
        })
    }

    emitContent(value?: string) {
        this.contentChange.emit(value || this.content);
        if(this.formControlValue) {
            this.formControlValue.setValue(value || this.content);
            this.formControlValueChange.emit(this.formControlValue);
        }
    }

    previewContent() {
        this._helpersService.dialogViewerRef(
            this.content,
            this.contentType,
        );
    }

    getCaretPos(event) {
        if (event.target.selectionStart || event.target.selectionStart == '0') {
            this.caretPos = event.target.selectionStart;
        }
        this.emitContent(event.target.value);
    }
}

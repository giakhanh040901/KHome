import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { AppConsts, DEFAULT_HEIGHT, DEFAULT_WIDTH } from '@shared/AppConsts';
import { IImage } from '@shared/interface/InterfaceConst.interface';
import { DialogService } from 'primeng/dynamicdialog';
import { UploadImageComponent } from 'src/app/components-general/upload-image/upload-image.component';

@Component({
  selector: 'form-image',
  templateUrl: './form-image.component.html',
  styleUrls: ['./form-image.component.scss'],
})
export class FormImageComponent implements OnInit {
  @Input()
  public classContainer: string = '';
  @Input()
  public imageSource: IImage;
  @Input()
  public widthLimit: number = DEFAULT_WIDTH;
  @Input()
  public heightLimit: number = DEFAULT_HEIGHT;
  public src: string = '';
  public width: number | string;
  public height: number | string;
  @Input()
  public classImage: string = '';
  @Input()
  public isDisabled: boolean = false;
  public baseUrl: string = '';
  @Input()
  public maxFileSize: number = 0;
  @Output()
  public _onChange: EventEmitter<IImage | undefined> = new EventEmitter<IImage | undefined>();

  constructor(private dialogService: DialogService) {}

  ngOnInit() {
    this.baseUrl = AppConsts.remoteServiceBaseUrl;
    console.log('!!!! imageSource: ', this.imageSource);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.imageSource) {
      this.src = this.imageSource.src;
      this.width = this.imageSource.width || DEFAULT_WIDTH;
      this.height = this.imageSource.height || DEFAULT_HEIGHT;
    }
  }

  public insertImage(event: any) {
    if (event && !this.isDisabled) {
      const ref = this.dialogService.open(UploadImageComponent, {
        data: {
          showOrder: false,
          isImage: true,
          maxFileSize: this.maxFileSize,
        },
        header: 'Chèn hình ảnh',
        width: '600px',
        footer: '',
      });
      ref.onClose.subscribe((images) => {
        if (images) {
          let imagesUrl = '';
          images?.forEach((image) => {
            imagesUrl += `${this.baseUrl}/${image.data}`;
          });

          this.src = `${this.baseUrl}/${images && images.length ? images[0].data : ''}`;
          this.width = this.widthLimit || DEFAULT_WIDTH;
          this.height = this.height || DEFAULT_HEIGHT;
          this._onChange.emit({
            src: this.src,
            width: this.width,
            height: this.height,
          } as IImage);
        }
      });
    }
  }
}

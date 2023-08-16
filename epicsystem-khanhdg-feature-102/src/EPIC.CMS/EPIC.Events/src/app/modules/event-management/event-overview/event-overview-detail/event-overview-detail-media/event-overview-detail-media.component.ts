import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { EventOverview, FormNotificationConst } from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatUrlImage, getUrlImage } from '@shared/function-common';
import { IImage, INotiDataModal } from '@shared/interface/InterfaceConst.interface';
import { EventOverviewDetailMediaModel } from '@shared/interface/event-management/event-overview/EventOverviewDetailMediaModel';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { RouterService } from '@shared/services/router.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'event-overview-detail-media',
  templateUrl: './event-overview-detail-media.component.html',
  styleUrls: ['./event-overview-detail-media.component.scss'],
})
export class EventOverviewDetailMediaComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private changeDetectorRef: ChangeDetectorRef,
    private eventOverviewService: EventOverviewService,
    private routerService: RouterService
  ) {
    super(injector, messageService);
  }
  public dataSource: EventOverviewDetailMediaModel = new EventOverviewDetailMediaModel();
  public imgBackground = 'assets/layout/images/add-image-required-bg.svg';
  public bannerBackground = 'assets/layout/images/add-banner-bg.svg';
  public defaultImage: IImage = {
    src: '',
    width: 140,
    height: 140,
  };
  public limitSlide: number = 5;

  public get KEY_AVATAR() {
    return EventOverview.keyEventDetailMedia.AVATAR;
  }

  public get KEY_BANNER() {
    return EventOverview.keyEventDetailMedia.BANNER;
  }

  public get KEY_SLIDE() {
    return EventOverview.keyEventDetailMedia.SLIDE;
  }

  ngOnInit() {
    this.dataSource.avatar = {
      ...this.defaultImage,
      src: this.imgBackground,
    };
    this.dataSource.banner = {
      ...this.defaultImage,
      width: 280,
      src: this.bannerBackground,
    };
  }

  ngAfterViewInit() {
    this.getAllImage(this.getDataSourceSlide.bind(this));
  }

  public onChangeImage(event: any, key: string) {
    if (event) {
      let isEdit: boolean = false;
      if (key === this.KEY_AVATAR) {
        this.dataSource.avatar.src = event.src;
        isEdit = !!this.dataSource.avatar.id;
        this.getBodyToCallAPI(this.KEY_AVATAR, isEdit, this.dataSource.avatar.src, this.dataSource.avatar.id);
      } else if (key === this.KEY_BANNER) {
        isEdit = !!this.dataSource.banner.id;
        this.dataSource.banner.src = event.src;
        this.getBodyToCallAPI(this.KEY_BANNER, isEdit, this.dataSource.banner.src, this.dataSource.banner.id);
      }
    }
  }

  private getBodyToCallAPI(key: string, isEdit: boolean, src: string, id?: number, index?: number) {
    const body = !isEdit
      ? {
          location: key,
          eventMedias: [
            {
              eventId: this.eventOverviewService.eventId,
              urlImage: formatUrlImage(src),
            },
          ],
        }
      : {
          id: id,
          eventId: this.eventOverviewService.eventId,
          urlImage: formatUrlImage(src),
        };
    if (JSON.stringify(body) !== '{}') this.callAPIOrUpdate(body, key, isEdit, index);
  }

  public onRemove(event: any, index: number, image: IImage) {
    if (event) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Thông báo',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: 'Bạn có chắc chắn muốn xóa hình ảnh?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService.deleteEventDetailMedia(image.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
                this.dataSource.slide.splice(index, 1);
                this.getDataSourceSlide();
              }
            },
            (err) => {
              this.messageError(`Không xóa được`);
            }
          );
        }
      });
    }
  }

  public onChangeSlide(event: any, index: number) {
    if (event) {
      let isEdit: boolean = false;
      this.dataSource.slide[index].src = event.src;
      isEdit = !!this.dataSource.slide[index].id;
      this.getBodyToCallAPI(
        this.KEY_SLIDE,
        isEdit,
        this.dataSource.slide[index].src,
        this.dataSource.slide[index].id,
        index
      );
      this.getDataSourceSlide();
    }
  }

  public getShowBtnRemove(data: IImage) {
    if (data) {
      return data.src !== this.imgBackground;
    }
    return false;
  }

  public getDataSourceSlide() {
    if (this.dataSource.slide.length < this.limitSlide) {
      if (
        !this.dataSource.slide.length ||
        this.dataSource.slide[this.dataSource.slide.length - 1].src !== this.imgBackground
      ) {
        this.dataSource.slide.push({
          ...this.defaultImage,
          src: this.imgBackground,
        });
      }
    }
  }

  private callAPIOrUpdate(body: any, key: string, isEdit: boolean, index?: number) {
    if (body) {
      this.eventOverviewService.createOrUpdateEventDetailMedia(body, isEdit).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Lưu thành công')) {
            if (!isEdit) {
              if (response.data && response.data.length) {
                const data = response.data[0];
                if (key === this.KEY_AVATAR) {
                  this.dataSource.avatar = { ...this.dataSource.avatar, id: data.id, src: getUrlImage(data.urlImage) };
                } else if (key === this.KEY_BANNER) {
                  this.dataSource.banner = { ...this.dataSource.banner, id: data.id, src: getUrlImage(data.urlImage) };
                } else if (key === this.KEY_SLIDE) {
                  this.dataSource.slide[index] = {
                    ...this.dataSource.slide[index],
                    id: data.id,
                    src: getUrlImage(data.urlImage),
                  };
                }
              }
            } else {
              if (response.data) {
                const { data } = response;
                if (key === this.KEY_AVATAR) {
                  this.dataSource.avatar = { ...this.dataSource.avatar, id: data.id, src: getUrlImage(data.urlImage) };
                } else if (key === this.KEY_BANNER) {
                  this.dataSource.banner = { ...this.dataSource.banner, id: data.id, src: getUrlImage(data.urlImage) };
                } else if (key === this.KEY_SLIDE) {
                  this.dataSource.slide[index] = {
                    ...this.dataSource.slide[index],
                    id: data.id,
                    src: getUrlImage(data.urlImage),
                  };
                }
              }
            }
          }
        },
        (err) => {}
      );
    }
  }

  private getAllImage(callBack: Function) {
    this.eventOverviewService.getAllEventDetailMedia().subscribe((res) => {
      if (this.handleResponseInterceptor(res, '')) {
        if (res.data.items && res.data.items.length) {
          res.data.items.forEach((item: any) => {
            if (item.location === this.KEY_AVATAR) {
              this.dataSource.avatar = { ...this.dataSource.avatar, id: item.id, src: getUrlImage(item.urlImage) };
            } else if (item.location === this.KEY_BANNER) {
              this.dataSource.banner = { ...this.dataSource.banner, id: item.id, src: getUrlImage(item.urlImage) };
            } else if (item.location === this.KEY_SLIDE) {
              this.dataSource.slide.unshift({ ...this.defaultImage, id: item.id, src: getUrlImage(item.urlImage) });
            }
          });
          this.changeDetectorRef.detectChanges();
          this.changeDetectorRef.markForCheck();
        }
        callBack();
      }
    });
  }

  public get labelAvatar() {
    return `Ảnh đại diện sự kiện (${this.dataSource.avatar.src === this.imgBackground ? 0 : 1}/1)`;
  }
  public get labelBanner() {
    return `Banner sự kiện (${this.dataSource.banner.src === this.bannerBackground ? 0 : 1}/1)`;
  }
  public get labelSlide() {
    const length = this.dataSource.slide.filter((e: IImage) => e.src !== this.imgBackground).length;
    return `Slide ảnh giới thiệu sự kiện (${length}/${this.limitSlide})`;
  }
}

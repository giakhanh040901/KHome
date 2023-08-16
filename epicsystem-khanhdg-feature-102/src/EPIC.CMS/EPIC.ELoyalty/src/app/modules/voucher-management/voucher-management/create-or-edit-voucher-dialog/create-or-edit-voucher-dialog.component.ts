import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { EConfigDataModal, MARKDOWN_OPTIONS, VoucherManagement, YES_NO } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IDescriptionContent, IImage } from '@shared/interface/InterfaceConst.interface';
import { CreateOrEditVoucher } from '@shared/interface/voucher-management/voucher-management/VoucherManagement.model';
import { VoucherManagementService } from '@shared/services/voucher-management-service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

export const DEFAULT_IMAGE = 150;

export const DEFAULT_IMAGE_AVATAR = {
  width: DEFAULT_IMAGE,
  height: DEFAULT_IMAGE,
};

export const DEFAULT_IMAGE_BANNER = {
  width: DEFAULT_IMAGE * 2,
  height: DEFAULT_IMAGE,
};

@Component({
  selector: 'create-or-edit-voucher-dialog',
  templateUrl: './create-or-edit-voucher-dialog.component.html',
  styleUrls: ['./create-or-edit-voucher-dialog.component.scss'],
})
export class CreateOrEditVoucherDialogComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private router: Router,
    private ref: DynamicDialogRef,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig,
    private voucherManagementService: VoucherManagementService
  ) {
    super(injector, messageService);
  }
  public crudVoucher: CreateOrEditVoucher = new CreateOrEditVoucher();
  public keyword: string = '';
  public imgBackground = 'assets/layout/images/add-image-voucher-bg.svg';
  public bannerBackground = 'assets/layout/images/add-banner-voucher-bg.svg';
  public selectedImageVoucher: IImage;
  public selectedBannerVoucher: IImage;
  public type: string = EConfigDataModal.CREATE;

  public get listKindOfVoucher() {
    return VoucherManagement.listKindOfVoucher;
  }

  public get listTypeOfVoucher() {
    return VoucherManagement.listTypeOfVoucher;
  }

  public get listValueUnit() {
    return VoucherManagement.listValueUnit;
  }

  public get MARKDOWN_OPTIONS() {
    return MARKDOWN_OPTIONS;
  }

  public get CUSTOMER() {
    return VoucherManagement.CUSTOMER;
  }

  public get VND() {
    return VoucherManagement.VND;
  }

  public get PERCENT() {
    return VoucherManagement.PERCENT;
  }

  ngOnInit() {
    this.selectedImageVoucher = {
      src: this.imgBackground,
      ...DEFAULT_IMAGE_AVATAR,
    };
    this.selectedBannerVoucher = {
      src: this.bannerBackground,
      ...DEFAULT_IMAGE_BANNER,
    };
    if (this.config.data) {
      this.type = this.config.data.type;
    }
    if (this.type === EConfigDataModal.CREATE) {
      this.crudVoucher.kind = this.listKindOfVoucher[0].value.toString();
      this.crudVoucher.type = this.listTypeOfVoucher[0].value.toString();
      this.crudVoucher.valueUnit = this.VND;
      this.crudVoucher.applyDate = new Date();
      this.crudVoucher.contentType = MARKDOWN_OPTIONS.MARKDOWN;
    } else {
      this.crudVoucher.mapData(this.config.data.dataSource);
      this.crudVoucher.avatar &&
        (this.selectedImageVoucher = {
          src: this.crudVoucher.avatar,
          ...DEFAULT_IMAGE_AVATAR,
        });
      this.crudVoucher.banner &&
        (this.selectedBannerVoucher = {
          src: this.crudVoucher.banner,
          ...DEFAULT_IMAGE_BANNER,
        });
    }
  }

  public save(event?: any) {
    if (event) {
      if (this.crudVoucher.isValidData()) {
        this.voucherManagementService
          .createOrEditVoucher(this.crudVoucher.toObjectSendToAPI(), this.type === EConfigDataModal.EDIT)
          .subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, '')) {
                this.ref.close(response);
              }
            },
            (err) => {}
          );
      } else {
        const messageError = this.crudVoucher.dataValidator.length
          ? this.crudVoucher.dataValidator[0].message
          : undefined;
        messageError && this.messageError(messageError);
      }
    }
  }

  public close(event: any) {
    this.ref.close();
  }

  public get isDisable() {
    return this.type === EConfigDataModal.VIEW;
  }

  public get widthDivAvatar() {
    return `calc(${DEFAULT_IMAGE}px + 1rem)`;
  }

  public get widthDivBanner() {
    return `calc(${DEFAULT_IMAGE * 2}px + 1rem)`;
  }

  public get DEFAULT_IMAGE_SIZE() {
    return DEFAULT_IMAGE_AVATAR;
  }

  public get DEFAULT_IMAGE_BANNER() {
    return DEFAULT_IMAGE_BANNER;
  }

  public onChangeAvatar(event: IImage | undefined) {
    if (event) {
      this.selectedImageVoucher = event;
      this.crudVoucher.avatar = event.src;
    }
  }

  public onChangeBanner(event: IImage | undefined) {
    if (event) {
      this.selectedBannerVoucher = event;
      this.crudVoucher.banner = event.src;
    }
  }

  public onChangeValueUnit(event: any) {
    if (event) {
      this.crudVoucher.value = 0;
    }
  }

  public onChangeDescriptionContent(event: IDescriptionContent | undefined) {
    if (event) {
      this.crudVoucher.content = event.content;
      this.crudVoucher.contentType = event.contentType;
    }
  }

  public get YES_NO() {
    return YES_NO;
  }
}

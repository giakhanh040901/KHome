import { Component, Injector, Input } from '@angular/core';
import { IssuerConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { OwnerServiceProxy } from '@shared/services/owner-service';
import { NationalityConst } from '@shared/nationality-list';
@Component({
  selector: 'app-owner-detail',
  templateUrl: './owner-detail.component.html',
  styleUrls: ['./owner-detail.component.scss']
})
export class OwnerDetailComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private routeActive: ActivatedRoute,
        private ownerService: OwnerServiceProxy,
        private breadcrumbService: BreadcrumbService
    ) { 
        super(injector, messageService);
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Chủ đầu tư', routerLink: ['/setting/owner']  },
            { label: 'Chi tiết chủ đầu tư',},
        ]);

        this.id = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
    }

    @Input() contentHeights: number[] = [];
    //
    ownerUpdate = {
        "id" : 0,
        "businessCustomerId" : null,
        "partnerId" : null,
        "businessTurnover" : null,
        "businessProfit" : null,
        "roa" : null,
        "roe" : null,
        "image" : null,
        "website" : null,
        "hotline" : null,
        "fanpage" : null,
    }

    id: number;
    ownerDetail: any = {};
    isEdit = false;
    labelButtonEdit = "Chỉnh sửa";

    IssuerConst = IssuerConst;

    NationalityConst = NationalityConst;

    ngOnInit(): void {
        this.isLoading = true;
        this.ownerService.getOwner(this.id).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.ownerDetail = res.data;
                this.ownerDetail = {
                    ...this.ownerDetail,
                    licenseDate: this.ownerDetail?.licenseDate ? new Date(this.ownerDetail?.licenseDate): null,
                    decisionDate: this.ownerDetail?.decisionDate ? new Date(this.ownerDetail?.decisionDate): null,
                    dateModified: this.ownerDetail?.dateModified ? new Date(this.ownerDetail?.dateModified): null,
                };
                console.log({ ownerDetail: this.ownerDetail });    
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
        });
    }
    
    changeEdit() {
        if(this.isEdit) {
            let body = {...this.filterField(this.ownerDetail, this.ownerUpdate)};
            this.ownerService.update(body).subscribe((response) => {
                if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                    this.isEdit = !this.isEdit;
                    this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
                }
            });
        } else {
            this.isEdit = !this.isEdit;
            this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
        }
    }
}


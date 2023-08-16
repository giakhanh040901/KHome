import { Component, Injector, Input } from '@angular/core';
import { ContractorConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { NationalityConst } from '@shared/nationality-list';
import { GeneralContractorService } from '@shared/services/general-contractor.service';
@Component({
  selector: 'app-general-contractor-detail',
  templateUrl: './general-contractor-detail.component.html',
  styleUrls: ['./general-contractor-detail.component.scss']
})
export class GeneralContractorDetailComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private routeActive: ActivatedRoute,
        private generalContractorService: GeneralContractorService, 
        private breadcrumbService: BreadcrumbService
    ) { 
        super(injector, messageService);
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Tổng thầu ', routerLink: ['/setting/general-contractor']  },
            { label: 'Chi tiết tổng thầu ',},
        ]);
        this.id = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
    }

    fieldDates = ['licenseDate', 'decisionDate', 'dateModified'];

    id: number;
    contractorDetail: any = {};
    isEdit = false;
    labelButtonEdit = "Chỉnh sửa";

    @Input() contentHeights: number[] = [];

    ContractorConst = ContractorConst;
    NationalityConst = NationalityConst;

    ngOnInit(): void {
        this.isLoading = true;
        this.generalContractorService.getContractor(this.id).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.contractorDetail = res.data;
                this.contractorDetail = {
                    ...this.contractorDetail,
                    licenseDate: this.contractorDetail?.licenseDate ? new Date(this.contractorDetail?.licenseDate): null,
                    decisionDate: this.contractorDetail?.decisionDate ? new Date(this.contractorDetail?.decisionDate): null,
                    dateModified: this.contractorDetail?.dateModified ? new Date(this.contractorDetail?.dateModified): null,
                };
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
        });
    }

    changeEdit() {
        if(this.isEdit) {
            let body = this.formatCalendar(this.fieldDates, {...this.contractorDetail});
            this.generalContractorService.update(body).subscribe((response) => {
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

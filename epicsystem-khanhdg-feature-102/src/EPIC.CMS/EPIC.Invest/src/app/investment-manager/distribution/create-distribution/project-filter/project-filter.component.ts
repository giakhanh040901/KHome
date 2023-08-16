import { Component, Injector, OnInit } from '@angular/core';
import { ProjectConst, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IColumn } from '@shared/interface/p-table.model';
import { Page } from '@shared/model/page';
import { DistributionService } from '@shared/services/distribution.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';


@Component({
  selector: 'app-project-filter',
  templateUrl: './project-filter.component.html',
  styleUrls: ['./project-filter.component.scss']
})
export class ProjectFilterComponent extends CrudComponentBase {
 
    rows:any[] = []
    isLoading:boolean;
    page: Page = new Page();

    constructor(
        injector:Injector,
        messageService:  MessageService,
        private _distributionService: DistributionService,
        public ref: DynamicDialogRef,
    ){
        super(injector, messageService)
    }

    columns: IColumn[] = [];

    ngOnInit(): void {
        this.columns = [
            {   field: 'invName', header: 'Tên dự án', width: 15 },
            {   field: 'guaranteeOrganization', header: 'Tổ chức đảm bảo', width: 25 },
            {   field: 'area', header: 'Diện tích', width: 10 },
            {   field: 'locationDescription', header: 'Vị trí', width: 15 },
            {   field: 'ownerName', header: 'Chủ đầu tư', width: 25 },
            {   field: 'generalContractorName', header: 'Tổng thầu', width: 25 },
            {   field: 'isChoose', header: 'Thao tác', width: 8, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT,
                type: TableConst.columnTypes.ACTION_BUTTON,  
                icon: 'pi pi-check', 
                class: 'justify-content-center', 
            },
        ]
    }

    ProjectConst = ProjectConst;
    ProjectTypes = ProjectConst.projectTypes;

    setPage(event?: Page) {
        this.isLoading = true;
        this._distributionService.getAllProject(this.page).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.setData(res?.data?.items || []);
            }
        }, (err) => {
            this.isLoading = false;
            console.log('err---', err);
        }); 
    }

    setData(rows) {
        this.rows = rows.map(row => {
            row.invNameDisplay = row?.invCode + ' - ' + row?.invName;
            row.ownerName = row?.owner?.businessCustomer?.name;
            row.generalContractorName = row?.generalContractor?.businessCustomer?.name;
            row.isChoose = (row) => this.isChoose(row);
            return row;
        });
    }

    isChoose(project) {
        this.ref.close(project);
    }
}

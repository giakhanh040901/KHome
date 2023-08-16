import { Component, Injector, OnInit } from '@angular/core';
import { SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CompanyShareTypeServiceProxy } from '@shared/service-proxies/setting-service';
import { MessageService } from 'primeng/api';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-company-share-type',
  templateUrl: './company-share-type.component.html',
  styleUrls: ['./company-share-type.component.scss']
})
export class CompanyShareTypeComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    private companyShareTypeService: CompanyShareTypeServiceProxy,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Loại cổ phần' },
    ]);
  }

  modalDialog: boolean;

  deleteItemDialog: boolean = false;

  deleteItemsDialog: boolean = false;

  rows: any[] = [];

  companyShareType: any = {
    "id": 0,
    "code": null,
    "name": null,
    "description":null,
    "icon":null,
    "status":null,
  }

  fieldErrors = {};
  submitted: boolean;
	expandedRows = {};
  cols: any[];
  statuses: any[];

  page = new Page();
  offset = 0;

  ngOnInit(): void {
    this.setPage({ page: this.offset });
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});
  }

  setFieldError() {
		for (const [key, value] of Object.entries(this.companyShareType)) {
			this.fieldErrors[key] = false;
		  }
		console.log({ filedError: this.fieldErrors });
	}
  
  create() {
    this.companyShareType = {};
    this.submitted = false;
    this.modalDialog = true;
  }

  deleteSelectedItems() {
    this.deleteItemsDialog = true;
  }

  edit(companyShareType) {
    this.companyShareType = { ...companyShareType };
    console.log({ productCategory: this.companyShareType })
    this.modalDialog = true;
  }

  delete(companyShareType) {
    this.deleteItemDialog = true;
    this.companyShareType = { ...companyShareType };
  }

  confirmDelete() {
    this.deleteItemDialog = false;
    this.companyShareTypeService.delete(this.companyShareType.id).subscribe(
      (response) => {
        if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
          this.setPage({ page: this.page.pageNumber });
          this.companyShareType = {};
        }
      }, () => {
        this.messageService.add({
          severity: 'error',
          summary: '',
          detail: `Không xóa được tài khoản ${this.companyShareType.name}`,
          life: 3000,
        });
      }
    );
  }

  setPage(pageInfo?: any) {
    this.setFieldError();
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this.companyShareTypeService.getAll(this.page).subscribe((res) => {
      if (this.handleResponseInterceptor(res, '')) {
        this.isLoading = false;
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data.items;
        console.log({ rows: res.data.items, totalItems: res.data.totalItems });
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }

  hideDialog() {
    this.modalDialog = false;
    this.submitted = false;
  }

  resetValid(field) {
		this.fieldErrors[field] = false;
	}

  save() {
    this.submitted = true;
    //
    if (this.companyShareType.id) {
      this.companyShareTypeService.update(this.companyShareType).subscribe(
        (response) => {
          this.callTriggerFiledError(response, this.fieldErrors);
          if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
            this.submitted = false;
            this.setPage({ page: this.page.pageNumber });
            this.hideDialog();
          } else {
						this.callTriggerFiledError(response, this.fieldErrors);
            this.submitted = false;
          }
        }, () => {
          this.submitted = false;
        }
      );
    } else {
      this.companyShareTypeService.create(this.companyShareType).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
            this.submitted = false;
            this.setPage();
            this.hideDialog();
          } else {
            this.callTriggerFiledError(response, this.fieldErrors);
            this.submitted = false;
          }
        }, () => {
          this.submitted = false;
        }
      );
    }
  }
  validForm(): boolean {

    const validRequired = this.companyShareType?.code?.trim() && this.companyShareType?.name?.trim();

    return validRequired;
  }

  header(): string {
    return this.companyShareType?.id > 0 ? 'Sửa Loại cổ phần' : 'Thêm loại cổ phần'
  }

}

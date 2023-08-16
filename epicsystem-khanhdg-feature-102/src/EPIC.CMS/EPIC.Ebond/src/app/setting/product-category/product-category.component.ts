import { Component, Injector, OnInit } from '@angular/core';
import { SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ProductCategoryServiceProxy } from '@shared/service-proxies/setting-service';
import { MessageService } from 'primeng/api';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-product-category',
  templateUrl: './product-category.component.html',
  styleUrls: ['./product-category.component.scss']
})
export class ProductCategoryComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    private productCategoryService: ProductCategoryServiceProxy,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Danh mục trái phiếu' },
    ]);
  }

  modalDialog: boolean;

  deleteItemDialog: boolean = false;

  deleteItemsDialog: boolean = false;

  rows: any[] = [];

  productCategory: any = {
    "productCategoryId": 0,
    "code": null,
    "name": null,
    "description": null,
    "icon": null,
    "content": null,
    "appRouter": null,
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
		for (const [key, value] of Object.entries(this.productCategory)) {
			this.fieldErrors[key] = false;
		  }
		console.log({ filedError: this.fieldErrors });
	}

  create() {
    this.productCategory = {};
    this.submitted = false;
    this.modalDialog = true;
  }

  deleteSelectedItems() {
    this.deleteItemsDialog = true;
  }

  edit(productCategory) {
    this.productCategory = { ...productCategory };
    console.log({ productCategory: this.productCategory })
    this.modalDialog = true;
  }

  delete(productCategory) {
    this.deleteItemDialog = true;
    this.productCategory = { ...productCategory };
  }

  confirmDelete() {
    this.deleteItemDialog = false;
    this.productCategoryService.delete(this.productCategory.productCategoryId).subscribe(
      (response) => {
        if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
          this.setPage({ page: this.page.pageNumber });
          this.productCategory = {};
        }
      }, () => {
        this.messageService.add({
          severity: 'error',
          summary: '',
          detail: `Không xóa được Danh mục trái phiếu ${this.productCategory.name}`,
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

    this.productCategoryService.getAll(this.page).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
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
    if (this.productCategory.productCategoryId) {
      this.productCategoryService.update(this.productCategory).subscribe(
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
        this.productCategoryService.create(this.productCategory).subscribe(
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

    const validRequired = this.productCategory?.code?.trim() && this.productCategory?.name?.trim();

    return validRequired;
  }

  header(): string {
    return this.productCategory?.productCategoryId > 0 ? 'Sửa Danh mục trái phiếu' : 'Thêm Danh mục trái phiếu'
  }
}

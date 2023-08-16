import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from '../layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  providers: [ConfirmationService],
})
export class UserComponent extends CrudComponentBase implements OnInit  {

	constructor(
		injector: Injector,
		private userService: UserServiceProxy, 
		messageService: MessageService,
		private breadcrumbService: BreadcrumbService) 
		{
			super(injector, messageService);
			this.breadcrumbService.setItems([
				{ label: 'Tài khoản' },
			]);
		}

	modalDialog: boolean;

    deleteItemDialog: boolean = false;

    deleteItemsDialog: boolean = false;

    rows: any[] = [];

    user: any = {
      userId: -1,
      displayName: "",
      email: "",
      userName: "",
      password: "",
	  confirmPassword: '',
    }

    submitted: boolean;

    cols: any[];

    statuses: any[];
	listAction:any[] = [];

    page = new Page();
	offset = 0;
	screenHeight: number = window.innerHeight;

    ngOnInit() {
        this.setPage({ page: this.offset});
    }
	
	genListAction(data = []) {
        this.listAction = data.map(businessCustomerItem => {
            const actions = [
                {
                    data: businessCustomerItem,
                    label: 'Sửa',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        this.edit($event.item.data);
                    }
                },
				{
                    data: businessCustomerItem,
                    label: 'Xoá',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
                }
            ];

            return actions;
        });
    }
    create() {
        this.user = {};
        this.submitted = false;
        this.modalDialog = true;
    }

    deleteSelectedItems() {
        this.deleteItemsDialog = true;
    }

    edit(user) {
        this.user = {...user};
        this.modalDialog = true;
    }

    delete(user) {
        this.deleteItemDialog = true;
        this.user = {...user};
    }

    confirmDelete(){
        this.deleteItemDialog = false;
		this.userService.delete(this.user.userId).subscribe(
			(response) => {
				if(this.handleResponseInterceptor(response, 'Xóa thành công')) {
				  this.setPage({ offset: this.page.pageNumber });
				  this.user = {};
				}
			  }, () => {
				this.messageError(`Không xóa được tài khoản ${this.user.displayName}`);
			  }
		);
    }

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		this.isLoading = true;
	
		this.userService.getAll(this.page).subscribe((res) => {
			this.isLoading = false;
			if(this.handleResponseInterceptor(res,'')){
			this.page.totalItems = res.data.totalItems;
			this.rows = res.data.items;
			this.genListAction(this.rows);
			console.log({ rows: res.data.items, totalItems: res.data.totalItems });
		  }
		}, () => {
		  this.isLoading = false;
		});
		// fix show dropdown options bị ẩn dướ
	  }

    hideDialog() {
        this.modalDialog = false;
        this.submitted = false;
    }

    save() {
      this.submitted = true;
      //
        if (this.user.userId >=0) {
            this.userService.update(this.user).subscribe(
				(response) => {
					if(this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
						this.submitted = false;
						this.setPage({ page: this.page.pageNumber });
						this.hideDialog();
					} else {
						this.submitted = false;
					}
				},() => {
					this.submitted = false;
				}
			);
        } else {
			this.user.userType = 'I';
			this.userService.create(this.user).subscribe(
				(response) => {
					if(this.handleResponseInterceptor(response, 'Thêm thành công')) {
						this.submitted = false;
						this.setPage();
						this.hideDialog();
					} else {
						this.submitted = false;
					}
				},() => {
					this.submitted = false;
				}
			);
        }
    }

	validatePassword(): boolean {
		return this.user?.password?.trim() && this.user?.confirmPassword?.trim() && this.user?.confirmPassword?.trim() != this.user?.password?.trim();
	}

	validForm(): boolean {
		
		const validIfCreate = this.user.confirmPassword === this.user.password && this.user?.userName?.trim() && this.user?.displayName?.trim() && this.user?.password?.trim() && this.user?.email?.trim();
		const validIfUpdate = this.user?.userName?.trim() && this.user?.displayName?.trim() && this.user?.email?.trim();

		return this.user.userId >= 0 ? validIfUpdate : validIfCreate;
	}	
}

import { Component, Injector, OnInit } from '@angular/core';
import { SearchConst, NotifyManagerConst, InvestorConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { NotificationExtendService } from '@shared/services/notification-extend.service';
import { NotificationService } from '@shared/services/notification.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';

@Component({
    selector: 'app-add-person-list',
    templateUrl: './add-person-list.component.html',
    styleUrls: ['./add-person-list.component.scss']
})
export class AddPersonListComponent extends CrudComponentBase implements OnInit {

    selectedCustomers: any[] = []
    isLoadingPersonList: boolean = true;
    rows: any[] = [];
    personListPage = new Page()
    inputData: any;
    statusCheckedAll: boolean = false;
    uniqCustomers: any[] = [];

    exitedPersonCodeList: any[] = [];
    exitedUserErrorList: any[] = [];

    filters: {
        pushAppStatus: any,
        sendEmailStatus: any,
        sendSMSStatus: any,
        sendNotifySize: any,
    }
    optionAges = Array.from(Array(100).keys()).filter(age => age >= 16);

    dataFilter = {
        // field: null,
        customerType: NotifyManagerConst.customerTypes[0].code,
        sex: null,
        startAge: null,
        endAge: null,
    }

    // CONSTS
    NotifyManagerConst = NotifyManagerConst;
    InvestorConst = InvestorConst;

    constructor(
        protected messageService: MessageService,
        public config: DynamicDialogConfig,
        public ref: DynamicDialogRef,
        private notificationService: NotificationService,
        private _notificationExtendService: NotificationExtendService,
        injector: Injector) {
        super(injector, messageService);
        this.filters = {
            pushAppStatus: null,
            sendEmailStatus: null,
            sendSMSStatus: null,
            sendNotifySize: NotifyManagerConst.TYPE_SELECTED_PAGE,
        }
    }


    ngOnInit(): void {
        this.setPersonList();
        this.inputData = this.config.data.inputData;

        // this.setPage({ page: this.offset });
        this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
            if (this.keyword === "") {
                this.setPersonList({ page: this.offset });
            } else {
                this.setPersonList();
            }
        })

        let filters = {
            'sendSMSStatus': this.filters.sendSMSStatus?.map(status => { return status.value }),
            'pushAppStatus': this.filters.pushAppStatus?.map(status => { return status.value }),
            'sendEmailStatus': this.filters.sendEmailStatus?.map(status => { return status.value }),
        }
        this.notificationService.getPersonList(this.inputData.id, this.personListPage, filters, true).subscribe((res) => {
            for(let person of res.results) {
                if(person?.personCode) this.exitedPersonCodeList.push(Number(person.personCode));
                if(!person?.personCode) this.exitedUserErrorList.push(person.phoneNumber);
            }
        });
    }

    findCustomer() {
    }

    changeFilter(fieldFilter = false) {
        if (fieldFilter && this.keyword || !fieldFilter) this.setPersonList();
    }

    

    setPersonList(pageInfo?: any) {
        this.personListPage.pageNumber = pageInfo?.page ?? this.offset;
        this.personListPage.keyword = this.keyword;
        this.isLoadingPersonList = true;

        this._notificationExtendService.getInvestorListNotify(this.personListPage, this.dataFilter).subscribe((res) => {
            if (this.handleResponseInterceptor(res, "")) {
                this.personListPage.totalItems = res.data.totalItems;
                console.log(' this.personListPage.totalItems: ', this.personListPage.totalItems);

                // this.rows = res.data.items;
                if (Array.isArray(res.data?.items)) {
                    let i = 0;
                    this.rows = res.data?.items.map(item => {
                        return {
                            ...item,
                            id: i++
                        }
                    });
                    console.log({ 'rows': this.rows });
                }
            }
            this.isLoadingPersonList = false;
        },
            () => {
                this.isLoadingPersonList = false;
            }
        );
    }

    addPeopleToSendingList() {
        let customers = this.selectedCustomers.map((customer, index) => {
            return {
                fullName: customer?.name,
                personCode: customer?.userId,
                phoneNumber: customer?.phone,
                email: customer?.email,
                notification: this.inputData.id,
                fcmTokens: customer?.fcmTokens ? [...customer?.fcmTokens] : [],  
            }
        })
        // lọc danh sách ko trùng personCode
        this.uniqCustomers = customers.filter((item, index, arg) => {
            console.log(index, item.personCode, item.phoneNumber, !this.exitedPersonCodeList.includes(item.personCode), (!this.exitedUserErrorList.includes(item.phoneNumber) && !item.personCode),arg);
            return !this.exitedPersonCodeList.includes(item.personCode) || (!this.exitedUserErrorList.includes(item.phoneNumber) && !item.personCode)
        });        

        this.isLoadingPersonList = true;
        this.notificationService.addPeopleToNotification({ sendingList: this.uniqCustomers }, this.inputData.id).subscribe(results => {
            this.isLoadingPersonList = false;
            this.messageService.add({ key: 'tst', severity: 'success', summary: 'Thành công', detail: 'Cập nhật danh sách người nhận thành công' });
            this.ref.close(null);
        }, error => {
            this.isLoadingPersonList = false;
            this.messageService.add({ key: 'tst', severity: 'error', summary: 'Lỗi', detail: 'Có lỗi xảy ra. Vui lòng thử lại!' });
        })

    }

    onRowSelect(event) {
        console.log('eventSelect', event);
    }

    onRowUnSelect($event) {
        console.log('eventSelect', this.selectedCustomers);
    }

    selectedAll(statusCheckedAll) {
        if (statusCheckedAll) {
            this.isLoadingPersonList = true;
            this.personListPage.keyword = this.keyword;
            this._notificationExtendService.getInvestorListNotify(this.personListPage, this.dataFilter, true).subscribe((res) => {
                if (this.handleResponseInterceptor(res, "")) {
                    if (Array.isArray(res.data?.items)) {
                        let i = 0;
                        this.selectedCustomers = res.data?.items.map(item => {
                            return { ...item, id: i++ }
                        });
                    }
                }
                this.isLoadingPersonList = false;
            }
            );
        } else {
            this.selectedCustomers = [];
        }
        //
        this.statusCheckedAll = statusCheckedAll;
    }
}

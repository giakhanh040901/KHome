import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { MessageBankResponse, OrderPaymentConst, StatusPaymentBankConst, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { WithdrawalService } from '@shared/services/withdrawal-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { DetailRequestComponent } from '../detail-request/detail-request.component';
import { OTPWithdrawComponent } from '../otpwithdraw/otpwithdraw.component';
import { IColumn } from '@shared/interface/p-table.model';

@Component({
  selector: 'app-list-request-bank',
  templateUrl: './list-request-bank.component.html',
  styleUrls: ['./list-request-bank.component.scss'],
  providers: [ConfirmationService],
})
export class ListRequestBankComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        public configDialog: DynamicDialogConfig,
        private dialogService: DialogService,
        public ref: DynamicDialogRef,
        private _withdrawalService: WithdrawalService, 
    ) { 
        super(injector, messageService);
    }

    //CONST
    StatusPaymentBankConst= StatusPaymentBankConst;

    requests: any = {};
    distributionId: number;

    columns: IColumn[] = [];
    rows = [];
    selectedRequests = [];
    bank: any;

    body = {
        tradingBankAccId: 0,
        withdrawalIds: [],
        interestPaymentIds: [],
        status: StatusPaymentBankConst.APPROVE_ONLINE,
    }

    banks = [];
    interestType: string;
    typeContract: string;

    ngOnInit(): void {
        this.interestType = this.configDialog?.data?.interestType;
        this.typeContract = this.configDialog?.data?.typeContract;
        this.distributionId = this.configDialog?.data?.distributionId;
        //
        this.getBanks(this.distributionId);
        //
        this.columns = [
            { field: '', header: '', width: 3, type: TableConst.columnTypes.CHECKBOX_ACTION },
            { field: 'id', header: '#ID', width: 5 },
            { field: 'name', header: 'Khách hàng', width: 16, },
            { field: 'totalExpire', header: 'Số tiền', width: 10, type: TableConst.columnTypes.CURRENCY },
            { field: 'validate', header: 'Kiểm tra', width: 15, isResize: true },
        ];
        this.selectedRequests = this.configDialog?.data?.requests;
        this.setData();
        this.rows = [...this.selectedRequests];
        //
    }

    setData() {
        for (let select of this.selectedRequests) {
            select.totalExpire = (this.typeContract == 'DaoHanHopDong') ? select?.totalExpire : select?.amountMoney;
        };
    }

    getBanks(distributionId) {
        this.isLoading = true;
        this._withdrawalService.getDistributionBank(distributionId, OrderPaymentConst.CHI).subscribe((res) => {
            this.isLoading = false;
            if(this.handleResponseInterceptor(res) && res?.data) {
                this.banks = res.data.map(bank => {
                    bank.labelName = bank?.bankAccNo + ' - ' + bank?.bankName + ' - ' + bank?.bankAccName;
                    return bank;
                });
            }
        },(err) => {
            console.log('err', err);
            this.messageError('Không lấy được danh sách ngân hàng chi!');
        });
    }

    cancel() {
        this.ref.close();
    }

    save() {
        let body = {...this.body}; 
        //
        body.interestPaymentIds = []; 
        body.interestPaymentIds = this.selectedRequests.map(s => s.id);
        //
        if(body.interestPaymentIds?.length) {
            // DUYỆT CHI TỰ ĐỘNG GỬI REQUEST SANG NGÂN HÀNG
            if(this.interestType == StatusPaymentBankConst.MANAGER_WITHDRAW) {
                body.withdrawalIds = [...body.interestPaymentIds];
                delete body.interestPaymentIds;
            }

            this.submitted = true;
            this._withdrawalService.requestBank(body, this.interestType).subscribe((resRequest) => {
                this.submitted = false;
                if(this.handleResponseInterceptor(resRequest)) {
                    if(resRequest?.data?.isSuccess) {
                        this.setMessageResponse(resRequest);
                        // POPUP NHẬP OTP ĐỂ XÁC THỰC CHI TIỀN
                        const ref = this.dialogService.open(
                        OTPWithdrawComponent,
                        {
                            header: "Nhập OTP",
                            width: '400px',
                            data: { 
                                requestId: resRequest?.data?.id,
                                interestType : this.interestType,
                                bodySubmit:  {
                                ...body,
                                    prepare: resRequest.data,
                                    status: StatusPaymentBankConst.APPROVE_ONLINE,
                                }
                            }
                        });
                        //
                        ref.onClose.subscribe((statusResponse) => {
                            if(statusResponse) {
                                this.ref.close(true);
                            }
                        });
                        //
                    } else {
                        this.setMessageResponse(resRequest);
                        this.messageError('Vui lòng kiểm tra lại thông tin yêu cầu (FE)!');
                    }
                } 
            },(err) => {
                this.submitted = false;
                this.messageError('Ngân hàng đang bảo trì vui lòng thử lại sau (FE)!');
                console.log('err', err);
            });
            //
        } else {
            this.messageError('Vui lòng chọn tối thiểu 1 yêu cầu chi tiền (FE)!')
        }
    }

    setMessageResponse(resRequest) {
        this.rows = this.rows.map(row => {
            let detail = resRequest.data.details.find(detail => detail.referId == row.id);
            if(detail) {
                row.bankInfo = detail;
                if(detail.errorMessage) {
                    row.isCheck = false;
                    row.validate = MessageBankResponse.getMessageMsb(detail.errorMessage);
                } else {
                    row.isCheck = true;
                    row.validate = '';
                }
            }
            return row;
        });
    }
    //
    approve(body) {
        this.isLoading = true;
        this._withdrawalService.approve(body,this.interestType).subscribe((res) => {
            this.isLoading = false;
            if(this.handleResponseInterceptor(res)) {
                this.ref.close(true);
            }
        }, (err) => {
            console.log('err__', err);
            this.isLoading = false;
        });
    }

    showInfo(row) {
        const ref = this.dialogService.open(
        DetailRequestComponent,
        {
            header: "Thông tin ngân hàng khách hàng",
            width: '600px',
            data: {
                info: {...row},
            }
        });
    }
}

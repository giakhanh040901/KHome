import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { MessageBankResponse, OrderPaymentConst, StatusPaymentBankConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { WithdrawalService } from '@shared/services/withdrawal-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { DetailRequestComponent } from '../detail-request/detail-request.component';
import { OTPWithdrawComponent } from '../otpwithdraw/otpwithdraw.component';

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
    private confirmationService: ConfirmationService,
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
  interestType: string;
  typeContract: string;
  cols = [];
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

  ngOnInit(): void {
    this.interestType = this.configDialog?.data?.interestType;
    this.typeContract = this.configDialog?.data?.typeContract;
    this.rows = this.selectedRequests = this.configDialog.data.requests;
    console.log("rows____",this.rows);

    this.cols = [
			{ field: 'nameDisplay', header: 'Khách hàng', width: '16rem', isPin: true },
			{ field: 'amountMoneyDisplay', header: 'Số tiền', width: '10rem', cutText: 'b-cut-text-12', isPin: true },
			// { field: 'policyName', header: 'Chính sách', width: '12rem' },
			{ field: 'validate', header: 'Kiểm tra', width: '15rem', isResize: true },
		];
    
    this.setData(this.rows);
    this.getBanks(this.selectedRequests[0]?.distributionId | this.selectedRequests[0]?.policy?.distributionId);
  }

  getBanks(distributionId) {
    this.isLoading = true;
    this._withdrawalService.getDistributionBank(distributionId, OrderPaymentConst.CHI)
			.subscribe((res) => {
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
  
  setData(rows) {
		for (let row of rows) {
			row.nameDisplay = row?.investor?.investorIdentification?.fullname || row?.name;
		};
	}

  cancel() {
    this.ref.close();
  }

  save() {
    this.body.withdrawalIds = [];
    //
		for(let row of this.selectedRequests) {
			this.body.withdrawalIds.push(row.id);
		}
    //
    if(this.body.withdrawalIds?.length) {
      if(this.interestType == StatusPaymentBankConst.INTEREST_CONTRACT) {
        this.body.interestPaymentIds =[...this.body.withdrawalIds];
        delete this.body.withdrawalIds;
      }
      // DUYỆT CHI TỰ ĐỘNG GỬI REQUEST SANG NGÂN HÀNG
      this.isLoading = true;
      this._withdrawalService.requestBank(this.body, this.interestType).subscribe((resRequest) => {
        this.isLoading = false;
        if(this.handleResponseInterceptor(resRequest)) {
          if(resRequest?.data?.isSuccess) {
            this.setMessageResponse(resRequest);
            // POPUP NHẬP OTP ĐỂ XÁC THỰC CHI TIỀN
            if (resRequest?.data?.isSubmitOtp){
              const ref = this.dialogService.open(
                OTPWithdrawComponent,
                {
                  header: "Nhập OTP",
                  width: '400px',
                  contentStyle: { "overflow": "auto", "padding": 0, "padding-bottom": "50px" },
                  style: { "overflow": "auto" },
                  data: { 
                    requestId: resRequest?.data?.id,
                    interestType : this.interestType,
                    bodySendOtp:  {
                      ...this.body,
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
            } else {
                this.messageSuccess("Xử lý rút tiền thành công!", 1000)
                this.ref.close(true);
            }
          } else {
            this.setMessageResponse(resRequest);
            this.messageError('Vui lòng kiểm tra lại thông tin yêu cầu! (FE)');
          }
        }
      },(err) => {
        this.isLoading = false;
        this.messageError('Ngân hàng đang bảo trì vui lòng thử lại sau! (FE)');
        console.log('err', err);
      });
    //
    } else {
      this.messageError('Vui lòng chọn tối thiểu 1 yêu cầu chi tiền! (FE)')
    }
	}

  setMessageResponse(resRequest) {
    this.rows = this.rows.map(row => {
      let detail = resRequest.data.details.find(detail => detail.referId == row.id);
      if(detail) {
        row.bankInfo = detail;
        if(detail.errorMessage) {
          row.isCheck = false;
          row.validate = MessageBankResponse.getMessageMsb(detail?.errorMessage);
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
        contentStyle: { "overflow": "auto", "padding": 0, "padding-bottom": "50px" },
        style: { "overflow": "auto" },
        data: {
          info: {...row},
        }
      }
    );
  }
}

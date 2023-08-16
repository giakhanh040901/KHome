import { Component, Injector } from '@angular/core';
import { ProductPolicyConst, OrderConst, ActiveDeactiveConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { DistributionService } from '@shared/services/distribution.service';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig} from 'primeng/dynamicdialog';
import { OrderService } from '@shared/services/order.service';

@Component({
  selector: 'app-reinstatement-request',
  templateUrl: './reinstatement-request.component.html',
  styleUrls: ['./reinstatement-request.component.scss']
})
export class ReinstatementRequestComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private _orderService: OrderService,
        private _distributionService: DistributionService,
        private dialogConfig: DynamicDialogConfig,
        private ref : DynamicDialogRef,
    ) {
        super(injector, messageService);
    }


    bondSecondary: any = {};
    policies: any[] = [];
    policyInfo: any = {};
    policyDetails: any[] = [];
    policyDetail: any = {};
    profit:number = null;

    ProductPolicyConst = ProductPolicyConst;
    OrderConst = OrderConst;

    orderId: number;
    orderDetail: any = {};

    body = {
        settlementMethod: 0,
        renewarsPolicyDetailId: 0,
        orderId: 0,
        requestNote: '',
        summary: '',
    }

    ngOnInit() {
        this.orderId = this.dialogConfig.data?.orderId;
        this.body.orderId = this.orderId;
        this.init();
    }

    init() {
        this.isLoading = true;
        this._orderService.get(this.orderId).subscribe((resOrder) => {
            if (this.handleResponseInterceptor(resOrder, '')) {
                this.orderDetail = { ...resOrder.data };
                this.body.renewarsPolicyDetailId = this.orderDetail.policyDetailId;
                this.initDistributionInfo(this.orderDetail?.distributionId);
            } else {
                this.isLoading = false;
            }
        });
    }

    initDistributionInfo(distributionId) {
        this._distributionService.getById(distributionId).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                if (res?.data?.policies && res?.data?.policies?.length) {
                this.policies = res.data.policies.reduce((result: any[], value: any) => {
                    if (value.status == ActiveDeactiveConst.ACTIVE) {
                        result.push({
                            ...value,
                            label: value.code + (value.name && value.name.length ? ` - ${value.name}` : ''),
                        })
                    }
                    return result;
                }, []);
                }
                this.policyInfo = this.policies.find(item => item.id == this.orderDetail.policyId);
                //
                this.policyDetails = this.policyInfo?.policyDetail.filter(pD => pD.status == ActiveDeactiveConst.ACTIVE)
                this.policyDetail = this.policyDetails.find(item => item.id == this.orderDetail.policyDetailId);
                this.profit = this.policyDetail.profit;
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
        });
    }

    changePolicy(policy) {
        this.orderDetail.policyId = policy.id;
        this.policyInfo = this.policies.find(item => item.id == policy.id);
        this.policyDetails = policy.policyDetail;
        this.profit = null;
    }

    changePolicyDetail(policyDetailId) {
        this.orderDetail.policyDetailId = policyDetailId;
        this.policyDetail = this.policyDetails.find(item => item.id == policyDetailId);
        this.profit = this.policyDetail.profit;
    }
    
    cancel() {
        this.ref.close();
    }

    save() {
        if(this.validForm()) {
        this.body.summary = this.orderDetail.contractCode 
                            + ' - ' + (this.orderDetail.investor?.investorIdentification?.fullname || this.orderDetail?.businessCustomer?.name)
                            + '- ' + (this.orderDetail?.investor?.phone || this.orderDetail?.businessCustomer?.name);
        //
        this._orderService.reinstatementRequest(this.body).subscribe(res => {
            if(this.handleResponseInterceptor(res, '')) {
                this.ref.close(res);
            }
        }, (err) => {
            this.messageError('Yêu cầu thất bại vui lòng thử lại sau!', '');
        });
        } else {
            this.messageError('Vui lòng nhập dữ liệu cho các trường có dấu (*)');
        }
    }

    validForm() {
        return  this.body.orderId && this.body.renewarsPolicyDetailId;
    }

}

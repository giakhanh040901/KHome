import { ActiveDeactiveConst } from './../../../../../shared/AppConsts';
import { Component, Injector, Input, OnInit } from "@angular/core";
import { AppConsts, FormNotificationConst, PolicyDetailTemplateConst, PolicyTempConst, ProductPolicyConst, SearchConst, YesNoConst, } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { PageBondPolicyTemplate } from "@shared/model/pageBondPolicyTemplate";
import { DistributionService } from "@shared/services/distribution.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { FormSetDisplayColumnComponent } from "src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { CreateDistributionPolicyComponent } from "./create-distribution-policy/create-distribution-policy.component";
import { Page } from '@shared/model/page';

@Component({
  selector: "app-distribution-policy",
  templateUrl: "./distribution-policy.component.html",
  styleUrls: ["./distribution-policy.component.scss"],
})
export class DistributionPolicyComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private _dialogService: DialogService,
    private _distributionService: DistributionService,
    public confirmationService: ConfirmationService,

  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: "Trang chủ", routerLink: ["/home"] },
      { label: "Chính sách mẫu" },
    ]);
  }

  @Input() distributionId: number;
  rows: any[] = [];

  ActiveDeactiveConst = ActiveDeactiveConst;
  PolicyTempConst = PolicyTempConst;
  PolicyDetailTemplateConst = PolicyDetailTemplateConst;
  YesNoConst = YesNoConst;

  row: any;
  col: any;

  policyTemp: any[] = [];

  _selectedColumns: any[];

  cols: any[];

  listAction: any[] = [];
  //
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

    // Xử lý ẩn hiện cột trong bảng
    this.cols = [
      { field: 'code', header: 'Mã chính sách', width: '12rem', isPin: true },
      { field: 'name', header: 'Tên chính sách', width: '16rem', isPin: true },
      { field: 'interestTypeName', header: 'Loại chính sách', width: '10rem' },
      { field: 'incomeTaxDisplay', header: 'Thuế LN ', width: '7rem', pTooltip: 'Thuế lợi nhuận', tooltipPosition: 'top', class: 'justify-content-end', iClass: 'pi pi-question-circle' },
      { field: 'transferAssetsFeeDisplay', header: 'Thuế CN ', width: '7rem', pTooltip: 'Thuế chuyển nhượng', tooltipPosition: 'top', class: 'justify-content-end', iClass: 'pi pi-question-circle' },
      { field: 'nameClassify', header: 'Phân loại', width: '7rem' },
    ];

    this.cols = this.cols.map((item, index) => {
      item.position = index + 1;
      return item;
    });

    this._selectedColumns = this.getLocalStorage("distributionPolicy") ?? this.cols;
  }

  setColumn(col, _selectedColumns) {
    const ref = this.dialogService.open(
      FormSetDisplayColumnComponent,
      this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._selectedColumns = dataCallBack.data.sort(function (a, b) {
          return a.position - b.position;
        });
        this.setLocalStorage(this._selectedColumns, 'distributionGan');
      }
    });
  }

  showData(rows) {
    for (let row of rows) {
      row.code = row.code,
      row.name = row.name,
      row.interestTypeName = this.PolicyTempConst.getInterestType(row?.interestType),
      row.incomeTaxDisplay = row?.incomeTax ? this.utils.transformPercent(row?.incomeTax) + "%": "",
      row.transferAssetsFeeDisplay = row?.transferAssetsFee ? this.utils.transformPercent(row?.transferAssetsFee) + "%": "",
      row.nameClassify = this.PolicyTempConst.getNameClassify(row.classify),
      row.sortOrderName = this.PolicyTempConst.getsortOrderName(row.sortOrder);
  }
    console.log("row", rows);
  }

  genListAction(data = []) {
    this.listAction = data.map((policy, index) => {
      const actions = [];

      if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_KyHan_ThemMoi])) {
        actions.push({
          data: policy,
          label: "Thêm kỳ hạn",
          icon: "pi pi-plus",
          command: ($event) => {
            this.edit($event.item.data, true);
          },
        });
      }

      if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_ChinhSach_BatTatShowApp])) {
				actions.push({
					data: policy,
					label: policy.isShowApp == false ? 'Bật show app' : 'Tắt show app',
					icon: policy.isShowApp == false ? 'pi pi-eye' : 'pi pi-eye-slash',
					command: ($event) => {
						this.toggleIsShowAppPolicy($event.item.data?.id);
					}
				});
			}
      //
      if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_ChinhSach_KichHoatOrHuy])) {
				actions.push({
					data: policy,
					label: policy.status == ActiveDeactiveConst.ACTIVE ? 'Khóa' : 'Kích hoạt',
					icon: policy.status == ActiveDeactiveConst.ACTIVE ? 'pi pi-lock' : 'pi pi-lock-open',
					command: ($event) => {
						this.changeStatusPolicy($event.item.data);
					}
				});
			}
      //
      if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_ChinhSach_CapNhat])) {
        actions.push({
          data: policy,
          index: index,
          label: "Sửa",
          icon: "pi pi-pencil",
          command: ($event) => {
            this.edit($event.item.data);
          },
        });
      }
      //
      if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_ChinhSach_Xoa])) {
        actions.push({
          data: policy,
          label: "Xoá",
          icon: "pi pi-trash",
          command: ($event) => {
            this.delete($event.item.data);
          },
        });
      }
      //
      return actions;
    });
    console.log("listActions", this.listAction);
  }

  create() {
    const ref = this.dialogService.open(CreateDistributionPolicyComponent, {
      header: "Thêm chính sách",
      width: '1000px',
      contentStyle: { "max-height": "600px", overflow: "auto", "margin-bottom": "60px", },
      baseZIndex: 10000,
      data: {
        distributionId: this.distributionId
      },
    });
    //
    ref.onClose.subscribe((res) => {
        this.setPage();
    });
  }

  edit(policy, isCreateDetail?: boolean) {
    console.log("policy: ", policy);

    const ref = this.dialogService.open(CreateDistributionPolicyComponent, {
      header: "Cập nhật chính sách",
      width: "1000px",
      contentStyle: { "max-height": "600px", overflow: "auto", "margin-bottom": "60px", },
      baseZIndex: 10000,
      data: {
        distributionId: this.distributionId,
        policyId: policy.id,
        isCreateDetail: isCreateDetail,
        startDate: policy.policy,
        endDate: policy.endDate,
      },
    });
    //
    ref.onClose.subscribe((statusUpdate) => {
      if (statusUpdate) {
        this.messageSuccess('Cập nhật thành công');
        this.setPage();
      }
    });
  }

  delete(policy) {
    const ref = this._dialogService.open(FormNotificationComponent, {
      header: "Thông báo",
      width: "600px",
      contentStyle: { "max-height": "600px", overflow: "auto", "padding-bottom": "50px", },
      styleClass: "p-dialog-custom",
      baseZIndex: 10000,
      data: {
        title: "Bạn có chắc chắn muốn xóa chính sách này?",
        icon: FormNotificationConst.IMAGE_CLOSE,
      },
    });
    ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
      if (dataCallBack?.accept) {
        this._distributionService.deletePolicy(policy.id).subscribe((response) => {
            if ( this.handleResponseInterceptor(response, "Xóa chính sách thành công")) {
              this.setPage();
            }
          }, (err) => {
            console.log('err____', err);
            this.messageError(`Không xóa được chính sách ${policy.name}`);
          });
      } else {
        this.messageService.add({
          severity: "error",
          detail: AppConsts.messageError,
          life: 2000,
        });
      }
    });
  }

  changeStatusPolicy(policy) {
    this._distributionService.changeStatusPolicy(policy.id).subscribe(
      (response) => {
        let message = "Kích hoạt thành công";
        if (policy.status == "A") message = "Hủy kích hoạt thành công";
        if (this.handleResponseInterceptor(response, message)) {
          this.setPage({ page: this.page.pageNumber });
        }
      },(err) => {
        console.log("err----", err);
        this.messageError(AppConsts.messageError);
      }
    );
  }

    // BAT TAT IS SHOW APP
	toggleIsShowAppPolicy(policyId) {
		this._distributionService.toggleIsShowAppPolicy(policyId).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, "Cập nhật show app thành công")) {
					this.submitted = false;
					this.setPage({ page: this.page.pageNumber });
				}
			}, (err) => {
				console.log('err---', err);
			}
		);
	}

  formatCurrency(value) {
    return value.toLocaleString('de-DE', { style: 'currency', currency: 'USD' });
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    this._distributionService.getAllPolicy(this.distributionId).subscribe((res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, "")) {
          this.rows = res?.data.map(row => {
            row.isShowApp = (row.isShowApp == this.YesNoConst.YES);
            return row;
          });
          if (this.rows?.length) {
            this.genListAction(this.rows);
            this.showData(this.rows);
          }
        }
      }, (err) => {
        this.isLoading = false;
        console.log('Error-------', err);
      });
    }
}

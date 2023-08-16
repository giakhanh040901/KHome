import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { forkJoin } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { PageBondPolicyTemplate } from '@shared/model/pageBondPolicyTemplate';
import { ProductBondPolicyTemplateServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { FormNotificationConst, ProductBondPolicyDetailTemplateConst, ProductBondPolicyTemplateConst, SearchConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';

@Component({
  selector: 'app-product-bond-policy-template',
  templateUrl: './product-bond-policy-template.component.html',
  styleUrls: ['./product-bond-policy-template.component.scss'],
})
export class ProductBondPolicyTemplateComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    private _productBondPolicyTemplateService: ProductBondPolicyTemplateServiceProxy,
    messageService: MessageService,
    private dialogService: DialogService,
    private _dialogService: DialogService,
    private breadcrumbService: BreadcrumbService) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Chính sách mẫu' },
    ]);
  }
  fieldErrors = {};
  //ref: DynamicDialogRef;

  modalDialog: boolean;
  modalDialogPolicyDetailTemplate: boolean;
  modalUpdateDialogPolicyTemplate: boolean;
  modalDialogTermSalesApply: boolean;

  deleteItemDialog: boolean = false;
  deleteBondPolicyTemplateDialog: boolean = false;
  deleteBondPolicyDetailTemplateDialog: boolean = false;

  //deleteItemsDialog: boolean = false;

  rows: any[] = [];
  rowsListPolicy: any;

  // Data Init
  productBondPrimary: any = [];
  ProductBondPolicyTemplateConst = ProductBondPolicyTemplateConst;
  ProductBondPolicyDetailTemplateConst = ProductBondPolicyDetailTemplateConst;
  ProjectTypes = [
    {
        name: 'Nhà riêng',
        code: '1',
    },
    {
        name: 'Căn hộ chung cư',
        code: '2',
    },
    {
        name: 'Biệt thự',
        code: '3',
    },
    {
        name: 'Đất nền',
        code: '4',
    },
    {
        name: 'Biệt thự nghỉ dưỡng',
        code: '5',
    },
    {
        name: 'Condotel',
        code: '6',
    },
    {
        name: 'Shophouse',
        code: '7',
    },
    {
        name: 'Officetel',
        code: '8',
    },
];

  ///////
  expandedRows = {};

  isExpanded: boolean = false;

  row: any;
  col: any;


  _selectedColumns: any[];


  policyTemplate: any = {
    'bondPolicyTempId': 0,
    'code': null,   // Mã chính sách
    'name': null,   // Tên chính sách
    'type': null,   // Kiểu chính sách
    'investorType': null,   // Loại nhà đầu tư
    'minMoney': null,  // Số tiền đầu tư tối thổi
    'transferTax': null, // Thuế chuyển nhượng %
    'incomeTax': null,  // Thuế lợi nhuận
    'classify': null, // phân loại
    'isTransfer': null, // Có cho phép chuyển nhượng
    'productBondPolicyDetailTemp': [],  // Kỳ hạn
  }

  policyDetailTemplate: any = {
    'bondPolicyDetailTempId': 0,
    'bondPolicyTempId': 0,
    'fakeId': 0,
    'stt': null, //Số thứ tự
    'shortName': null, //Tên viết tắt
    'name': null,  //Tên kỳ hạn (Tên sản phẩm)
    'interestPeriodQuantity': null,   // Số kỳ trả lợi tức
    'interestPeriodType': null,   // Đơn vị (D:ngày, M: tháng, Y: năm)
    'periodQuantity': null,   // Số kỳ đầu tư
    'periodType': null,   // Đơn vị (D:ngày, M: tháng, Y: năm)
    'profit': null,   // Lợi nhuận
    'interestDays': null,  // Số ngày
    'interestType': null, // Kiểu trả lãi
    'status': null,
    'unSave': null

  }
  statusSearch = ProductBondPolicyTemplateConst.status;
  policyDetailTemplateId: number;

  blockText: RegExp = /[0-9,.]/;
  submitted: boolean;
  cols: any[];
  statuses: any[];
  listAction: any[] = [];
  listActionTerm: any[] = [];
  listActionTermA: any[] = [];
  actions: any[] = [];
  fieldDates = ['holdDate'];
  page = new PageBondPolicyTemplate();
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

    this.cols = [
      { field: 'code', header: 'Mã chính sách', width: '10rem', position: 1, cutText: 'b-cut-text-10', isPin: true },
      { field: 'name', header: 'Tên chính sách', width: '15rem', position: 2, cutText: 'b-cut-text-15', isPin: true },
      { field: 'nameType', header: 'Kiểu', width: '12rem', position: 3, cutText: 'b-cut-text-12' },
      { field: 'nameInvestorType', header: 'Loại nhà đầu tư', width: '12rem', position: 4, cutText: 'b-cut-text-12' },
      { field: 'incomeTaxDisplay', header: 'Thuế LN ', width: '8rem', pTooltip: 'Thuế lợi nhuận', tooltipPosition: 'top', class: 'text-right justify-content-end', iClass: 'pi pi-question-circle', position: 5, cutText: 'b-cut-text-8' },
      { field: 'transferTaxDisplay', header: 'Thuế CN ', width: '8rem', pTooltip: 'Thuế chuyển nhượng', tooltipPosition: 'top', class: 'text-right justify-content-end', iClass: 'pi pi-question-circle', position: 6, cutText: 'b-cut-text-8' },
      { field: 'nameClassify', header: 'Phân loại', width: '12rem', position: 7, cutText: 'b-cut-text-12' },
    ];
    // this._selectedColumns = this.cols;
    this._selectedColumns = this.getLocalStorage('bondPolicy') ?? this.cols;
  }

  getLocalStorage(key) {
    return JSON.parse(localStorage.getItem(key))
  }
  setLocalStorage(data) {
    return localStorage.setItem('bondPolicy', JSON.stringify(data));
  }
  changeStatus() {
    this.setPage({ page: this.page.pageNumber });

  }
  setColumn(col, _selectedColumns) {
    console.log('cols:', col);

    console.log('_selectedColumns', _selectedColumns);

    const ref = this.dialogService.open(
      FormSetDisplayColumnComponent,
      this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      console.log('dataCallBack', dataCallBack);
      if (dataCallBack?.accept) {
        this._selectedColumns = dataCallBack.data.sort(function (a, b) {
          return a.position - b.position;
        });
        this.setLocalStorage(this._selectedColumns)
        console.log('Luu o local', this.getLocalStorage('bondPolicy'));
      }
    });
  }

  showData(rows) {
    for (let row of rows) {
      row.name = row?.name;
      row.nameType = this.ProductBondPolicyTemplateConst.getNameType(row?.type),
      row.nameInvestorType = this.ProductBondPolicyTemplateConst.getNameInvestorType(row?.investorType),
      row.incomeTaxDisplay = this.utils.transformPercent(row?.incomeTax) + '%';
      row.transferTaxDisplay = this.utils.transformPercent(row?.transferTax) + '%';
      row.nameClassify = this.ProductBondPolicyTemplateConst.getNameClassify(row?.classify)
    }
    //
    console.log('row', rows);
    this.actions = [
      {
        label: "Sửa",
        icon: "pi pi-pencil",
        permission: this.isGranted(),
        command: () => {
          this.editPolicyDetailUnSave(this.policyDetailTemplate);
        },
      },
      {
        label: "Xoá",
        icon: "pi pi-trash",
        permission: this.isGranted(),
        command: () => {
          this.deletePolicyDetailUnSave(this.policyDetailTemplate);
        },
      },
    ];

  }

  getDetail(bondPolicyTempId) {
    // this.submitted = true;
    this._productBondPolicyTemplateService.get(bondPolicyTempId).subscribe(
      (res) => {
        if (this.handleResponseInterceptor(res, '')) {
          this.policyTemplate = res.data;
          this.genlistActionTerm(res.data.productBondPolicyDetailTemp);
        }
        this.submitted = false;
      });
  }

  genlistActionTerm(data = []) {
    this.listActionTerm = data.map(policyDetailTemplate => {
      const actionsTerm = [];

      if (this.isGranted([this.PermissionBondConst.BondCaiDat_CSM_CapNhatKyHan])){
				actionsTerm.push({
          data: policyDetailTemplate,
          label: 'Sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            console.log("$event.item.data", $event.item.data);

            this.editBondPolicyDetailTemplate($event.item.data);
          }
				})
			}

      if (this.isGranted([this.PermissionBondConst.BondCaiDat_CSM_XoaKyHan])){
				actionsTerm.push({
          data: policyDetailTemplate,
          label: 'Xoá',
          icon: 'pi pi-trash',
          command: ($event) => {
            this.deleteBondPolicyDetailTemplate($event.item.data);
          }
				})
			}

      if (policyDetailTemplate.status == this.ProductBondPolicyDetailTemplateConst.KICH_HOAT && this.isGranted([this.PermissionBondConst.BondCaiDat_CSM_KyHan_KichHoatOrHuy])) {
        actionsTerm.push({
          data: policyDetailTemplate,
          label: 'Huỷ kích hoạt',
          icon: 'pi pi-times',
          command: ($event) => {
            this.changeStatusBondPolicyDetailTemplate($event.item.data);
          }
        });
      }

      if (policyDetailTemplate.status == this.ProductBondPolicyDetailTemplateConst.KHONG_KICH_HOAT && this.isGranted([this.PermissionBondConst.BondCaiDat_CSM_KyHan_KichHoatOrHuy])) {
        actionsTerm.push({
          data: policyDetailTemplate,
          label: 'Kích hoạt',
          icon: 'pi pi-check',
          command: ($event) => {
            this.changeStatusBondPolicyDetailTemplate($event.item.data);
          }
        });
      }

      return actionsTerm;
    });
    console.log('listActions', this.listActionTerm);
  }

  genListAction(data = []) {
    this.listAction = data.map(productBondPolicyTemplate => {

      const actions = [];

      if (this.isGranted([this.PermissionBondConst.BondCaiDat_CSM_ThemKyHan])){
				actions.push({
          data: productBondPolicyTemplate,
          label: 'Thêm kỳ hạn',
          icon: 'pi pi-plus',
          command: ($event) => {
            this.createPolicyDetailTemplate($event.item.data.bondPolicyTempId);
          }
				})
			}

      if (this.isGranted([this.PermissionBondConst.BondCaiDat_CSM_CapNhatChinhSach])){
				actions.push({
          data: productBondPolicyTemplate,
          label: 'Sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.editBondPolicyTemplate($event.item.data);
          }
				})
			}

      if (this.isGranted([this.PermissionBondConst.BondCaiDat_CSM_XoaChinhSach])){
				actions.push({
          data: productBondPolicyTemplate,
          label: 'Xoá',
          icon: 'pi pi-trash',
          command: ($event) => {
            this.deleteBondPolicyTemplate($event.item.data);
          }
				})
			}

      if (productBondPolicyTemplate.status == this.ProductBondPolicyTemplateConst.KICH_HOAT && this.isGranted([this.PermissionBondConst.BondCaiDat_CSM_KichHoatOrHuy])) {
        actions.push({
          data: productBondPolicyTemplate,
          label: 'Hủy kích hoạt',
          icon: 'pi pi-times',
          command: ($event) => {
            this.changeStatusBondPolicyTemplate($event.item.data);
          }
        });
      }

      if (productBondPolicyTemplate.status == this.ProductBondPolicyTemplateConst.KHONG_KICH_HOAT && this.isGranted([this.PermissionBondConst.BondCaiDat_CSM_KichHoatOrHuy])) {
        actions.push({
          data: productBondPolicyTemplate,
          label: 'Kích hoạt',
          icon: 'pi pi-check',
          command: ($event) => {
            this.changeStatusBondPolicyTemplate($event.item.data);
          }
        });
      }

      if (productBondPolicyTemplate.status == this.ProductBondPolicyTemplateConst.KICH_HOAT){
				actions.push({
          data: productBondPolicyTemplate,
          label: 'Áp dụng bán theo kỳ hạn',
          icon: 'pi pi-clone',
          command: ($event) => {
            this.termSalesApply($event.item.data);
          }
				})
			}

      return actions;
    });
    console.log('listActions', this.listAction);

  }

  createPolicyDetailTemplate(bondPolicyTemplateId) {
    this.policyDetailTemplate = {};
    this.policyDetailTemplate.bondPolicyTempId = bondPolicyTemplateId;
    this.policyDetailTemplate.unSave = false;
    this.submitted = false;
    this.modalDialogPolicyDetailTemplate = true;
  }

  create() {
    this.policyTemplate = {};
    this.submitted = false;
    this.modalDialog = true;
  }

  changeInterestType(value) {
    if (value == this.ProductBondPolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END) {
      this.policyDetailTemplate.interestPeriodType = null;
      this.policyDetailTemplate.interestPeriodQuantity = null;
    } else {
      this.policyDetailTemplate.interestPeriodType = this.ProductBondPolicyDetailTemplateConst.INTEREST_PERIOD_TYPE_MONTH;
    }
  }

  createPolicyDetailTemplateUnSave() {
    this.policyDetailTemplate = {};
    this.policyDetailTemplate.unSave = true;
    this.policyDetailTemplate.interestPeriodType = this.ProductBondPolicyDetailTemplateConst.INTEREST_PERIOD_TYPE_MONTH;
    this.policyDetailTemplate.periodType = this.ProductBondPolicyDetailTemplateConst.INTEREST_PERIOD_TYPE_MONTH;
    console.log("aaaa", this.policyDetailTemplate);
    this.submitted = false;
    this.modalDialogPolicyDetailTemplate = true;
    console.log("createPolicyDetailTemplateUnSave", this.policyDetailTemplate);

  }

  editPolicyDetailUnSave(item) {
    this.policyDetailTemplate = { ...item };
    console.log("editPolicyDetailUnSave", this.policyDetailTemplate);
    this.policyDetailTemplate.unSave = true;
    this.submitted = false;
    this.modalDialogPolicyDetailTemplate = true;

  }

  termSalesApply(item) {
    this.policyTemplate = { ...item };
    this.submitted = false;
    this.modalDialogTermSalesApply = true;
    this.isLoading = true;
    this._productBondPolicyTemplateService.getAllListPolicy(this.page, this.status).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        this.rowsListPolicy = res?.data?.items;
        console.log("this.rowsListPolicy",this.rowsListPolicy);
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }


  editBondPolicyTemplate(item) {
    this.policyTemplate = { ...item };
    this.genlistActionTerm(this.policyTemplate.productBondPolicyDetailTemp);
    this.submitted = false;
    this.modalUpdateDialogPolicyTemplate = true;
  }

  editBondPolicyDetailTemplate(policyDetailTemplate) {
    this.policyDetailTemplate = { ...policyDetailTemplate };
    this.policyDetailTemplate.unSave = false;
    this.submitted = false;
    this.modalDialogPolicyDetailTemplate = true;
    console.log("policyDetailTemplate", this.policyDetailTemplate);
  }

  saveTermSalesApply() {
    this.submitted = true;
    this.policyTemplate.secondary = this.policyTemplate.secondary.map((num) => (parseInt(num)));
		console.log("this.policyTemplate.secondary",this.policyTemplate.secondary);
      this._productBondPolicyTemplateService.addListPolicy(this.policyTemplate.secondary, this.policyTemplate.bondPolicyTempId).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
            this.submitted = false;
            this.setPage({ page: this.page.pageNumber });
            this.hideDialog();
          } else {
            this.submitted = false;
          }
        }, () => {
          this.submitted = false;
        }
      );
    
  }

  save() {
    this.submitted = true;
    if (this.policyTemplate.bondPolicyTempId) {
      this._productBondPolicyTemplateService.update(this.policyTemplate, this.policyTemplate.bondPolicyTempId).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
            this.submitted = false;
            this.setPage({ page: this.page.pageNumber });
            this.hideDialog();
          } else {
            this.submitted = false;
          }
        }, () => {
          this.submitted = false;
        }
      );
    } else {
      console.log(this.policyTemplate);
      this._productBondPolicyTemplateService.create(this.policyTemplate).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
            this.submitted = false;
            this.setPage();
            this.hideDialog();
          } else {
            this.submitted = false;
          }
        }, () => {
          this.submitted = false;
        }
      );
    }
  }

  //Thêm và update kỳ hạn 
  saveProductBondPolicyDetailTemplate() {
    this.submitted = true;
    if (this.policyDetailTemplate.bondPolicyDetailTempId) {
      this._productBondPolicyTemplateService.updateBondPolicyDetailTemplate(this.policyDetailTemplate, this.policyDetailTemplate.bondPolicyDetailTempId).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
            this.submitted = false;
            this.setPage({ page: this.page.pageNumber });
            this.getDetail(this.policyTemplate.bondPolicyTempId);
            console.log("this.policyTemplate.bondPolicyTempId", this.policyTemplate.bondPolicyTempId);
            this.hideDialog('modalDialogPolicyDetailTemplate');
          } else {
            this.submitted = false;
          }
        }, () => {
          this.submitted = false;
        }
      );
    } else {
      this._productBondPolicyTemplateService.createBondPolicyDetailTemplate(this.policyDetailTemplate).subscribe(
        (response) => {

          if (this.handleResponseInterceptor(response, 'Thêm kỳ hạn thành công')) {
            this.submitted = false;
            this.setPage({ page: this.page.pageNumber });
            if (this.policyTemplate.bondPolicyTempId != 0) {
              this.getDetail(this.policyTemplate.bondPolicyTempId);
            }
            console.log("this.policyTemplate.bondPolicyTempId", this.policyTemplate.bondPolicyTempId);
            this.hideDialog('modalDialogPolicyDetailTemplate');
          } else {
            this.submitted = false;
          }
        }, () => {
          this.submitted = false;
        }
      );
    }
  }

  //Save Policy Detail vào table tạm
  savePolicyDetail() {
    if (this.policyDetailTemplate?.fakeId) {
      let index = this.policyTemplate.productBondPolicyDetailTemp.findIndex(item => item.fakeId == this.policyDetailTemplate.fakeId);
      this.policyTemplate.productBondPolicyDetailTemp[index] = { ...this.policyDetailTemplate };
      console.log({ policyTemplate: this.policyTemplate });
    } else {
      this.policyDetailTemplate.fakeId = new Date().getTime();
      if (!this.policyTemplate.productBondPolicyDetailTemp) {
        this.policyTemplate.productBondPolicyDetailTemp = [];
      }
      this.policyTemplate.productBondPolicyDetailTemp.push(this.policyDetailTemplate);
    }
    this.modalDialogPolicyDetailTemplate = false;
  }

  deletePolicyDetailUnSave(item) {
    this.deleteItemDialog = true;
    this.policyDetailTemplate = { ...item };
  }

  confirmDeleteUnSave() {
    this.deleteItemDialog = false;
    let index = this.policyTemplate.productBondPolicyDetailTemp.findIndex(item => item.fakeId == this.policyDetailTemplate.fakeId);
    this.policyTemplate.productBondPolicyDetailTemp.splice(index, 1);
  }

  deleteBondPolicyTemplate(item) {
    this.deleteBondPolicyTemplateDialog = true;
    this.policyTemplate = { ...item };
    // this.contractTemplate = { ...contractTemplate }
		const ref = this._dialogService.open(
			FormNotificationComponent,
			{
				header: "Thông báo",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: {
					title: "Bạn có chắc chắn xóa chính sách?",
					icon: FormNotificationConst.IMAGE_CLOSE,
				},
			}
		);
		ref.onClose.subscribe((dataCallBack) => {
			console.log({ dataCallBack });
				  if (dataCallBack?.accept) {
					  this._productBondPolicyTemplateService.delete(this.policyTemplate.bondPolicyTempId).subscribe(
              (response) => {
                if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
                  this.setPage({ page: this.page.pageNumber });
                  this.policyTemplate = {};
                }
              }, () => {
                this.messageService.add({
                  severity: 'error',
                  summary: '',
                  detail: `Không xóa được chính sách ${this.policyTemplate.name}`,
                  life: 3000,
                });
              }
            );
				  }
			  });
  }

  confirmDeleteBondPolicyTemplate() {
    this.deleteBondPolicyTemplateDialog = false;
    this._productBondPolicyTemplateService.delete(this.policyTemplate.bondPolicyTempId).subscribe(
      (response) => {
        if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
          this.setPage({ page: this.page.pageNumber });
          this.policyTemplate = {};
        }
      }, () => {
        this.messageService.add({
          severity: 'error',
          summary: '',
          detail: `Không xóa được chính sách ${this.policyTemplate.name}`,
          life: 3000,
        });
      }
    );
  }

  deleteBondPolicyDetailTemplate(item) {
    this.deleteBondPolicyDetailTemplateDialog = true;
    this.policyDetailTemplate = { ...item };

  }

  changeStatusBondPolicyTemplate(item) {
    this._productBondPolicyTemplateService.changeStatusBondPolicyTemplate(item.bondPolicyTempId).subscribe(
      (response) => {
        var message = "";
        if (item.status == 'A') {
          message = "Hủy kích hoạt thành công";
        } else {
          message = "Kích hoạt thành công";
        }
        if (this.handleResponseInterceptor(response, message)) {
          this.setPage({ page: this.page.pageNumber });
        }
      }, () => {
        this.messageService.add({
          severity: 'error',
          summary: '',
          detail: `Không thay đổi được trạng thái của chính sách ${item.name}`,
          life: 3000,
        });
      }
    );
  }

  changeStatusBondPolicyDetailTemplate(item) {
    this._productBondPolicyTemplateService.changeStatusBondPolicyDetailTemplate(item.bondPolicyDetailTempId).subscribe(
      (response) => {
        var message = "";
        if (item.status == 'A') {
          message = "Hủy kích hoạt thành công";
        } else {
          message = "Kích hoạt thành công";
        }
        if (this.handleResponseInterceptor(response, message)) {
          this.setPage({ page: this.page.pageNumber });
          this.getDetail(this.policyTemplate.bondPolicyTempId);
        }
      }, () => {
        this.messageService.add({
          severity: 'error',
          summary: '',
          detail: `Không thay đổi được trạng thái của kỳ hạn ${item.name}`,
          life: 3000,
        });
      }
    );
  }

  confirmDeleteBondPolicyDetailTemplate() {
    this.deleteBondPolicyDetailTemplateDialog = false;
    this._productBondPolicyTemplateService.deleteBondPolicyDetailTemplate(this.policyDetailTemplate.bondPolicyDetailTempId).subscribe(
      (response) => {
        if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
          //this.setPage({ page: this.page.pageNumber });

          this.getDetail(this.policyTemplate.bondPolicyTempId);
          this.policyDetailTemplate = {};
        }
      }, () => {
        this.messageService.add({
          severity: 'error',
          summary: '',
          detail: `Không xóa được kỳ hạn ${this.policyDetailTemplate.name}`,
          life: 3000,
        });
      }
    );
  }

      callTriggerFiledError(response, fieldErrors) {
		let errField = this.getKeyFirstNameError(response);
    	if(fieldErrors[errField] !== undefined) {
            fieldErrors[errField] = true;
            console.log(fieldErrors[errField], errField);
    	}
	}

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

    this.page.keyword = this.keyword;
    this.isLoading = true;
    this._productBondPolicyTemplateService.getAll(this.page, this.status).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        this.rows = res?.data?.items;
        if (this.rows?.length) {
          this.genListAction(this.rows);
          this.showData(this.rows)
        }
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      // 
    });
  }



  hideModalAll() {
    this.modalDialog = false;
    this.modalDialogPolicyDetailTemplate = false;
    this.modalUpdateDialogPolicyTemplate = false;
    this.modalDialogTermSalesApply = false;
  }

  hideDialog(modalName?: string) {
    this[modalName] = false;
    if (!modalName) {
      this.modalDialog = false;
      this.modalDialogPolicyDetailTemplate = false;
      this.modalUpdateDialogPolicyTemplate = false;

      this.deleteItemDialog = false;
      this.deleteBondPolicyTemplateDialog = false;
      this.deleteBondPolicyDetailTemplateDialog = false;
      this.modalDialogTermSalesApply = false;
    }
  }

  validForm(): boolean {
    const validRequired = this.policyTemplate?.code
      && this.policyTemplate?.name
      && this.policyTemplate?.type
      && this.policyTemplate?.investorType
      && ( this.policyTemplate?.transferTax || this.policyTemplate.isTransfer == 'N')
      && this.policyTemplate?.incomeTax
      && this.policyTemplate?.isTransfer
      && this.policyTemplate?.classify
      && this.policyTemplate?.minMoney;
    return validRequired;
  }

  validFormPolicyDetailTemplate(): boolean {
    const validRequired = this.policyDetailTemplate?.shortName
                        && this.policyDetailTemplate?.name
                        && this.policyDetailTemplate?.stt
                        && this.policyDetailTemplate?.profit
                        && this.policyDetailTemplate?.interestType
                        && this.policyDetailTemplate?.periodQuantity
                        && this.policyDetailTemplate?.periodType
                        && (this.policyDetailTemplate?.interestType == ProductBondPolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END || 
                          (this.policyDetailTemplate?.interestType !== ProductBondPolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END && this.policyDetailTemplate.interestPeriodQuantity && this.policyDetailTemplate.interestPeriodType));
    return validRequired;
  }

  validFormListPolicy(): boolean {
    const validRequired = this.policyTemplate?.secondary;
      
    return validRequired;
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }

  formatCurrency(value) {
    return value.toLocaleString('de-DE', { style: 'currency', currency: 'USD' });
  }

  resetTransferTax(){
    if (this.policyTemplate.isTransfer == 'N'){
      this.policyTemplate.transferTax = null;
    }
  }

}

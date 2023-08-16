//
import { Component, EventEmitter, Injector, Input, OnInit, Output } from '@angular/core';
import { MessageService, TreeNode } from 'primeng/api';
import { CrudComponentBase } from '@shared/crud-component-base';
import { UserRoleService } from '@shared/service-proxies/role-service';
import { UserTypes } from '@shared/AppConsts';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-website-detail',
  templateUrl: './website-detail.component.html',
  styleUrls: ['./website-detail.component.scss']
})
export class WebsiteDetailComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _userRoleService: UserRoleService,
  ) {
    super(injector, messageService);
    this.userLogin = this.getUser();
  }

  @Input() webKey: number;
  @Input() modalDialogPermissionDetail: boolean;
  @Input() params: any;
  
  @Output() onCloseDialog = new EventEmitter<any>();

  userLogin: any = {};

  permissionWeb: any[] = [];

  permissionsTree: any[] = [];

  selecteds: any[] = [];
  selectedOlds: any[] = [];

  parentNode: any[] =[];

  item = {};

  permissionWebs = [];

  permissionWebKeysSelected = [];

  permissionMax = {
    "permissionInWeb": 0,
    "permissionKeys": [],
    "permissionKeysRemove": [],
  }

  apiGet: Observable<any>;
  apiCreatePermissionDetail: Observable<any>;

  ngOnInit(): void {
    this.permissionWeb = this.params.permissionDetails;
    this.permissionMax.permissionInWeb = this.params?.permissionWeb?.webKey;
    //
    if(UserTypes.TYPE_EPICS.includes(this.userLogin.user_type)) {
      this.apiGet = this._userRoleService.getPermissionOfWebPartner(this.params?.partnerId, this.params?.permissionWeb?.webKey);
    } 
    //
    else if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
      this.apiGet = this._userRoleService.getPermissionOfWebTradingProvider(this.params?.tradingProviderId, this.params?.permissionWeb?.webKey);
    }
    //
    this.isLoading = true;
    this.apiGet.subscribe((res) => {
      // Xử lý loading gọi modal do open modal giật gây khó chịu
      setTimeout(() => { this.isLoading = false; }, 100);
      if(this.handleResponseInterceptor(res, '')) {
        if(res?.data?.length) this.permissionWebKeysSelected = res.data.map(item => item.permissionKey);
        //
        this.selecteds = this.permissionWeb.filter(p => this.permissionWebKeysSelected.includes(p.key));
        this.selectedOlds = [...this.selecteds.map(s => s.key)];
        if(this.permissionWeb) this.handleDataPermision();
      }
    }, (err) => {
        this.isLoading = false;
        this.messageError('Có lỗi xảy ra vui lòng thử lại sau!');      
    });
    
  }

  hideDialog() {
    this.onCloseDialog.emit();
  }
   
  save() {
    this.submitted = true;
    this.isLoading = true;    
    // Xử lý data lấy ra các key
    this.permissionMax.permissionKeys = this.selecteds.map(s => s.key);
    // Lấy ra các key bị xóa
    if(this.selectedOlds.length) {
      this.permissionMax.permissionKeysRemove = this.selectedOlds.filter(keyOld => !this.permissionMax.permissionKeys.includes(keyOld));
    }
    if(UserTypes.TYPE_EPICS.includes(this.userLogin.user_type)) {
      this.apiCreatePermissionDetail = this._userRoleService.createPermissionWebDetailPartner(this.permissionMax, {partnerId: this.params.partnerId});
    } else 
    if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
      this.apiCreatePermissionDetail = this._userRoleService.createPermissionWebDetailTradingProvider(this.permissionMax, {tradingProviderId: this.params.tradingProviderId });
    }
    //
    this.apiCreatePermissionDetail.subscribe((res) => {
      this.submitted = false;
      this.isLoading = false;
      if(this.handleResponseInterceptor(res, 'Thêm thành công')) {
        this.onCloseDialog.emit();
      }
    }, (err) => {
        this.submitted = false;
        this.isLoading = false; 
    });
  }

  // XỬ LÝ CÂY (DỮ LIỆU, SELECT, UNSELECT) ------------------------------------------------------------------

  // XỬ LÝ DỮ LIỆU THÀNH CẤU TRÚC CÂY THEO KEY VÀ PARENT_KEY (CÂY VÔ HẠN CẤP)
  handleDataPermision() {
    this.permissionsTree = [...this.permissionWeb];
    for( let i=1; i <= this.permissionWeb.length; i++) {
      this.permissionsTree = this.permissionsTree.map(item => {
        item.children = this.permissionWeb.filter(c => c.parentKey == item.key);
        item.styleClass = ((item.children?.length || !item.parentKey)  ? 'permission-parents ' : '') + (item.parentKey == null ? 'node-0 ' : '');
        return item;
      });
    }

    this.permissionsTree = this.permissionsTree.filter(item => item.parentKey == null);
    // Call event show all node of tree
    // this.permissionsTree.forEach( node => this.showNodes(node, true));
  }

  // SHOW TẤT CẢ CÁC NODE CÓ TRONG CÂY (TREE)
  showNodes(node:TreeNode, isExpand:boolean) {
    node.expanded = isExpand;
    if (node.children){
        node.children.forEach( childNode => {
            this.showNodes(childNode, isExpand);
        } );
    }
  }

  //  SELECT
  selectedNote(node) {
    this.customDataSelected(node);
  }

  // UNSELECT
  unSelectedNote(node) {
    this.customDataSelected(node, true);
  }

  // CUSTOM BASE XỬ LÝ UNSELECT NODE CON KO UNSELECT NODE CHA
  customDataSelected(node, unSelected: boolean = false) {
    // Xử lý bước 1 (Dùng cho cả Select và unSelect)
    this.parentNode = [];
    this.getParentNode(node);
    this.filterItemNodeDuplidate();
    //
    if(this.parentNode.length) {
      this.parentNode.forEach(node => {
        let issetNode = this.selecteds.find(s => s.key == node.key);
        if(!issetNode) this.selecteds.push(node);
      });
    }
    // Xử lý bước 2 (Chỉ dùng cho unSelect)
    if(unSelected) this.unSelectParentNode(node?.parent);
  }

  // UNSELECT NODE CHA KHI CÁC NODE CON BỊ UNSELECT HẾT
  unSelectParentNode(parentNode) {
    if(parentNode) {
        let unSelect = true;
        //
        parentNode.children.forEach(childNode => {
          let childNodeIssetSelected = this.selecteds.find(s => s.key == childNode.key);
          if(childNodeIssetSelected) unSelect = false;
        });
        //
        if(unSelect) {
          let indexNode = this.selecteds.findIndex(s => s.key == parentNode.key);
          if(indexNode >= 0) this.selecteds.splice(indexNode, 1);
          if(parentNode.parent) this.unSelectParentNode(parentNode.parent);
        }
        //
    }
  }

  // LẤY DANH SÁCH TỔ TIÊN CHA, ÔNG, CỤ... CỦA NODE
  getParentNode(node) {
    if(node?.parent) {
      this.parentNode.push(node.parent);
      this.getParentNode(node.parent);
    }                                
  }

  // LỌC PHẦN TỬ DUPLICATE TRONG DANH SÁCH NODE SELECTED
  filterItemNodeDuplidate() {
    let keyIsset = []
    this.selecteds.forEach((item, index) => {
      if(!keyIsset.includes(item.key)) {
        keyIsset.push(item.key);
      } else {
        this.selecteds.splice(index, 1);
      }
    });
  }

}

import { Component, EventEmitter, Injector, Input, OnInit, Output } from '@angular/core';
import { WebKeys, UserTypes, RoleTypeConst } from '@shared/AppConsts';
import { MessageService, TreeNode } from 'primeng/api';
import { CrudComponentBase } from '@shared/crud-component-base';
import { UserRoleService } from '@shared/service-proxies/role-service';
import PermissionWebConfig from '@shared/PermissionWebConfig';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-create-or-update-role',
  templateUrl: './create-or-update-role.component.html',
  styleUrls: ['./create-or-update-role.component.scss']
})
export class CreateOrUpdateRoleComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _userRoleService: UserRoleService,
  ) {
    super(injector, messageService);
    this.userLogin = this.getUser();

    this.webConfigs = Object.keys(PermissionWebConfig).map((key) => {
      return { ...PermissionWebConfig[key], key: key, label: PermissionWebConfig[key].name };
    });
  }

  @Output() onCloseDialog = new EventEmitter<any>();

  @Input() webKey: number;
  @Input() modalDialog: number;
  @Input() permissionFull: any[] = [];
  @Input() roleInfo: any = {};
  @Input() isDefault: boolean;
  webConfigs: any[] = [];
  
  permissionRole: any[] = [];

  permissionsTree: any[] = [];

  selectedTemps: any[] = [];

  selecteds: any[] = [];
  selectedOlds: any[] = [];

  parentNode: any[] =[];

  userLogin: any = {};

  item = {};

  role = {
    id: 0,
    name: '',
    description: '',
    roleType: null,
    permissionInWeb: null,
    permissionKeys: [],
    permissionKeysRemove: [],
  };

  rows = [];
  header: string = '';

  ngOnInit(): void {
    this.isLoading = true;
    let selfApiGet: Observable<any>; 
    // EPIC
    if(this.userLogin.user_type == UserTypes.EPIC_ROOT) {
        this.permissionRole = [...this.permissionFull];
        // Xử lý quyền theo cấu trúc dữ liệu dạng cây
        if(this.permissionRole) this.handleDataPermision();
        // Get Permission Role
        if(this.roleInfo?.id) {
          this.getPermissionOfRole();
          } else {
            this.isLoading = false;
          }
        //
    } else
    // PARTNER
    if (this.userLogin.user_type == UserTypes.PARTNER_ROOT) {
      selfApiGet = this._userRoleService.getPermissionOfWebPartner(this.userLogin?.partner_id, this.webKey);
    } else
    // TRADING PROVIDER
    if(this.userLogin.user_type == UserTypes.TRADING_PROVIDER_ROOT) {
      selfApiGet = this._userRoleService.getPermissionOfWebTradingProvider(this.userLogin?.trading_provider_id, this.webKey);
    }
    //
    if(selfApiGet) {
      selfApiGet.subscribe((res) => {
        if(!this.roleInfo?.id) this.isLoading = false;
        // Lây danh sách key quyền của partner
        let permissionKeyIsset = res?.data.map(d => d.permissionKey);
  
        // Lấy danh sách permission của partner thuộc danh sách permission khai báo
        this.permissionRole = this.permissionFull.filter(p => permissionKeyIsset.includes(p.key));
  
        // Xử lý quyền theo cấu trúc dữ liệu dạng cây
        if(this.permissionRole) this.handleDataPermision();
  
        // Update Role
        if(this.roleInfo?.id) {
          this.getPermissionOfRole();
        } 
      });
    }
    this.getHeader();
  }

  getHeader(){
    if (this.isDefault){
      this.header = this.role.id ? "Cập nhật vai trò mặc định" : "Thêm vai trò mặc định";
    } else {
      this.header = this.role.id ? "Cập nhật vai trò" : "Thêm vai trò";
    }
  }
  //
  getPermissionOfRole() {
    let selfApiGetPermisionOfRole: any;
    // EPIC
    if(this.userLogin.user_type == UserTypes.EPIC_ROOT) {
      selfApiGetPermisionOfRole = this._userRoleService.getPermissionOfRoleEpic(this.roleInfo.id);
    } else 
    // PARTNER
    if (this.userLogin.user_type == UserTypes.PARTNER_ROOT) {
      selfApiGetPermisionOfRole = this._userRoleService.getPermissionOfRolePartner(this.roleInfo.id);
    } else
    // TRADING PROVIDER
    if(this.userLogin.user_type == UserTypes.TRADING_PROVIDER_ROOT) {
      selfApiGetPermisionOfRole = this._userRoleService.getPermissionOfRoleTradingProvider(this.roleInfo.id);
    }
    //
    selfApiGetPermisionOfRole.subscribe((res) => {
      this.isLoading = false;
      if(this.handleResponseInterceptor(res, '')) {
        // Đổ dữ liệu quyền của Role
        let permissionKeyIsset = res?.data?.permissionKeys.map(d => d.permissionKey);
        this.selecteds = this.permissionRole.filter(p => permissionKeyIsset.includes(p.key));
        this.selectedOlds = [...this.selecteds.map(s => s.key)];
        this.role = res.data;
      }
    }, (err) => {
      this.isLoading = false;
    });
  }
  //
  save() {
    this.submitted = true;
    this.isLoading = true;
    if(this.roleInfo?.id) this.role.id = this.roleInfo.id;
    //
    this.role.permissionInWeb = this.webKey;
    this.role.permissionKeys = this.selecteds.map(s => s.key);
    //
    this.role.permissionKeysRemove = [];
    if(this.selectedOlds?.length) {
      this.role.permissionKeysRemove = this.selectedOlds.filter(keyOld => !this.role.permissionKeys.includes(keyOld));
    }
    // Add quyền web
    let webInfo = this.webConfigs.find(w => w.webKey == this.webKey);
    this.role.permissionKeys.push(webInfo.key);
    //
    if(!this.role.id) {
      let selfApiCreate: any;
      // EPIC
      if(this.userLogin.user_type == UserTypes.EPIC_ROOT) {
        if(this.isDefault){
          this.role.roleType = RoleTypeConst.MAC_DINH;
        }
        selfApiCreate = this._userRoleService.createRoleEpic(this.role);
      } else
      // PARTNER  
      if(this.userLogin.user_type == UserTypes.PARTNER_ROOT) {
        selfApiCreate = this._userRoleService.createRolePartner(this.role);
      }
      // TRADING PROVIDER  
      if(this.userLogin.user_type == UserTypes.TRADING_PROVIDER_ROOT) {
        selfApiCreate = this._userRoleService.createRoleTradingProvider(this.role);
      }
      //
      selfApiCreate.subscribe((res) => {
        this.submitted = false;
        this.isLoading = false;
        if(this.handleResponseInterceptor(res, 'Thêm thành công')) {
          this.onCloseDialog.emit(true);
        }
      }, (err) => {
         this.submitted = false;
         this.isLoading = true; 
      });
    } 
    else {
      let selfApiUpdate: any;
      // EPIC
      if(this.userLogin.user_type == UserTypes.EPIC_ROOT) {
        selfApiUpdate = this._userRoleService.updateRoleEpic(this.role);
      } else 
      // PARTNER
      if(this.userLogin.user_type == UserTypes.PARTNER_ROOT) {
        selfApiUpdate = this._userRoleService.updateRolePartner(this.role);
      } else 
      if (this.userLogin.user_type == UserTypes.TRADING_PROVIDER_ROOT) {
        selfApiUpdate = this._userRoleService.updateRoleTradingProvider(this.role);
      }
      //
      if(selfApiUpdate) {
        selfApiUpdate.subscribe((res) => {
          this.submitted = false;
          this.isLoading = true;
          if(this.handleResponseInterceptor(res, 'Cập nhật thành công')) {
            if(this.handleResponseInterceptor(res, '')) {
              this.onCloseDialog.emit(true);
            }
          }
        }, (err) => {
           this.submitted = false; 
           this.isLoading = true;
        });
      }
    }
  }

  hideDialog() {
    this.onCloseDialog.emit();
  }

  validForm(): boolean {
    const validForm = this.role.name?.trim() && true; 
		return validForm;
	}  

  // XỬ LÝ DỮ LIỆU THÀNH CẤU TRÚC CÂY THEO KEY VÀ PARENT_KEY (CÂY VÔ HẠN CẤP)
  handleDataPermision() {
    this.permissionsTree = [...this.permissionRole];
    for( let i=1; i <= this.permissionRole.length; i++) {
      this.permissionsTree = this.permissionsTree.map(item => {
        item.children = this.permissionRole.filter(c => c.parentKey == item.key);
        item.styleClass = ((item.children?.length || !item.parentKey)  ? 'permission-parents ' : '') + (item.parentKey == null ? 'node-0 ' : '');
        return item;
      });
    }
    //
    this.permissionsTree = this.permissionsTree.filter(item => item.parentKey == null);
  }

  // SHOW TẤT CẢ CÁC NODE CÓ TRONG CÂY (TREE)
  showNodes(node:TreeNode, isExpand:boolean) {
    node.expanded = isExpand;
    if (node.children) {
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

  // LẤY DANH SÁCH TỔ TIÊN CHA, ÔNG, CỤ, KỴ... CỦA NODE
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

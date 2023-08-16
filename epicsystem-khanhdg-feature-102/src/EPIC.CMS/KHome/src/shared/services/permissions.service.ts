import { Injectable } from "@angular/core";
import { UserServiceProxy } from "@shared/service-proxies/service-proxies";
import { BehaviorSubject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class PermissionsService {

    private permissions: any[] = [];
    private permissionSubject = new BehaviorSubject<string[]>([]);

    getPermisions$ = this.permissionSubject.asObservable();

    constructor(private _userService: UserServiceProxy) {
    }

    setPermissions(permissions) {
        this.permissionSubject.next(permissions);
    }

    /**
     * Kiểm tra có permission không
     * @param permissionName
     * @returns
     */
    isGrantedRoot(permissionNames = []): boolean {
        for(let permission of permissionNames) {
            if(permission === undefined) console.log('Key quyền khai báo không tồn tại trong màn này FE!', permission);
            if(this.permissions.includes(permission)) {
                return true;
            }
        }
        return false;
    }
}

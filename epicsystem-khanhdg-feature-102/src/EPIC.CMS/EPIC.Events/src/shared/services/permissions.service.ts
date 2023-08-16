import { Injectable } from '@angular/core';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PermissionsService {
    private permissions: any[] = [];
    private permissionSubject = new BehaviorSubject<string[]>([]);

    constructor(private _userService: UserServiceProxy) {}

    getPermissions$ = this.permissionSubject.asObservable();

    setPermissions(permissions) {
        console.log('setPermission');
        return this.permissionSubject.next(permissions);
    }

    // getPermission() {
    //     this.getPermisions$.subscribe((response) => {
    //         this.permissions = response;
    //     })
    // }

    // getAllPermission() {
    //     this._userService.getAllPermission().subscribe(
    //         (res) => {
    //             this.permissions = res.data;
    //         }, () => {
    //             this.permissions = [];
    //         }
    //     );
    // }

    /**
     * Kiểm tra có permission không
     * @param permissionName
     * @returns
     */
    // isGrantedRoot(permissionNames = []): boolean {
    //     for (let permission of permissionNames) {
    //         if (this.permissions.includes(permission)) {
    //             return true;
    //         }
    //     }
    //     return false;
    // }
}

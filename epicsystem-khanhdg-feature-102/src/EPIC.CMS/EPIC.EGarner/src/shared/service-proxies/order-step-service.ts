import { OrderConst } from '../AppConsts';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable } from "@angular/core";


import { Subject } from 'rxjs';
import { OBJECT_ORDER } from '@shared/base-object';

@Injectable()
export class OrderStepService {
    
    orderInfo = {...OBJECT_ORDER.STEP};
   
    private orderComplete = new Subject<any>();
    
    orderComplete$ = this.orderComplete.asObservable();
   
    resetValue() {
        for (const [key, value] of Object.entries(OBJECT_ORDER.STEP)) {
            this.orderInfo[key] = value;
        }
    }
   
    getOrderInformation() {
        return this.orderInfo;
    }
   
    complete() {
        this.orderComplete.next(this.orderInfo.customerInfo);
    }
}


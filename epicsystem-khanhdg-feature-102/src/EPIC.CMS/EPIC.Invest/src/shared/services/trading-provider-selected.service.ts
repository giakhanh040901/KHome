import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, Subject } from "rxjs";

@Injectable({
    providedIn: "root",
})
// 
export class TradingProviderSelectedService {
    
    private $tradingProviders: BehaviorSubject <[]> = new BehaviorSubject([]);

    public get TradingProviderObservable(): Observable<any> {
        return this.$tradingProviders.asObservable();
    }

    public setSelectedOptions(options?:any) {
        this.$tradingProviders.next(options);
    }

    
}
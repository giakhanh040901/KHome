import { CrudComponentBase } from '@shared/crud-component-base';
import { Inject, Injectable, Injector, Optional } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { Observable, Subject } from "rxjs";
import { TokenService } from '@shared/services/token.service';
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { HttpClient } from "@angular/common/http";
import { AppConsts } from '@shared/AppConsts';

@Injectable({
    providedIn: "root",
})
// 
export class SignalrService {

    constructor(
        private _tokenService: TokenService,
    ) {
        this.baseUrl = AppConsts.remoteServiceBaseUrl;
    }

    private baseUrl: string;
    private hubConnection: any;

    public startConnection() {
        return new Promise((resolve, reject) => {
            this.hubConnection = new HubConnectionBuilder()
                .withUrl(
                    `${this.baseUrl}/hub/real-estate/product-item`,
                    {
                        accessTokenFactory: () => this._tokenService.getToken(),
                        skipNegotiation: true,
                        transport: signalR.HttpTransportType.WebSockets
                    })
                .build();

            this.hubConnection.start().then(() => {
                console.log("connection singlr true");
                return resolve(true);
            })
            .catch((err: any) => {
                console.log("connection singlr false", err);
                reject(err);
            });
        });
    }

    private $productItem: Subject<any> = new Subject<any>();

    public get AllProductItemObservable(): Observable<any> {
        return this.$productItem.asObservable();
    }

    public listenProductItemStatuses() {
        (<HubConnection>this.hubConnection).on("UpdateProductItemStatus", (data: any) => {
            this.$productItem.next(data);
        });
    }

    private $updateProductItem: Subject<any> = new Subject<any>();

    public get UpdateProductItemObservable(): Observable<any> {
        return this.$updateProductItem.asObservable();
    }

    public listenUpdateCountProductItem() {
        (<HubConnection>this.hubConnection).on("UpdateCountProductItem", (data: any) => {
            this.$updateProductItem.next(data);
        });
    }

    private $lastestProductItem: Subject<any> = new Subject<any>();

    public get LastestProductItemObservable(): Observable<any> {
        return this.$lastestProductItem.asObservable();
    }

    public listenUpdateLastestProductItem() {
        (<HubConnection>this.hubConnection).on("UpdateOrderNewInProject", (data: any) => {
            this.$lastestProductItem.next(data);
        });
    }
    
    private $lastestOpenSell: Subject<any> = new Subject<any>();

    public get LastestOpenSellObservable(): Observable<any> {
        return this.$lastestOpenSell.asObservable();
    }

    public listenUpdateLastestOpenSell() {
        (<HubConnection>this.hubConnection).on("OpenSellId", (data: any) => {
            this.$lastestOpenSell.next(data);
        });
    }
}

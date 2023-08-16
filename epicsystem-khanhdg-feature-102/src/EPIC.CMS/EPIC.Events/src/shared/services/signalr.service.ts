import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { AppConsts } from '@shared/AppConsts';
import { TokenService } from '@shared/services/token.service';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
//
export class SignalrService {
  constructor(private _tokenService: TokenService) {
    this.baseUrl = AppConsts.remoteServiceBaseUrl;
  }

  private baseUrl: string;
  private hubConnection: any;

  isSignalRConnected: boolean;
  public startConnection() {
    return new Promise((resolve, reject) => {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(`${this.baseUrl}/hub/event`, {
          accessTokenFactory: () => this._tokenService.getToken(),
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets,
        })
        .build();

      this.hubConnection
        .start()
        .then(() => {
          this.isSignalRConnected = true;
          this.hubConnection.onclose((error) => {
            this.isSignalRConnected = false;
          });
          this.hubConnection.onreconnecting((error) => {
            this.isSignalRConnected = false;
          });
          this.hubConnection.onreconnected((connectionId) => {
            this.isSignalRConnected = true;
          });
          resolve(true);
        })
        .catch((err: any) => {
          this.isSignalRConnected = false;
          reject(err);
        });
    });
  }

  public listen(eventName: string, callback: (...args: any[]) => void) {
    if (!this.hubConnection) {
      throw new Error('SignalR connection is not established. Call startConnection() first.');
    }

    this.hubConnection.on(eventName, callback);
  }

  public startConnectionMSBNoti() {
    return new Promise((resolve, reject) => {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(`${this.baseUrl}/api/payment/msb/receive-notification`, {
          accessTokenFactory: () => this._tokenService.getToken(),
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets,
        })
        .build();

      this.hubConnection
        .start()
        .then(() => {
          console.log('connection singlr true');
          return resolve(true);
        })
        .catch((err: any) => {
          console.log('connection singlr false', err);
          reject(err);
        });
    });
  }

  private $receiveMSBNotification: Subject<any> = new Subject<any>();

  public get _receiveMSBNotification$(): Observable<any> {
    return this.$receiveMSBNotification.asObservable();
  }

  public listenReceiveMSBNotification() {
    (<HubConnection>this.hubConnection).on('UpdateOrderStatus', (data: any) => {
      this.$receiveMSBNotification.next(data);
    });
  }
}

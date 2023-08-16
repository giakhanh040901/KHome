import { Injectable } from '@angular/core';
import { Socket } from 'ngx-socket-io';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalingService {
  constructor(private socket: Socket) { }

  getMessages(event): Observable<any> {
    return this.socket.fromEvent(event);
  }

  sendMessage(event, payload): void {
    this.socket.emit(event, payload);
  }
}

import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class SpinnerService {
  public isShow: boolean = false;
  public isShowSpinner() {
    this.isShow = true;
    this.setTimeOut();
  }
  public remove() {
    this.isShow = false;
  }
  public setTimeOut() {
    setTimeout(() => {
      this.isShow = false;
    }, 60 * 1000);
  }
}

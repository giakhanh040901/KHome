import { Component, Input, OnInit } from "@angular/core";

@Component({
  selector: "overview-card-ratio",
  templateUrl: "./overview-card-ratio.component.html",
  styleUrls: ["./overview-card-ratio.component.scss"],
})
export class OverviewCardRatioComponent implements OnInit {
  @Input()
  public limitSize: number = 0;
  @Input()
  public value: number = 0;
  @Input()
  public color: string = "";
  @Input()
  public backgroundParent: string = "";

  constructor() {}

  ngOnInit() {}

  public getBackGround() {
    const percent = this.value * 3.6;
    let margin: number = 10;
    if (this.value >= 85 && this.value < 95) {
      margin = 5;
    } else if (this.value >= 95 || this.value === 0) {
      margin = 0;
    }
    return `conic-gradient(
      ${this.color} 0deg ${percent}deg,
      ${this.backgroundParent} ${percent}deg ${percent + margin}deg,
      #eeeff7 ${percent + margin}deg ${360 - margin}deg,
      ${this.backgroundParent} ${360 - margin}deg 360deg
    )`;
  }
}

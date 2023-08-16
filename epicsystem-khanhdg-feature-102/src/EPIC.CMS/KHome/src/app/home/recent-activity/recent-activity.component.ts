import { Component, Input, OnInit } from "@angular/core";

@Component({
  selector: "recent-activity",
  templateUrl: "./recent-activity.component.html",
  styleUrls: ["./recent-activity.component.scss"],
})
export class RecentActivityComponent implements OnInit {
  @Input()
  public avatar: string = "";
  @Input()
  public name: string = "";
  @Input()
  public infor: string = "";
  @Input()
  public time: string = "";

  constructor() {}

  ngOnInit() {}

  public handleClick(event: any) {
    if (event) {
    }
  }
}

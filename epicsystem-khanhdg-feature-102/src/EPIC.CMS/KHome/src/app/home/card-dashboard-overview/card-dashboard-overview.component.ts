import { Component, Input, OnInit } from "@angular/core";

@Component({
  selector: "card-dashboard-overview",
  templateUrl: "./card-dashboard-overview.component.html",
  styleUrls: ["./card-dashboard-overview.component.scss"],
})
export class CardDashboardOverviewComponent implements OnInit {
  @Input()
  public styleCard: {
    backgroundColor: string;
    textColor: string;
  } = {
    backgroundColor: "",
    textColor: "",
  };
  @Input()
  public title: string = "";
  @Input()
  public description: string = "";
  @Input()
  public value: number = 0;
  @Input()
  public infor: string = "";
  @Input()
  public valueInfor: string = "";

  constructor() {}

  ngOnInit() {}
}

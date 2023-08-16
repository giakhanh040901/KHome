import { Component, Input, OnInit } from "@angular/core";

@Component({
  selector: "create-account-chart-element",
  templateUrl: "./create-account-chart-element.component.html",
  styleUrls: ["./create-account-chart-element.component.scss"],
})
export class CreateAccountChartElementComponent implements OnInit {
  @Input()
  public width: number = 0;
  @Input()
  public height: number = 0;
  @Input()
  public backgroundColor: string = "";
  @Input()
  public label: string = "";
  @Input()
  public classLabel: string = "";
  @Input()
  public value: number = 0;

  constructor() {}

  ngOnInit() {}
}

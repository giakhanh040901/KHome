import { Component, Input, OnInit, SimpleChanges } from "@angular/core";

@Component({
  selector: "create-account-chart",
  templateUrl: "./create-account-chart.component.html",
  styleUrls: ["./create-account-chart.component.scss"],
})
export class CreateAccountChartComponent implements OnInit {
  @Input()
  public listValue: number[] = [];

  public listElement: {
    width: number;
    height: number;
    backgroundColor: string;
    label: string;
    classLabel: string;
    value: number;
  }[] = [
    {
      width: 360,
      height: 88,
      backgroundColor: "#ECEBFB",
      label: "Đăng ký ngay",
      classLabel: "text-4xl font-bold",
      value: 0,
    },
    {
      width: 330,
      height: 80,
      backgroundColor: "#E1DEFC",
      label: "OTP Sent",
      classLabel: "text-3xl font-bold",
      value: 0,
    },
    {
      width: 300,
      height: 75,
      backgroundColor: "#D7D3F8",
      label: "Nhập OTP thành công",
      classLabel: "text-2xl font-bold",
      value: 0,
    },
    {
      width: 275,
      height: 65,
      backgroundColor: "#C9C6FB",
      label: "Thêm giấy tờ thành công",
      classLabel: "text-xl font-bold",
      value: 0,
    },
    {
      width: 250,
      height: 60,
      backgroundColor: "#B4AFFA",
      label: "Start eKYC",
      classLabel: "text-lg font-bold",
      value: 0,
    },
    {
      width: 230,
      height: 55,
      backgroundColor: "#9A93F1",
      label: "eKYC thành công",
      classLabel: "text-base font-bold",
      value: 0,
    },
    {
      width: 210,
      height: 55,
      backgroundColor: "#827BF0",
      label: "Thêm ngân hàng thành công",
      classLabel: "text-base font-bold",
      value: 0,
    },
    {
      width: 190,
      height: 55,
      backgroundColor: "#5346E0",
      label: "Hoàn thành đăng ký",
      classLabel: "text-base font-bold",
      value: 0,
    },
  ];
  constructor() {}

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (this.listValue && this.listValue.length === 8) {
      this.listElement.forEach((e, i) => {
        e.value = this.listValue[i];
      });
    }
  }
}

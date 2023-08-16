import { Component, Input, OnInit, SimpleChanges } from "@angular/core";

@Component({
  selector: "card-dashboard-galleria",
  templateUrl: "./card-dashboard-galleria.component.html",
  styleUrls: ["./card-dashboard-galleria.component.scss"],
})
export class CardDashboardGalleriaComponent implements OnInit {
  @Input()
  public srcImg: string = "";
  @Input()
  public title: string = "";
  @Input()
  public project: {
    name: string;
    position: string;
    quantity: number;
    sold: number;
    acreage: string;
  } = {
    name: "",
    position: "",
    quantity: 0,
    sold: 0,
    acreage: "",
  };
  public listProjectInfor: {
    label: string;
    infor: string | number;
  }[] = [];
  constructor() {}

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes.project) {
      const { project } = this;
      this.listProjectInfor = [
        {
          label: "Tên dự án",
          infor: project.name,
        },
        {
          label: "Vị trí",
          infor: project.position,
        },
        {
          label: "Số lượng sản phẩm",
          infor: project.quantity,
        },
        {
          label: "Đã bán",
          infor: project.sold,
        },
        {
          label: "Diện tích",
          infor: project.acreage,
        },
      ];
    }
  }
}

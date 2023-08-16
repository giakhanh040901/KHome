import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  Input,
  OnInit,
  SimpleChanges,
  ViewChild,
} from "@angular/core";

@Component({
  selector: "overview-card",
  templateUrl: "./overview-card.component.html",
  styleUrls: ["./overview-card.component.scss"],
})
export class OverviewCardComponent implements OnInit {
  @Input()
  public backgroundInfor: string = "";
  @Input()
  public textColorInfor: string = "";
  @Input()
  public contentFirstLevel: string = "";
  @Input()
  public valueFirstLevel: number = 0;
  @Input()
  public contentSecondLevel: string = "";
  @Input()
  public valueSecondLevel: number = 0;
  @Input()
  public transactionColor: string = "";
  @ViewChild("divOverviewCardRatio", { static: false })
  divOverviewCardRatio: ElementRef<HTMLDivElement>;

  constructor(private changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {}

  ngAfterViewInit() {
    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  public getHeight(value: number) {
    return `calc(100% - ${value}px)`;
  }

  public get getPadding() {
    return this.divOverviewCardRatio?.nativeElement?.offsetWidth / 6 || 0;
  }

  public get transactionValue() {
    return !!this.valueFirstLevel
      ? Number(((this.valueSecondLevel / this.valueFirstLevel) * 100).toFixed())
      : 0;
  }
}

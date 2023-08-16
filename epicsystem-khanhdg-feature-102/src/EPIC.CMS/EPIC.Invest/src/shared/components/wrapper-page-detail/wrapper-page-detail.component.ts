import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';

@Component({
  selector: 'wrapper-page-detail',
  templateUrl: './wrapper-page-detail.component.html',
  styleUrls: ['./wrapper-page-detail.component.scss']
})
export class WrapperPageDetailComponent implements OnInit {

    constructor() { }

    @Input() pageName: string;
    @Output() _countScrollHeight: EventEmitter<number> = new EventEmitter<number>();
    @Output() _contentHeights: EventEmitter<number[]> = new EventEmitter<number[]>();

    ngOnInit(): void {}

    @ViewChild('pageEl') pageEl: ElementRef<HTMLElement>;

    ngAfterViewInit() {
        setTimeout(() => {
            // TÍNH CHIỀU CAO NỘI DUNG PAGE TABVIEW KHI CHƯA CÓ NỘI DUNG 
            let pageLayoutHeight = document.getElementById("layout-content").clientHeight;
            // CHIỀU CAO PAGE KHI CHƯA LOAD DỮ LIỆU API
            let pageHeightInit = this.pageEl.nativeElement.offsetHeight;
            // PADDING CỦA TABVIEW-PANEL
            this.tabviewPanelPadding = document.querySelectorAll(".p-tabview-panel")[0].clientHeight;
            // CHIỀU CAO CỦA TABVIEW-PANEL
            this.tabviewPanelHeight = pageLayoutHeight - (pageHeightInit - this.tabviewPanelPadding) - 5; // Sai số 5->10
            // CHIỀU CAO NỘI DUNG CỦA TABVIEW-PANEL SAU KHI TRỪ PADDING
            this.tabviewContentHeight = this.tabviewPanelHeight - this.tabviewPanelPadding;
            // DANH SÁCH CÁC ELEMENT TABVIEW-PANEL
            this.countHeightContentTabview();
            let tabviewContentHeights: number[] = [];
            for(let i = 1; i <= 20;i++) {
                tabviewContentHeights.push(this.tabviewContentHeight);
            }
            this._contentHeights.emit(tabviewContentHeights);
            // console.log('pageEl____', [this.tabviewContentHeight]);
        }, 0);
    }

    tabviewPanelPadding: number;
    tabviewContentHeight: number;
    tabviewPanelHeight: number;
    callbackCount: number = 1;
    countHeightContentTabview(timeout=300) {
        setTimeout(() => {
            let tabviewContentHeights: number[] = [];
            let tabviewPanels: any = document.getElementsByTagName("p-tabpanel");
            if(tabviewPanels.length === 1 && this.callbackCount < 4) {
                console.log('callbackCountContentHeightTabview', this.callbackCount);
                this.callbackCount++;
                this.countHeightContentTabview(250);
            }
            for(let i=0; i < tabviewPanels.length; i++) {
                const panelElement = tabviewPanels[i];
                const listClass: HTMLElement = panelElement.className;
                // NẾU TỒN TẠI CLASS pt-0 hoặc pb-0 THÌ contentHeight CỦA TABVIEW-PANEL PHẢI CỘNG THÊM PHẦN PADDING BỊ BỎ ĐI(XỬ LÝ pt-0 pb-0 trong theme.scss)
                let tabviewContentHeightCustom = this.tabviewContentHeight;
                if(listClass.toString().search('pt-0') !== -1) tabviewContentHeightCustom = tabviewContentHeightCustom + (this.tabviewPanelPadding/2);
                if(listClass.toString().search('pb-0') !== -1) tabviewContentHeightCustom = tabviewContentHeightCustom + (this.tabviewPanelPadding/2);
                if(listClass.toString().search('table') !== -1) tabviewContentHeightCustom = tabviewContentHeightCustom + this.tabviewPanelPadding;
                // SET HEIGHT CỐ ĐỊNH CHO CÁC TABVIEW-PANEL
                panelElement.getElementsByTagName("div")[0].style.height = this.tabviewPanelHeight+'px';
                // DANH SÁCH GIÁ TRỊ HEIGHT CONTENT CỦA TỪNG TABVIEW-PANEL
                tabviewContentHeights[i] = tabviewContentHeightCustom;
            }
            this._contentHeights.emit(tabviewContentHeights);
        }, timeout);
        
    }
}

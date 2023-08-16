import { animate, style, transition, trigger } from '@angular/animations';
import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { AppConsts, SearchConst, TableConst, YesNoConst } from '@shared/AppConsts';
import { IAction, IColumn } from '@shared/interface/p-table.model';
import { Page } from '@shared/model/page';
import { DialogService } from 'primeng/dynamicdialog';
import { Image } from 'primeng/image';
import { Paginator } from 'primeng/paginator';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column.component';

@Component({
    selector: 'e-table',
    templateUrl: './e-table.component.html',
    styleUrls: ['./e-table.component.scss'],
    animations: [
        trigger('animationRefresh', [
        // transition(':enter', [
        //   style({ opacity: 1 }),
        //   animate('300ms', style({ opacity: 1 })),
        // ]),
        transition(':leave', [
            style({ height: 0, opacity: 1 }),
            animate('100ms', style({ opacity: 1})),
        ])
    ])]
})
export class ETableComponent implements OnInit {

    constructor(
		private dialogService: DialogService,
		private ref: ChangeDetectorRef
	) { }

	@Input() rows: any[] = [];
	@Input() class: string;
	@Input() styleClassPaginator: string = "show-option-all";
	@Input() isLoading: boolean = false;
	@Input() rowHover: boolean = true;
	@Input() responsiveLayout: string = 'scroll';
	@Input() dataKey: string = 'id';
	@Input() reorderableColumns: boolean = false;
	@Input() showCurrentPageReport: boolean = true;
	@Input() paginator: boolean = false;
	@Input() scrollable: boolean = true;
	@Input() additionalHeight: number = 0;
	@Input() columnCacheName: string;
	@Input() columns: IColumn[] = [];
	@Input() listAction: IAction[][] = [];
	
	@Input() onChangeAtributionData: boolean = true;
	@Input() isShowPaginator: boolean = true;

	@Input() keyword: string;

	@Input() page: Page = new Page();
	@Output() pageChange: EventEmitter<Page> = new EventEmitter<Page>();

	@Input() selectedItems: any[] = [];
	@Output() selectedItemsChange: EventEmitter<any> = new EventEmitter<any>();
	@Output() _onChangeCheckbox: EventEmitter<any> = new EventEmitter<any>();

	@Input() isShowIconFilter: boolean = true;	
	@Input() isShowFilter: boolean = true;	
	@Output() isShowFilterChange: EventEmitter<boolean> = new EventEmitter<boolean>();

	@Output() _onPageChange: EventEmitter<any> = new EventEmitter<any>();
	@Output() _onSort: EventEmitter<any> = new EventEmitter<any>();

	@Input() tabContentHeight: number = 0;
	@Input() idHeader: string;
	@Input() scrollHeightConst: number = 0;
	@Input() isHeightAuto: boolean = false;

	columnShows: IColumn[] = [];

	permissibleErrorScrollHeight: number = 10; // Sai số cho phép
    divFilterHeight: number = 0;
	scrollHeight: number = 0;

	imageDefault = AppConsts.imageDefault;

	isRowOdds: boolean = false;
	isShowTable: boolean = false;

	TableConst = TableConst;
	YesNoConst = YesNoConst;

	@ViewChild('eTable') eTable: ElementRef<HTMLElement>;
	@ViewChild('wrapperPaginator') wrapperPaginator: ElementRef<HTMLElement>;
	@ViewChild('paginatorEl', { static: false }) paginatorEl: Paginator;

	keywordSubject: Subject<string> = new Subject<string>();

	sortData: any[] = [];
	baseUrl: string;

	ngOnInit(): void {
		this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
		this.keywordSubject.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
            this._onPageChange.emit(this.page);
        });
	}

	ngOnChanges(changes: SimpleChanges) {
		if(changes?.columns) {
			this.setAtributionColumn();
			let columnCaches = this.getLocalStorageColumns(this.columnCacheName);
			let isUpdateColumnCache = columnCaches && (columnCaches?.length === this.columns?.length);
			//
			if(isUpdateColumnCache) {
				let columnCacheNews = this.columns.map((column, index) => {
					return {
						...column,
						isShow: columnCaches[index].isShow
					}
				});
				this.setLocalStorageColumns(columnCacheNews);
				this.columns = columnCacheNews;
			} 
            this.setDistanceFrozenColumn();
		}

		if(changes?.rows) {
			// UPDATE COLOR BACKGROUND ROW THEO SỐ DÒNG CHẴN LẺ (UI)
			if(this.rows.length < this.page.perPageOptions[0]) {
				this.isRowOdds = !(this.rows?.length%2 === 0);
			}
			
			// FIX DISPLAY RESET PAGINATOR VỀ TRANG ĐẦU KHI RELOAD DATA
			if(this.paginatorEl && this.page.pageNumber === 0) {
				this.paginatorEl.first = 0;
			}
		}
		// console.log('onChanges', changes);
	}

    ngAfterViewInit() {
		setTimeout(() => {
			// TÍNH CHIỀU CAO TABLE LIST Ở PAGE DANH SÁCH
			if(!this.scrollHeightConst) {
				if(!this.tabContentHeight) {
					this.divFilterHeight = document.getElementById("filter-table")?.clientHeight || 0;
					let pageHeight = document.getElementById("layout-content").clientHeight;
					let pageHeaderHeight = document.getElementById("page-header")?.clientHeight || 0;
					this.scrollHeight = pageHeight - (pageHeaderHeight + this.eTable.nativeElement.offsetHeight);
				} else {
					// TÍNH CHIỀU CAO TABLE LIST Ở PAGE DETAIL TRONG TAB_VIEW
					let pageHeaderHeight = document.getElementById(this.idHeader)?.clientHeight || 0;
					this.scrollHeight = this.tabContentHeight - (this.eTable.nativeElement.offsetHeight + pageHeaderHeight) - 10;	// Sai số 5
				}
			}
		}, 0);
	}

	setPage(event?: any) {
		if(event) {
			this.page.pageNumber = event.page;
			this.page.pageSize = event.rows;
			this.pageChange.emit(this.page);
			if(this.page.pageSize !== this.page.pageSizeAll) {
				if(this.page.pageNumber === 0) {
					this.reRenderHtml('isShowPaginator');
				}
				this._onPageChange.emit(this.page);
			} else {
				this.loadMore();
			}
		} 
	}

	reRenderHtml(variableName: string) {
		this[variableName] = false;
		this.ref.detectChanges();
		this[variableName] = true;
	}

	loadMore() {
		const elementWrapperPaginator = this.wrapperPaginator.nativeElement;
		const elementLabelPageSize = elementWrapperPaginator.querySelector(".p-dropdown-label");
		elementLabelPageSize.textContent = "Tất cả";
		this.ref.detectChanges();
		this._onPageChange.emit();
	}

	// LOAD MORE DATA WITH SOLUTION EVENT SCROLL
	@ViewChild('wrapperETable') wrapperETable: ElementRef<HTMLElement>;
	scrollHeightMaxBefore: number = 0;
	onScroll() {
		const elementTable = this.wrapperETable.nativeElement;
		let pos = (elementTable.scrollTop) + elementTable.offsetHeight;
		let max = elementTable.scrollHeight;
		if((pos >= (max - 250)) && max > this.scrollHeightMaxBefore && this.rows.length < this.page.totalItems && this.page.pageSize === this.page.pageSizeAll && !this.isLoading) {
			this.scrollHeightMaxBefore = max;
			this.loadMore();
		}
	}

	setAtributionColumn() {
		this.columns = this.columns.map((col: IColumn, index) => { 
			return { ...col, 
				position: index+1, 
				isShow: col?.isShow === undefined ? true : col.isShow,
				isSort: !!col?.isSort, 
				type: col?.type || TableConst.columnTypes.TEXT,
				cutText: (!col?.isResize && col.field !== 'id') ? `b-cut-text-${col.width}` : '',
				displaySettingColumn: col?.displaySettingColumn === undefined ? true : col.displaySettingColumn,
				isPermission: col?.isPermission === undefined ? true : col.isPermission,
				class: (
						[ 
							TableConst.columnTypes.CHECKBOX_ACTION, TableConst.columnTypes.CHECKBOX_SHOW, 
							TableConst.columnTypes.ACTION_BUTTON, TableConst.columnTypes.DATE,
							TableConst.columnTypes.IMAGE, TableConst.columnTypes.ACTION_ICON,
						].includes(col?.type) && (col?.class + ' justify-content-center text-center')
					)
					|| (col?.type === TableConst.columnTypes.CURRENCY && (col?.class + ' justify-content-end text-right'))
					|| col?.class
				}
			}
		);
	}
	
  	setColumn() {
		this.dialogService.open(
			FormSetDisplayColumnComponent,
			{    
				header: "Sửa cột hiển thị",
				width: '300px',
				styleClass:'dialog-setcolumn',
				baseZIndex: 10000,
				data: {
					cols: this.columns,
				},
			}
		).onClose.subscribe((response) => {
			if (response?.accept) {
				this.columns = response.data;
				this.setLocalStorageColumns(this.columns);
				this.setDistanceFrozenColumn();
			}
		});
	}

	// set khoảng cách fixed các cột fix lỗi của theme khi xử lý động các cột frozen
  	setDistanceFrozenColumn() {
		let rightValueStyleFrozenColumn = 0;
		let leftValueStyleFrozenColumn = 0;
		this.columns = this.columns.map((item: IColumn) => {
			if(item?.isFrozen && item?.alignFrozen == TableConst.alignFrozenColumn.LEFT) {
				item.left = leftValueStyleFrozenColumn;
				leftValueStyleFrozenColumn += item?.width;
			}
			return item;
		});
		//
		this.columns = this.columns.reverse().map((item: IColumn) => {
			if(item?.isFrozen && item?.alignFrozen === TableConst.alignFrozenColumn.RIGHT) {
				item.right = rightValueStyleFrozenColumn;
				rightValueStyleFrozenColumn += item?.width;
			}
			return item;
		}).reverse();
		//
		this.columnShows = this.columns.filter(col => col?.isShow);
		this.reRenderHtml('isShowTable');
  	}

  	getLocalStorageColumns(key) {
		return JSON.parse(localStorage.getItem(key))
	}

	setLocalStorageColumns(data) {
		return localStorage.setItem(this.columnCacheName, JSON.stringify(data));
	}

	getStyleColumn(col) {
		return	{  
			'max-width': !col?.isResize ? col?.width+'rem' : '', 
			'min-width': col?.width+'rem', 
			'right': col?.right+'rem',
			'left': col?.left+'px',
		};
	}

	onSelectedChange() {
		this.selectedItemsChange.emit(this.selectedItems);
		this._onChangeCheckbox.emit(this.selectedItems);
	}

	changeDisplayFilter() {
		this.isShowFilter = !this.isShowFilter;
		this.scrollHeight = !this.isShowFilter ? (this.scrollHeight + this.divFilterHeight) : (this.scrollHeight - this.divFilterHeight);
		this.isShowFilterChange.emit(this.isShowFilter);
	}

	onSort(event: any) {
		if (JSON.stringify(this.sortData) != JSON.stringify(event?.multisortmeta)){
			this.sortData = [];
			event?.multisortmeta?.forEach(meta => {
				this.sortData.push({
				field: meta.field,
				order: meta.order
				});
			});
			this._onSort.emit(this.sortData);
		}
	}

	@ViewChild('pImage') pImage: Image;
	src: string;
	showImage(src) {
		this.src = src;
		this.pImage.onImageClick();
	}
}
export class Page {

    pageSizeAll = 9999999;

    perPageOptions: any = [25, 50, 100, 200, this.pageSizeAll];

    pageSize: number = this.perPageOptions[0];
    // The total number of elements
    totalItems: number = 0;
    // The total number of pages
    totalPages: number = 0;
    // The current page number
    pageNumber: number = 0;
    //
    pageNumberLoadMore: number = 0;
    pageSizeLoadMore: number = 200;

    keyword: string = '';

    isActive: boolean | string;

    getPageNumber() {
        let pageNumber = this.pageNumber;
        if(this.pageSize === this.pageSizeAll) {
            pageNumber = this.pageNumberLoadMore;
            this.pageNumberLoadMore++;
        } else {
			this.pageNumberLoadMore = 0;
        }
        return pageNumber + 1;
    }

    getPageSize() {
        return (this.pageSize !== this.pageSizeAll) ? this.pageSize : this.pageSizeLoadMore;
    }
}

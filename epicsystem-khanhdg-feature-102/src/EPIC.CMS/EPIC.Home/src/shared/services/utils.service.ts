import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class AppUtilsService {
    constructor() {
    }

    /**
     * đảo từ 1-12-2021 -> 2021-12-1
     * @param date
     * @returns
     */
    reverseDateString(date: string) {
        return date.split(/[-,/]/).reverse().join("-");
    }

    /**
     * tạo một thẻ a download file
     * @param fileName tên file
     * @param href đường dẫn
     */
    makeDownload(fileName: string, href: string) {
        let a = document.createElement("a");
        a.href = href;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        a.remove();
    }

    getBaseUrl() {
        return document.getElementsByTagName('base')[0].href;
    }

    // Dashboard Mobile
    transformMoney(num: number, ...args: any[]): string {
        const value = `${num}`;
		if (value === '' || value === null || typeof value === 'undefined') {
			return '';
		}

		let locales = 'vi-VN';
		const cur = Number(value);

		if (args.length > 0) {
			locales = args[0];	
		}
        const result = new Intl.NumberFormat(locales).format(cur);
		return result === 'NaN' ? '' : this.replaceAll(result,',', '.');
    }

    replaceAll(str, find, replace) {
        var escapedFind=find.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
        return str.replace(new RegExp(escapedFind, 'g'), replace);
    }
}

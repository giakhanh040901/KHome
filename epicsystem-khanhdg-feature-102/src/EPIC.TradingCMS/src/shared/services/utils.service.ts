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
}

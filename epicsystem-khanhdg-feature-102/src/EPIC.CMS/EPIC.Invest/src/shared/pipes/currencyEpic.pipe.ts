import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'currencyEpic'
})
export class FormatCurrencyPipe implements PipeTransform {

    constructor() {

	}

    transform(value: string, ...args: any[]): string {
		if (value === '' || value === null || typeof value === 'undefined') {
			return '';
		}

		let locales = 'vi-VN';
		const cur = Number(value);

		if (args.length > 0) {
			locales = args[0];	
		}
        const result = new Intl.NumberFormat(locales).format(cur);

		return result === 'NaN' ? '' : result;
    }
}

import { Pipe, PipeTransform } from '@angular/core';
import { IValueFormatter } from '@shared/interface/InterfaceConst.interface';

@Pipe({
  name: 'valueFormatter',
})
export class ValueFormatterPipe implements PipeTransform {
  // value: cell, valueFormatter: func format
  transform(value: any, valueFormatter: Function): string {
    if (valueFormatter && typeof valueFormatter === 'function') {
      return valueFormatter({
        data: value,
      } as IValueFormatter);
    }
    return value;
  }
}

import * as moment from 'moment';
import { AppConsts, COMPARE_TYPE, ETypeFormatDate } from './AppConsts';

export function compareDate(firstDate: Date, secondDate: Date, type: number) {
  if (firstDate && firstDate instanceof Date && secondDate && secondDate instanceof Date) {
    const firstDateTime = firstDate.getTime();
    const secondDateTime = secondDate.getTime();
    switch (type) {
      case COMPARE_TYPE.EQUAL:
        return firstDateTime === secondDateTime;
      case COMPARE_TYPE.GREATER:
        return firstDateTime > secondDateTime;
      case COMPARE_TYPE.LESS:
        return firstDateTime < secondDateTime;
      case COMPARE_TYPE.GREATER_EQUAL:
        return firstDateTime >= secondDateTime;
      case COMPARE_TYPE.LESS_EQUAL:
        return firstDateTime <= secondDateTime;
    }
  }
  return null;
}

export function formatCurrency(value: any): string | 0 {
  return new Intl.NumberFormat('en-DE').format(value);
}

export function formatDate(value: any, type: ETypeFormatDate) {
  return moment(value).isValid() && value ? moment(value).format(type) : '';
}

export function formatCalendarItem(datetime: string) {
  return moment(new Date(datetime)).format('YYYY-MM-DDTHH:mm:ss');
}

export function formatCalendarDisplayItem(datetime: string) {
  return new Date(datetime);
}

export function formatUrlImage(srcImage: string) {
  const baseUrl = AppConsts.remoteServiceBaseUrl;
  const result = srcImage.replace(`${baseUrl}/`, '');
  return result;
}

export function getUrlImage(srcImage: string) {
  const baseUrl = AppConsts.remoteServiceBaseUrl;
  return `${baseUrl}/${srcImage}`;
}

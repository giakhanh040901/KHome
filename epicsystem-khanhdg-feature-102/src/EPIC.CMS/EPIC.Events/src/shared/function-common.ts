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

export function convertTimeToAPI(time: string) {
  const times = time.split(':');
  return Number(times[0]) * 3600 + Number(times[1]) * 60;
}

export function convertTimesToHHMM(times: number) {
  if (times >= 0) {
    const totalMinutes = Math.floor(times / 60);

    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;

    return `${hours < 10 ? `0${hours}` : hours}-${minutes < 10 ? `0${minutes}` : minutes}`;
  } else {
    return '';
  }
}

export function convertTimesToHHMMSS(times: number) {
  if (times > 0) {
    const result = new Date(times).toISOString().slice(11, 19);
    return `${result}`;
  } else {
    return '00:00:00';
  }
}

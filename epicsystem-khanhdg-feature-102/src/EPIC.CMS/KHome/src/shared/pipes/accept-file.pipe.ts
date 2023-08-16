import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'acceptFile'
  })
  export class AcceptFilePipe implements PipeTransform {
  
      transform(type: string): unknown {
          let accept: string;
          switch(type) {
              case 'image':
                  accept = 'image/*';
                  break;
              case 'video':
                  accept = 'video/mp4,video/x-m4v,video/*';
                  break;
              case 'media': 
                  accept = 'image/*,video/mp4,video/x-m4v,video/*';
                  break;
              default: 
                  accept = '';
          }
          // 
          return accept;
      }
  
  }
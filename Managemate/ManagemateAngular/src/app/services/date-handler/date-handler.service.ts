import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class DateHandlerService {

  constructor(private translate:TranslateService) { }

  
  changeToUtc(date: Date): Date {

    return new Date(
      Date.UTC(
        date.getFullYear(),
        date.getMonth(),
        date.getDate(),
        date.getHours(),
        date.getMinutes(),
        date.getSeconds(),
        date.getMilliseconds()
      )
    );

  }

  formatDate(input:Date): string {

    const date = new Date(input);
    const dsep = "-";
    const tsep = ":";

    let date_array:string[] = [
      date.getUTCDate().toString(),
      (date.getUTCMonth()+1).toString(),
      date.getUTCFullYear().toString(),
      date.getUTCHours().toString(),
      date.getUTCMinutes().toString()
    ];

    for(let i = 0; i < 5; i++){

      if(date_array[i].length < 2) date_array[i] = "0"+date_array[i];

    }    

    return date_array[0] + dsep + date_array[1] + dsep + date_array[2] + ", " + date_array[3] + tsep + date_array[4];

  }

  get_month_name(month:number):string{

    return this.translate.instant("months."+month);

  }

}

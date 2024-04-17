import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService {
  private errorMessages: { [key: string]: string } = {};

  constructor(private translate: TranslateService) {
    this.loadErrorMessages();
  }

  private loadErrorMessages(): void {
    this.translate.get('errorMessages').subscribe(
      (translations) => {
        this.errorMessages = translations;
      },
      (error) => {
        console.error('Failed to load error messages:', error);
        this.errorMessages = { 'default': 'Unknown error' };
      }
    );
  }

  getErrorMessage(code: string): string {
    return this.errorMessages[code] || this.errorMessages['default'];
  }
}

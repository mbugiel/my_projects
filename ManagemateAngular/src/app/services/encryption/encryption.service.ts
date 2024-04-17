import { Injectable } from '@angular/core';
import { AES,enc } from 'crypto-js';
import { environment } from '../../../environments/environment.development';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class EncryptionService {
  private secretKey = environment.AES_KEY;

  constructor(private cookie: CookieService) { }

  encrypt(data: any): string {
    return AES.encrypt(JSON.stringify(data), this.secretKey).toString();
  }

  decrypt(key: string) {
    const encryptedData = this.cookie.get(key);
    if (encryptedData) {
      const decryptedData = AES.decrypt(encryptedData, this.secretKey).toString(enc.Utf8);
      return JSON.parse(decryptedData);
    }
  }

  decryptStr(data: string) {
    if (data) {
      return JSON.parse(AES.decrypt(data, this.secretKey).toString(enc.Utf8));
    }
  }

}

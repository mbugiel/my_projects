import { Component, OnInit,  ElementRef, ViewChild } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MatInputModule} from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import {MatSelectModule} from '@angular/material/select';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
import { Output_Cities_List_Model } from '../../../../shared/interfaces/API_Output_Models/Cities_List_Models/Output_Cities_List_Model';
import { ClientService } from '../../../../services/client/client.service';
import { Add_Client_Data } from '../../../../shared/interfaces/API_Input_Models/Client_Models/Add_Client_Data';
import {MatIconModule} from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { EditDialogComponent } from '../../../partials/edit-dialog/edit-dialog.component';
import { AuthService } from '../../../../services/auth/auth.service';
import { Output_Company_Model } from '../../../../shared/interfaces/API_Output_Models/Company_Models/Output_Company_Model';
import { AccountService } from '../../../../services/account/account.service';
import { Add_Company_Data } from '../../../../shared/interfaces/API_Input_Models/Company_Models/Add_Company_Data';
import { Edit_Company_Data } from '../../../../shared/interfaces/API_Input_Models/Company_Models/Edit_Company_Data';
//test

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule, MatIconModule, ],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent implements OnInit {

  imageUrl: any | undefined;

  public account_Form!:FormGroup;
  public submitted:boolean = false;
  public emptyError: boolean = false;
  public LoggedOut:boolean = true;

  public mustFill!:boolean;
  public isEmpty!:boolean;
  public isEditing:boolean = false;

  public display_file_size_error:boolean = false;
  public max_file_size_MB:number = 1.5;

  public fileUploaded!:string;
  public fileType!:string | undefined;
  private supportedFileTypes: string[] = ['image/png', 'image/jpeg'];
  public wrongType:boolean = false;

  public pl_language:boolean;

  public isLoadedCitiesList:boolean = false;
  public isLoadedAccountInfo:boolean = true;

  @ViewChild('inputCity') inputCity!: ElementRef;

  @ViewChild('inputSurname') inputSurname!: ElementRef;
  @ViewChild('inputName') inputName!: ElementRef;
  @ViewChild('inputCompanyName') inputCompanyName!: ElementRef;
  @ViewChild('inputNIP') inputNIP!: ElementRef;
  @ViewChild('inputPhoneNumber') inputPhoneNumber!: ElementRef;
  @ViewChild('inputEmail') inputEmail!: ElementRef;
  @ViewChild('inputAddress') inputAddress!: ElementRef;
  @ViewChild('inputPostalCode') inputPostalCode!: ElementRef;
  @ViewChild('inputBankName') inputBankName!: ElementRef;
  @ViewChild('inputBankNumber') inputBankNumber!: ElementRef;
  @ViewChild('inputWebPage') inputWebPage!: ElementRef;
  @ViewChild('inputMoneySign') inputMoneySign!: ElementRef;

  public cities_list: Array<Output_Cities_List_Model> = new Array<Output_Cities_List_Model>;
  public cities_list_filtered!: Array<Output_Cities_List_Model>;

  public editing_account_info!:Output_Company_Model;

  constructor(
    private fb: FormBuilder,
    private auth:AuthService,
    private account_service:AccountService,
    private router:Router,
    private translate:TranslateService,
    private dialog:MatDialog
  ){

    this.pl_language = translate.currentLang == "pl";

    this.account_Form = this.fb.group({
      inputSurname: ['', Validators.required],
      inputName: ['', Validators.required],
      inputCompanyName: ['', Validators.required],
      inputNIP: ['', Validators.required],
      inputPhoneNumber: ['', [Validators.required, Validators.pattern("\\+?[0-9]+(\\s[0-9]+)*")]],
      inputEmail: ['', [Validators.required, Validators.email]],
      inputAddress: ['', Validators.required],
      inputCity: ['', Validators.required],
      inputPostalCode: ['', Validators.required],
      inputBankName: ['', Validators.required],
      inputBankNumber: ['', Validators.required],
      inputWebPage: ['', Validators.required],
      inputMoneySign: ['', Validators.required],
      inputMoneySignDecimal: ['', Validators.required]
    });

  }


  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }

  getAllCities(){
    this.account_service.get_cities_list().subscribe({

      next: response => {

        this.cities_list = response.responseData;

        this.isLoadedCitiesList = true;

        // console.log(this.item_types);

      },
      error: err => {

        this.isLoadedCitiesList = true;

        this.redirectOnSessionError(err);

      },
      complete: () =>{

        this.isLoadedCitiesList = true;

      }

    });
  }


  ngOnInit(){

    this.account_Form.valueChanges.subscribe(() => {
      this.checkFormValidity();
    });
    this.getAllCities();

    this.account_service.get_company_data().subscribe({

      next: response => {

        //console.log(response.responseData+"\n");
        //console.log(this.editing_client+"\n");


        this.editing_account_info = response.responseData;

        //console.log(this.editing_client+"\n");
        this.mustFill = false;
        this.isEmpty = false;
        this.isLoadedAccountInfo = true;

      },
      error: err => {

        this.isLoadedAccountInfo = true;
        
        if(parseInt(err.error.code) === 19){
          this.mustFill = true;
          this.isEmpty = true;
          this.isEditing = true; 
        }
        
        this.redirectOnSessionError(err);

      },
      complete: () =>{

        this.isEmpty = false;
        this.mustFill = false;

        const fc = this.account_Form.controls;
        const e_client = this.editing_account_info;

        fc.inputName.setValue(e_client.name);
        fc.inputSurname.setValue(e_client.surname);
        fc.inputCompanyName.setValue(e_client.company_name);
        fc.inputNIP.setValue(e_client.nip);
        fc.inputPhoneNumber.setValue(e_client.phone_number);
        fc.inputEmail.setValue(e_client.email);
        fc.inputAddress.setValue(e_client.address);
        fc.inputCity.setValue(e_client.city_name);
        fc.inputPostalCode.setValue(e_client.postal_code);
        fc.inputBankName.setValue(e_client.bank_name);
        fc.inputBankNumber.setValue(e_client.bank_number);
        fc.inputWebPage.setValue(e_client.web_page);
        fc.inputMoneySign.setValue(e_client.money_sign);
        fc.inputMoneySignDecimal.setValue(e_client.money_sign_decimal);

        this.fileType = e_client.file_type ? 'image/'+e_client.file_type.replace('.', '') : undefined;
        this.fileUploaded = e_client.company_logo;

        if(e_client.company_logo !== null){
          this.fetchAndDisplayLogo(e_client.company_logo, this.fileType);
        }

        fc.inputName.disable();
        fc.inputSurname.disable();
        fc.inputCompanyName.disable();
        fc.inputNIP.disable();
        fc.inputPhoneNumber.disable();
        fc.inputEmail.disable();
        fc.inputAddress.disable();
        fc.inputCity.disable();
        fc.inputPostalCode.disable();
        fc.inputBankName.disable();
        fc.inputBankNumber.disable();
        fc.inputWebPage.disable();
        fc.inputMoneySign.disable();
        fc.inputMoneySignDecimal.disable();

        this.isLoadedAccountInfo = true;

      }
      
    })

  }


  fetchAndDisplayLogo(logo: string, type: string | undefined): void {
    this.imageUrl = 'data:' + type + ';base64,' + logo;
  }

  checkFormValidity(): void {
    this.emptyError = this.account_Form.get("inputCity")?.value !== null && Object.keys(this.account_Form.controls).every(controlName => {
      return !this.account_Form.get(controlName)?.hasError('required') || this.account_Form.get(controlName)?.value !== '';
    });
  }

  add_account_info(){

    this.submitted = true;
    this.checkFormValidity();


    if (this.account_Form.invalid) return;

    this.isLoadedAccountInfo = false;

    const fc = this.account_Form.controls;

    const Surname = fc.inputSurname.value;
    const Name = fc.inputName.value;
    const CompanyName = fc.inputCompanyName.value;
    const NIP = fc.inputNIP.value;
    const PhoneNumber = fc.inputPhoneNumber.value;
    const Email = fc.inputEmail.value;
    const Address = fc.inputAddress.value;
    const City = fc.inputCity.value;
    const PostalCode = fc.inputPostalCode.value;
    const BankName = fc.inputBankName.value;
    const BankNumber = fc.inputBankNumber.value;
    const WebPage = fc.inputWebPage.value;
    const MoneySign = fc.inputMoneySign.value;
    const MoneySignDecimal = fc.inputMoneySignDecimal.value;

    const add_account_info_input:Add_Company_Data = {
      surname:Surname,
      name:Name,
      company_name:CompanyName,
      nip:NIP,
      phone_number:PhoneNumber,
      email:Email,
      address:Address,
      city_id_FK:Number(this.cities_list.find(function(type){ //Number powinien chyba być
        return type.city == City;
      })!.id),
      postal_code:PostalCode,
      bank_name:BankName,
      bank_number:BankNumber,
      web_page:WebPage,
      money_sign:MoneySign,
      money_sign_decimal:MoneySignDecimal,
      company_logo: this.fileUploaded,
      file_type: this.fileType ? '.'+this.fileType!.split('/')[1] : undefined
    }

    this.account_service.add_account_info(add_account_info_input).subscribe({

      error: () => {

        this.isLoadedAccountInfo = true;

      },
      complete: () => {

        this.isLoadedAccountInfo = true;
        this.mustFill = false;
        this.isEmpty = false;
        this.isEditing = false;

        fc.inputName.disable();
        fc.inputSurname.disable();
        fc.inputCompanyName.disable();
        fc.inputNIP.disable();
        fc.inputPhoneNumber.disable();
        fc.inputEmail.disable();
        fc.inputAddress.disable();
        fc.inputCity.disable();
        fc.inputPostalCode.disable();
        fc.inputBankName.disable();
        fc.inputBankNumber.disable();
        fc.inputWebPage.disable();
        fc.inputMoneySign.disable();
        fc.inputMoneySignDecimal.disable();

      }

    })
    this.display_file_size_error = false;
  }
  
  change_button(){
    this.isEditing = true;

    const fc = this.account_Form.controls;
    fc.inputName.enable();
    fc.inputSurname.enable();
    fc.inputCompanyName.enable();
    fc.inputNIP.enable();
    fc.inputPhoneNumber.enable();
    fc.inputEmail.enable();
    fc.inputAddress.enable();
    fc.inputCity.enable();
    fc.inputPostalCode.enable();
    fc.inputBankName.enable();
    fc.inputBankNumber.enable();
    fc.inputWebPage.enable();
    fc.inputMoneySign.enable();
    fc.inputMoneySignDecimal.enable();

  }

  edit_account_info(){

    this.submitted = true;
    this.checkFormValidity();


    if (this.account_Form.invalid) return;

    this.isLoadedAccountInfo = false;

    const fc = this.account_Form.controls;

    const Surname = fc.inputSurname.value;
    const Name = fc.inputName.value;
    const CompanyName = fc.inputCompanyName.value;
    const NIP = fc.inputNIP.value;
    const PhoneNumber = fc.inputPhoneNumber.value;
    const Email = fc.inputEmail.value;
    const Address = fc.inputAddress.value;
    const City = fc.inputCity.value;
    const PostalCode = fc.inputPostalCode.value;
    const BankName = fc.inputBankName.value;
    const BankNumber = fc.inputBankNumber.value;
    const WebPage = fc.inputWebPage.value;
    const MoneySign = fc.inputMoneySign.value;
    const MoneySignDecimal = fc.inputMoneySignDecimal.value;

    const edit_account_info_input:Edit_Company_Data = {
      surname:Surname,
      name:Name,
      company_name:CompanyName,
      nip:NIP,
      phone_number:PhoneNumber,
      email:Email,
      address:Address,
      city_id_FK:Number(this.cities_list.find(function(type){ //Number powinien chyba być
        return type.city == City;
      })!.id),
      postal_code:PostalCode,
      bank_name:BankName,
      bank_number:BankNumber,
      web_page:WebPage,
      money_sign:MoneySign,
      money_sign_decimal:MoneySignDecimal,
      company_logo:this.fileUploaded,
      file_type: this.fileType ? '.'+this.fileType!.split('/')[1] : undefined
    }

    this.account_service.edit_account_info(edit_account_info_input).subscribe({

      error: () => {

        this.isLoadedAccountInfo = true;

      },
      complete: () => {

        this.isLoadedAccountInfo = true;
        this.mustFill = false;
        this.isEmpty = false;
        this.isEditing = false;

        fc.inputName.disable();
        fc.inputSurname.disable();
        fc.inputCompanyName.disable();
        fc.inputNIP.disable();
        fc.inputPhoneNumber.disable();
        fc.inputEmail.disable();
        fc.inputAddress.disable();
        fc.inputCity.disable();
        fc.inputPostalCode.disable();
        fc.inputBankName.disable();
        fc.inputBankNumber.disable();
        fc.inputWebPage.disable();
        fc.inputMoneySign.disable();
        fc.inputMoneySignDecimal.disable();

      }

    })
    this.display_file_size_error = false;
  }

  openAddDialog(enterAnimationDuration: string, exitAnimationDuration: string): void {
    const dialogRef = this.dialog.open(EditDialogComponent, {
      data: {title: this.translate.instant('addDialog.cityTitle'), label:  this.translate.instant('addDialog.cityLabel')},
      width: '300px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      this.isLoadedCitiesList = true;

      if(result.success){

        this.account_service.add_city({city: result.value}).subscribe(
          {
            error: err => {
    
              if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10){
    
                this.router.navigateByUrl('/login');
    
              }
    
            },
            complete: () =>{


              this.getAllCities();

            }
          }
        );


      }


    });
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
  }
  
  onDrop(event: DragEvent) {
    event.preventDefault();
    if (event.dataTransfer?.files[0] !== undefined) {
      const file = event.dataTransfer?.files[0];
      let passedThrough = false;
      this.supportedFileTypes.forEach((element) => {
        passedThrough = file.type === element ? true : passedThrough;
      });
      if (passedThrough) {
        this.readFile(file);
      }else this.wrongType = true;
    }
  }

  removeImage() {
    this.imageUrl = undefined;
    this.fileType = undefined;
    this.wrongType = false;
  }
  
  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    let passedThrough = false;
    this.supportedFileTypes.forEach(element => {
      passedThrough = file.type === element? true : passedThrough;
    });
    if (passedThrough) {
      this.readFile(file);
    }else this.wrongType = true;
  }
  
  readFile(file: File) {

    if(file.size > this.max_file_size_MB*1024*1024){
      
      this.display_file_size_error = true;
      
    }else{

      this.display_file_size_error = false;

      const reader = new FileReader();

      reader.onload = (e: any) => {
          this.imageUrl = e.target.result;
      };

      const readerArrayBuffer = new FileReader();

      readerArrayBuffer.onload = async (event: any) => {

        const blob = new Blob([file], { type: file.type });
        this.fileType = blob.type;

        console.log('Blob:', blob, blob.type);
        try {

          const base64String = await this.convertBlobToBase64(blob);

          console.log('Base64:', base64String);

          this.fileUploaded = base64String;

        } catch (error) {

          console.error('Błąd konwersji Blob na Base64:', error);

        }

      };

      reader.readAsDataURL(file);
      readerArrayBuffer.readAsArrayBuffer(file);

    }

}

convertBlobToBase64(blob: Blob): Promise<string> {
  return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => {
          const base64String = reader.result as string;
          resolve(base64String.split(',')[1]);
      };
      reader.onerror = () => {
          reject('Błąd odczytu Blob jako Base64.');
      };
      reader.readAsDataURL(blob);
  });
}


  filterCities(): void {
    const filterValue = this.inputCity.nativeElement.value.toLowerCase();
    this.cities_list_filtered = this.cities_list.filter(o => o.city.toLowerCase().includes(filterValue));
  }

  logout(){

    this.LoggedOut = false;
    
    this.auth.logout().subscribe(() => this.LoggedOut = true);

  }

}
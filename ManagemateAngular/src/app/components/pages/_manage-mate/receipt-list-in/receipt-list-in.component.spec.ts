import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceiptListInComponent } from './receipt-list-in.component';

describe('ReceiptListInComponent', () => {
  let component: ReceiptListInComponent;
  let fixture: ComponentFixture<ReceiptListInComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReceiptListInComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ReceiptListInComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

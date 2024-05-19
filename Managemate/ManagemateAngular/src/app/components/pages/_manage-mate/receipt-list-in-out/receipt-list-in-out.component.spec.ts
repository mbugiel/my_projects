import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceiptListOutComponent } from './receipt-list-in-out.component';

describe('ReceiptListOutComponent', () => {
  let component: ReceiptListOutComponent;
  let fixture: ComponentFixture<ReceiptListOutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReceiptListOutComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ReceiptListOutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

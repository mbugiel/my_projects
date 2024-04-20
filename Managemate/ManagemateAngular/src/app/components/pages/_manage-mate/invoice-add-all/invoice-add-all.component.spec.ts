import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceAddAllComponent } from './invoice-add-all.component';

describe('InvoiceAddAllComponent', () => {
  let component: InvoiceAddAllComponent;
  let fixture: ComponentFixture<InvoiceAddAllComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvoiceAddAllComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InvoiceAddAllComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

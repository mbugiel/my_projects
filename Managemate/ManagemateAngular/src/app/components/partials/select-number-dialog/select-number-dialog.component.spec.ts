import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectNumberDialogComponent } from './select-number-dialog.component';

describe('SelectNumberDialogComponent', () => {
  let component: SelectNumberDialogComponent;
  let fixture: ComponentFixture<SelectNumberDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SelectNumberDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SelectNumberDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

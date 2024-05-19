import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAuthorizedWorkerDialogComponent } from './add-authorized-worker-dialog.component';

describe('AddAuthorizedWorderDialogComponent', () => {
  let component: AddAuthorizedWorkerDialogComponent;
  let fixture: ComponentFixture<AddAuthorizedWorkerDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddAuthorizedWorkerDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddAuthorizedWorkerDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

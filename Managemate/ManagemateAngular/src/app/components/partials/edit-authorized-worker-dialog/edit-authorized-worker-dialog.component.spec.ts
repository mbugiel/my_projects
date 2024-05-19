import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAuthorizedWorkerDialogComponent } from './edit-authorized-worker-dialog.component';

describe('EditAuthorizedWorkerDialogComponent', () => {
  let component: EditAuthorizedWorkerDialogComponent;
  let fixture: ComponentFixture<EditAuthorizedWorkerDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditAuthorizedWorkerDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EditAuthorizedWorkerDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

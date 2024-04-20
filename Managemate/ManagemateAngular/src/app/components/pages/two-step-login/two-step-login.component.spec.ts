import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TwoStepLoginComponent } from './two-step-login.component';

describe('TwoStepLoginComponent', () => {
  let component: TwoStepLoginComponent;
  let fixture: ComponentFixture<TwoStepLoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TwoStepLoginComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TwoStepLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConSiteEditComponent } from './con-site-edit.component';

describe('ConSiteEditComponent', () => {
  let component: ConSiteEditComponent;
  let fixture: ComponentFixture<ConSiteEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConSiteEditComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConSiteEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

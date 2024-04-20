import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConSiteAddComponent } from './con-site-add.component';

describe('ConSiteAddComponent', () => {
  let component: ConSiteAddComponent;
  let fixture: ComponentFixture<ConSiteAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConSiteAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConSiteAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

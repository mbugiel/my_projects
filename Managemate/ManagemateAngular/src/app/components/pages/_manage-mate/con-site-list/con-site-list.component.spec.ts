import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConSiteListComponent } from './con-site-list.component';

describe('ConSiteListComponent', () => {
  let component: ConSiteListComponent;
  let fixture: ComponentFixture<ConSiteListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConSiteListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConSiteListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

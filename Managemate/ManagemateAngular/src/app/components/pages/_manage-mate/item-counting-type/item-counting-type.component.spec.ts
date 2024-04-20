import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemCountingTypeComponent } from './item-counting-type.component';

describe('ItemCountingTypeComponent', () => {
  let component: ItemCountingTypeComponent;
  let fixture: ComponentFixture<ItemCountingTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ItemCountingTypeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ItemCountingTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

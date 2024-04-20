import { TestBed } from '@angular/core/testing';

import { ConSiteService } from './con-site.service';

describe('ConSiteService', () => {
  let service: ConSiteService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConSiteService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

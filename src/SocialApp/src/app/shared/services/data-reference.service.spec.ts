import { TestBed } from '@angular/core/testing';

import { DataReferenceService } from './data-reference.service';

describe('DataReferenceService', () => {
  let service: DataReferenceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataReferenceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { NewsfeedService } from './newsfeed';

describe('Newsfeed', () => {
  let service: NewsfeedService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewsfeedService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { UserPicturesService } from './user-pictures';

describe('UserPictures', () => {
  let service: UserPicturesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserPicturesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

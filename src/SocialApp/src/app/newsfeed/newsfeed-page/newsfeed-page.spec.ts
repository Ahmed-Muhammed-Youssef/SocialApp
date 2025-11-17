import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsfeedPage } from './newsfeed-page';

describe('NewsfeedPage', () => {
  let component: NewsfeedPage;
  let fixture: ComponentFixture<NewsfeedPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewsfeedPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewsfeedPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

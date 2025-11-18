import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GlobalLoader } from './global-loader';

describe('GlobalLoader', () => {
  let component: GlobalLoader;
  let fixture: ComponentFixture<GlobalLoader>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GlobalLoader]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GlobalLoader);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

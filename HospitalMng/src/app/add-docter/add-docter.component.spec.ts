import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDocterComponent } from './add-docter.component';

describe('AddDocterComponent', () => {
  let component: AddDocterComponent;
  let fixture: ComponentFixture<AddDocterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddDocterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDocterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

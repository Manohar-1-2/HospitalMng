import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocterDetailsComponent } from './docter-details.component';

describe('DocterDetailsComponent', () => {
  let component: DocterDetailsComponent;
  let fixture: ComponentFixture<DocterDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DocterDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DocterDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

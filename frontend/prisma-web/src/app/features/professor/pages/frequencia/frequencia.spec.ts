import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Frequencia } from './frequencia';

describe('Frequencia', () => {
  let component: Frequencia;
  let fixture: ComponentFixture<Frequencia>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Frequencia],
    }).compileComponents();

    fixture = TestBed.createComponent(Frequencia);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GabaritoLista } from './gabarito-lista';

describe('GabaritoLista', () => {
  let component: GabaritoLista;
  let fixture: ComponentFixture<GabaritoLista>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GabaritoLista],
    }).compileComponents();

    fixture = TestBed.createComponent(GabaritoLista);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CriarLista } from './criar-lista';

describe('CriarLista', () => {
  let component: CriarLista;
  let fixture: ComponentFixture<CriarLista>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CriarLista],
    }).compileComponents();

    fixture = TestBed.createComponent(CriarLista);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

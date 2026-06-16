import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeusExercicios } from './meus-exercicios';

describe('MeusExercicios', () => {
  let component: MeusExercicios;
  let fixture: ComponentFixture<MeusExercicios>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MeusExercicios],
    }).compileComponents();

    fixture = TestBed.createComponent(MeusExercicios);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

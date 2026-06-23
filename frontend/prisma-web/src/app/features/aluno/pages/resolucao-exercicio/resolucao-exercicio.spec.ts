import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResolucaoExercicio } from './resolucao-exercicio';

describe('ResolucaoExercicio', () => {
  let component: ResolucaoExercicio;
  let fixture: ComponentFixture<ResolucaoExercicio>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResolucaoExercicio],
    }).compileComponents();

    fixture = TestBed.createComponent(ResolucaoExercicio);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

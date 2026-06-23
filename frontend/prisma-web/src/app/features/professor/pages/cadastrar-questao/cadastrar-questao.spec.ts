import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastrarQuestao } from './cadastrar-questao';

describe('CadastrarQuestao', () => {
  let component: CadastrarQuestao;
  let fixture: ComponentFixture<CadastrarQuestao>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CadastrarQuestao],
    }).compileComponents();

    fixture = TestBed.createComponent(CadastrarQuestao);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BancoQuestoesComponent } from './banco-questoes';

describe('BancoQuestoesComponent', () => {
  let component: BancoQuestoesComponent;
  let fixture: ComponentFixture<BancoQuestoesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BancoQuestoesComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(BancoQuestoesComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
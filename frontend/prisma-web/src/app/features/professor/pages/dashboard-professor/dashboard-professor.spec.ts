import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { provideCharts, withDefaultRegisterables } from 'ng2-charts';

import { DashboardProfessorComponent } from './dashboard-professor';
import { ProfessorService } from '../../services/professor.service';

describe('DashboardProfessor', () => {
  let component: DashboardProfessorComponent;
  let fixture: ComponentFixture<DashboardProfessorComponent>;
  let mockProfessorService: any;

  beforeEach(async () => {
    // Mock puro usando função do TypeScript sem depender do Jasmine
    mockProfessorService = {
      getEstatisticasDashboard: () => of({
        totalAlunos: 100,
        questoesCadastradas: 50,
        desempenhoMedio: 80,
        listasAtivas: 10,
        linhaLabels: ['S1'],
        linhaSuaTurma: [10],
        linhaMediaGeral: [10],
        roscaLabels: ['T1'],
        roscaDados: [10]
      })
    };

    await TestBed.configureTestingModule({
      imports: [DashboardProfessorComponent],
      providers: [
        { provide: ProfessorService, useValue: mockProfessorService },
        provideCharts(withDefaultRegisterables())
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DashboardProfessorComponent);
    component = fixture.componentInstance;
    
    fixture.detectChanges(); 
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutAluno } from './layout-aluno';

describe('LayoutAluno', () => {
  let component: LayoutAluno;
  let fixture: ComponentFixture<LayoutAluno>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LayoutAluno],
    }).compileComponents();

    fixture = TestBed.createComponent(LayoutAluno);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

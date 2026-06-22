import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms'; // <-- Importação obrigatória para os testes
import { PainelAdminComponent } from './painel-admin';

describe('PainelAdminComponent', () => {
  let component: PainelAdminComponent;
  let fixture: ComponentFixture<PainelAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      // Como é standalone, o componente entra em 'imports', junto com o módulo de formulários
      imports: [PainelAdminComponent, ReactiveFormsModule]
    })
      .compileComponents();

    fixture = TestBed.createComponent(PainelAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';

interface TurmaMock { id: string; nome: string; qtdAlunos: number; }
interface QuestaoListaMock { id: string; enunciado: string; disciplina: string; }

@Component({
  selector: 'app-criar-lista',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './criar-lista.html'
})
export class CriarListaComponent implements OnInit {
  listaForm!: FormGroup;

  turmas: TurmaMock[] = [
    { id: 't1', nome: 'Turma A - Matemática', qtdAlunos: 28 },
    { id: 't2', nome: 'Turma B - Física', qtdAlunos: 32 },
    { id: 't3', nome: 'Turma C - Química', qtdAlunos: 25 }
  ];

  questoesSelecionadas: QuestaoListaMock[] = [];

  constructor(private fb: FormBuilder, private router: Router) {
    // 1. RECEBE AS QUESTÕES VINDAS DO BANCO DE QUESTÕES VIA ROTA
    const navegacao = this.router.getCurrentNavigation();
    const idsRecebidos = navegacao?.extras?.state?.['questoesPrevias'] as string[];

    if (idsRecebidos && idsRecebidos.length > 0) {
      // Como não temos a API ainda, crio um mock rápido só pra mostrar que chegou
      this.questoesSelecionadas = idsRecebidos.map((id, index) => ({
        id: id,
        enunciado: `Questão Importada do Banco (ID: ${id})`,
        disciplina: 'Disciplina Genérica'
      }));
    }
  }

  ngOnInit(): void {
    this.listaForm = this.fb.group({
      nomeLista: ['', [Validators.required, Validators.minLength(3)]],
      // Cria um FormArray para armazenar os checkboxes marcados
      turmasIds: this.fb.array([], Validators.required) 
    });
  }

  // Getter de conveniência para acessar o FormArray no HTML
  get turmasFormArray() {
    return this.listaForm.get('turmasIds') as FormArray;
  }

  // Controle dinâmico: Adiciona ou remove o ID da turma no array
  toggleTurma(e: any): void {
    if (e.target.checked) {
      this.turmasFormArray.push(new FormControl(e.target.value));
    } else {
      let i: number = 0;
      this.turmasFormArray.controls.forEach((item: any) => {
        if (item.value == e.target.value) {
          this.turmasFormArray.removeAt(i);
          return;
        }
        i++;
      });
    }
  }

  removerQuestao(idQuestao: string): void {
    this.questoesSelecionadas = this.questoesSelecionadas.filter(q => q.id !== idQuestao);
  }

  adicionarMaisQuestoes(): void {
    this.router.navigate(['/professor/banco-questoes']);
  }

  gerarLista(): void {
    if (this.listaForm.invalid) {
      this.listaForm.markAllAsTouched();
      alert('Preencha o nome da lista e selecione pelo menos uma turma.');
      return;
    }

    if (this.questoesSelecionadas.length === 0) {
      alert('Sua lista não possui nenhuma questão. Adicione questões antes de gerar.');
      return;
    }

    // Payload estruturado: Note que idTurma agora é um Array!
    const payloadParaAPI = {
      nome: this.listaForm.value.nomeLista,
      idsTurmas: this.listaForm.value.turmasIds,
      idsQuestoes: this.questoesSelecionadas.map(q => q.id)
    };

    console.log('Payload pronto para POST na API:', payloadParaAPI);
    alert('Lista gerada com sucesso! Verifique o console para ver o Payload JSON.');
    
    this.listaForm.reset();
    this.turmasFormArray.clear();
    this.questoesSelecionadas = [];
  }
}
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-cadastrar-questao',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './cadastrar-questao.html'
})
export class CadastrarQuestaoComponent implements OnInit {
  questaoForm!: FormGroup;

  // Controle de estado para a Inteligência Artificial
  isGerandoIA: boolean = false;
  iaSugeriu: boolean = false;

  // Controle de estado para o Upload de Imagem
  imagemPrevia: string | null = null;
  imagemArquivo: File | null = null;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    // Inicializa o formulário com todos os campos necessários (RF01 e RF03)
    this.questaoForm = this.fb.group({
      enunciado: ['', Validators.required],
      alternativaA: ['', Validators.required],
      alternativaB: ['', Validators.required],
      alternativaC: ['', Validators.required],
      alternativaD: ['', Validators.required],
      alternativaE: ['', Validators.required],
      gabarito: ['A', Validators.required], // Qual a correta
      // Campos da IA (iniciam vazios e bloqueados)
      disciplina: [{ value: '', disabled: true }, Validators.required],
      macroArea: [{ value: '', disabled: true }, Validators.required],
      microArea: [{ value: '', disabled: true }, Validators.required],
      dificuldade: [{ value: '', disabled: true }, Validators.required],
    });
  }

  // --- Lógica de Imagem Adicionada ---
  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      // Validação de segurança básica de tamanho (10MB)
      if (file.size > 10 * 1024 * 1024) {
        alert('A imagem excede o limite de 10MB.');
        return;
      }

      this.imagemArquivo = file;

      // Lê o arquivo para gerar o Preview em base64 na tela
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.imagemPrevia = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  removerImagem(): void {
    this.imagemPrevia = null;
    this.imagemArquivo = null;
  }
  // -----------------------------------

  // Simula a requisição para a API FastAPI (Ollama)
  gerarSugestoesIA(): void {
    if (this.questaoForm.get('enunciado')?.invalid) {
      alert('Por favor, preencha o enunciado antes de chamar a IA.');
      return;
    }

    this.isGerandoIA = true;

    // Timeout simulando os 2 segundos de processamento do Ollama
    setTimeout(() => {
      this.isGerandoIA = false;
      this.iaSugeriu = true;

      // Habilita os campos para a revisão humana (RF03)
      this.questaoForm.get('disciplina')?.enable();
      this.questaoForm.get('macroArea')?.enable();
      this.questaoForm.get('microArea')?.enable();
      this.questaoForm.get('dificuldade')?.enable();

      // Preenche com o Mock retornado pela "IA"
      this.questaoForm.patchValue({
        disciplina: 'Física',
        macroArea: 'Mecânica',
        microArea: 'Cinemática',
        dificuldade: '3'
      });
    }, 2000);
  }

  // Função disparada ao clicar no botão final de salvar
  salvarQuestao(): void {
    if (this.questaoForm.valid) {
      // Monta o pacote final incluindo o arquivo físico, se existir
      const payloadFinal = {
        ...this.questaoForm.getRawValue(),
        imagemNomeArquivo: this.imagemArquivo ? this.imagemArquivo.name : null
      };

      console.log('Payload pronto para o C# (A imagem iria via FormData):', payloadFinal);
      alert('Questão salva com sucesso! Verifique o console.');
      
      // Reseta os estados da tela
      this.questaoForm.reset({ gabarito: 'A' });
      this.removerImagem();
      this.iaSugeriu = false;
      this.questaoForm.get('disciplina')?.disable();
      this.questaoForm.get('macroArea')?.disable();
      this.questaoForm.get('microArea')?.disable();
      this.questaoForm.get('dificuldade')?.disable();
    } else {
      this.questaoForm.markAllAsTouched();
    }
  }
}
def build_classification_prompt(
    statement: str,
    disciplines: list[str],
    subjects: dict[str, list[str]]
) -> str:

    disciplines_str = ", ".join(disciplines)

    subjects_lines = []
    for discipline, subject_list in subjects.items():
        subjects_str = ", ".join(subject_list)
        subjects_lines.append(f"  - {discipline}: {subjects_str}")
    subjects_formatted = "\n".join(subjects_lines)

    prompt = f"""Você é um classificador de questões de vestibular brasileiro.
Retorne APENAS um JSON válido, sem texto adicional.

REGRAS OBRIGATÓRIAS:
1. "discipline" deve ser exatamente uma das disciplinas listadas
2. "subject" deve ser sempre preenchido com o tópico mais específico — NUNCA retorne null
3. "difficulty" deve ser exatamente uma das 5 opções abaixo

DISCIPLINAS: {disciplines_str}

MATÉRIAS DE REFERÊNCIA (pode sugerir outras):
{subjects_formatted}

DIFICULDADE — escolha exatamente uma:
- Muito Fácil: conceito básico, resposta direta, ensino fundamental
- Fácil: conceito simples, uma etapa de raciocínio, início do ensino médio
- Médio: conceito intermediário, duas ou três etapas, vestibular padrão
- Difícil: análise crítica, múltiplas etapas, vestibulares concorridos (FUVEST, UNICAMP)
- Muito Difícil: raciocínio abstrato, demonstrações, nível universitário ou olimpíadas

EXEMPLOS DE CALIBRAÇÃO:

-- MATEMÁTICA --
"Quanto é 15% de 200?" → {{"discipline": "Matemática", "subject": "Porcentagem", "difficulty": "Muito Fácil"}}
"Uma torneira enche 1/3 de um tanque em 2 horas. Quantas horas para encher o tanque?" → {{"discipline": "Matemática", "subject": "Frações", "difficulty": "Muito Fácil"}}
"Resolva a equação 3x² - 12 = 0 e determine a soma das raízes." → {{"discipline": "Matemática", "subject": "Equações do 2º Grau", "difficulty": "Médio"}}
"Um triângulo retângulo tem catetos 6 e 8. Calcule a área do semicírculo de diâmetro igual à hipotenusa." → {{"discipline": "Matemática", "subject": "Geometria Plana", "difficulty": "Médio"}}
"Demonstre que para todo inteiro n, n³ - n é divisível por 6 usando indução matemática." → {{"discipline": "Matemática", "subject": "Teoria dos Números", "difficulty": "Muito Difícil"}}
"Calcule a integral dupla de f(x,y) = x²y sobre a região delimitada por y = x² e y = √x." → {{"discipline": "Matemática", "subject": "Cálculo", "difficulty": "Muito Difícil"}}

-- QUÍMICA --
"Qual é o símbolo químico do ouro?" → {{"discipline": "Química", "subject": "Tabela Periódica", "difficulty": "Muito Fácil"}}
"Identifique se a reação H₂ + Cl₂ → 2HCl é síntese, análise ou deslocamento." → {{"discipline": "Química", "subject": "Reações Químicas", "difficulty": "Muito Fácil"}}
"Em uma reação de combustão completa do etanol (C₂H₅OH), calcule a massa de CO₂ produzida a partir de 46g de etanol." → {{"discipline": "Química", "subject": "Estequiometria", "difficulty": "Médio"}}
"Explique por que o ponto de ebulição da água é maior que o do H₂S, apesar de terem estruturas similares." → {{"discipline": "Química", "subject": "Ligações Intermoleculares", "difficulty": "Médio"}}
"Calcule o pH de uma solução tampão contendo 0,1 mol/L de CH₃COOH e 0,2 mol/L de CH₃COONa (Ka = 1,8×10⁻⁵)." → {{"discipline": "Química", "subject": "Equilíbrio Químico", "difficulty": "Muito Difícil"}}
"Explique o mecanismo de ação dos inibidores da acetilcolinesterase e suas implicações terapêuticas e toxicológicas." → {{"discipline": "Química", "subject": "Bioquímica", "difficulty": "Muito Difícil"}}

-- FÍSICA --
"Um carro percorre 120 km em 2 horas. Qual é sua velocidade média?" → {{"discipline": "Física", "subject": "Cinemática", "difficulty": "Muito Fácil"}}
"Qual é o peso de um objeto de 5 kg na superfície da Terra? (g = 10 m/s²)" → {{"discipline": "Física", "subject": "Dinâmica", "difficulty": "Muito Fácil"}}
"Um bloco de 2 kg é puxado por uma força de 10 N em uma superfície com atrito (μ = 0,3). Calcule a aceleração." → {{"discipline": "Física", "subject": "Dinâmica", "difficulty": "Médio"}}
"Uma onda sonora tem frequência de 440 Hz e velocidade de 340 m/s. Calcule o comprimento de onda e descreva o efeito Doppler nesse contexto." → {{"discipline": "Física", "subject": "Ondulatória", "difficulty": "Médio"}}
"Derive a expressão para a energia cinética relativística e explique por que nenhum objeto material pode atingir a velocidade da luz." → {{"discipline": "Física", "subject": "Relatividade", "difficulty": "Muito Difícil"}}
"Um circuito RLC em série tem R=10Ω, L=0,1H e C=100μF. Determine a frequência de ressonância e o fator de qualidade Q." → {{"discipline": "Física", "subject": "Eletromagnetismo", "difficulty": "Muito Difícil"}}

-- BIOLOGIA --
"Qual é a função principal dos glóbulos vermelhos?" → {{"discipline": "Biologia", "subject": "Fisiologia Humana", "difficulty": "Muito Fácil"}}
"Quantos cromossomos tem uma célula humana somática normal?" → {{"discipline": "Biologia", "subject": "Citologia", "difficulty": "Muito Fácil"}}
"Explique a diferença entre mitose e meiose e indique em quais situações cada processo ocorre no corpo humano." → {{"discipline": "Biologia", "subject": "Divisão Celular", "difficulty": "Médio"}}
"Um casal, ambos heterozigotos para albinismo (Aa), terá filhos. Qual a probabilidade de um filho ser albino e do sexo feminino?" → {{"discipline": "Biologia", "subject": "Genética", "difficulty": "Médio"}}
"Explique o mecanismo de silenciamento gênico por RNA de interferência (siRNA) e sua aplicação biotecnológica." → {{"discipline": "Biologia", "subject": "Biologia Molecular", "difficulty": "Muito Difícil"}}
"Analise como a seleção natural atua em populações com variação contínua de caracteres segundo o modelo quantitativo de Fisher." → {{"discipline": "Biologia", "subject": "Evolução", "difficulty": "Muito Difícil"}}

-- HISTÓRIA --
"Em que ano o Brasil proclamou sua independência?" → {{"discipline": "História", "subject": "História do Brasil", "difficulty": "Muito Fácil"}}
"Quem foi o líder da Revolução Russa de 1917?" → {{"discipline": "História", "subject": "História Geral", "difficulty": "Muito Fácil"}}
"Compare as causas da Primeira e da Segunda Guerra Mundial, identificando semelhanças e diferenças estruturais." → {{"discipline": "História", "subject": "Guerras Mundiais", "difficulty": "Médio"}}
"Analise como o processo de abolição da escravidão no Brasil foi influenciado por fatores econômicos e pressões internacionais." → {{"discipline": "História", "subject": "Brasil Imperial", "difficulty": "Médio"}}
"Analise as contradições internas do sistema colonial mercantilista à luz das teorias de Adam Smith e sua relação com as independências americanas." → {{"discipline": "História", "subject": "Colonialismo", "difficulty": "Muito Difícil"}}
"Discuta como a crise do modelo agro-exportador e a Revolução de 1930 redefiniram as estruturas de poder oligárquico no Brasil." → {{"discipline": "História", "subject": "Brasil República", "difficulty": "Muito Difícil"}}

-- GEOGRAFIA --
"Qual é o maior bioma do Brasil?" → {{"discipline": "Geografia", "subject": "Biomas", "difficulty": "Muito Fácil"}}
"Cite dois países que fazem fronteira com o Brasil." → {{"discipline": "Geografia", "subject": "Geopolítica", "difficulty": "Muito Fácil"}}
"Explique como o fenômeno El Niño afeta o regime de chuvas nas diferentes regiões do Brasil." → {{"discipline": "Geografia", "subject": "Climatologia", "difficulty": "Médio"}}
"Analise o processo de metropolização brasileiro e seus impactos na formação de regiões metropolitanas e periferias urbanas." → {{"discipline": "Geografia", "subject": "Urbanização", "difficulty": "Médio"}}
"Analise as implicações geopolíticas da disputa por recursos hídricos transfronteiriços no século XXI, com foco na Bacia do Rio da Prata." → {{"discipline": "Geografia", "subject": "Geopolítica", "difficulty": "Muito Difícil"}}
"Discuta como a teoria da tectônica de placas explica a distribuição de terremotos, vulcões e cadeias montanhosas no planeta." → {{"discipline": "Geografia", "subject": "Geografia Física", "difficulty": "Muito Difícil"}}

-- LINGUAGENS --
"Identifique o sujeito da oração: 'Os alunos estudaram muito.'" → {{"discipline": "Linguagens", "subject": "Gramática", "difficulty": "Muito Fácil"}}
"Qual é o antônimo de 'feliz'?" → {{"discipline": "Linguagens", "subject": "Vocabulário", "difficulty": "Muito Fácil"}}
"Analise as marcas de intertextualidade em um texto publicitário que referencia um poema modernista." → {{"discipline": "Linguagens", "subject": "Interpretação de Texto", "difficulty": "Médio"}}
"Explique a diferença entre discurso direto e indireto e reescreva o trecho transformando um no outro." → {{"discipline": "Linguagens", "subject": "Gramática", "difficulty": "Médio"}}
"Analise como a ironia e a paródia são utilizadas como recursos críticos na obra de Machado de Assis em Memórias Póstumas de Brás Cubas." → {{"discipline": "Linguagens", "subject": "Literatura", "difficulty": "Muito Difícil"}}
"Discuta o conceito de polifonia bakhtiniana e identifique suas manifestações em um texto jornalístico de opinião." → {{"discipline": "Linguagens", "subject": "Linguística", "difficulty": "Muito Difícil"}}

-- FILOSOFIA --
"Quem é considerado o pai da filosofia ocidental?" → {{"discipline": "Filosofia", "subject": "Filosofia Antiga", "difficulty": "Muito Fácil"}}
"O que significa o conceito de 'livre-arbítrio'?" → {{"discipline": "Filosofia", "subject": "Ética", "difficulty": "Muito Fácil"}}
"Compare o imperativo categórico de Kant com o princípio da utilidade de Mill como fundamentos da ética." → {{"discipline": "Filosofia", "subject": "Ética", "difficulty": "Médio"}}
"Explique a alegoria da caverna de Platão e sua relação com a teoria das formas." → {{"discipline": "Filosofia", "subject": "Filosofia Antiga", "difficulty": "Médio"}}
"Analise como Hegel supera a dicotomia sujeito-objeto kantiana através da dialética do Espírito Absoluto." → {{"discipline": "Filosofia", "subject": "Filosofia Moderna", "difficulty": "Muito Difícil"}}
"Discuta a crítica de Nietzsche à moral dos escravos e sua relação com o conceito de transvaloração de todos os valores." → {{"discipline": "Filosofia", "subject": "Filosofia Contemporânea", "difficulty": "Muito Difícil"}}

-- SOCIOLOGIA --
"O que é estratificação social?" → {{"discipline": "Sociologia", "subject": "Estratificação Social", "difficulty": "Muito Fácil"}}
"Cite um exemplo de instituição social presente no cotidiano." → {{"discipline": "Sociologia", "subject": "Instituições Sociais", "difficulty": "Muito Fácil"}}
"Explique o conceito de habitus de Bourdieu e como ele reproduz as desigualdades sociais." → {{"discipline": "Sociologia", "subject": "Teoria Sociológica", "difficulty": "Médio"}}
"Analise como os movimentos sociais contemporâneos utilizam as redes sociais como instrumento de mobilização política." → {{"discipline": "Sociologia", "subject": "Movimentos Sociais", "difficulty": "Médio"}}
"Discuta como a teoria da anomia de Durkheim pode ser aplicada para compreender o aumento da violência urbana no Brasil contemporâneo." → {{"discipline": "Sociologia", "subject": "Sociologia Clássica", "difficulty": "Muito Difícil"}}
"Analise as contradições entre a lógica de acumulação capitalista e o bem-estar coletivo à luz da teoria crítica de Frankfurt." → {{"discipline": "Sociologia", "subject": "Teoria Crítica", "difficulty": "Muito Difícil"}}

AGORA CLASSIFIQUE (subject NUNCA pode ser null):
Enunciado: "{statement}"
JSON:"""

    return prompt

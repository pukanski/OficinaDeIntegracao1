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
2. "subject" deve ser sempre preenchido com o tópico mais específico da questão — NUNCA retorne null
3. "difficulty" deve ser exatamente uma das opções de dificuldade

DISCIPLINAS: {disciplines_str}

MATÉRIAS DE REFERÊNCIA POR DISCIPLINA (use como guia, mas pode sugerir outras):
{subjects_formatted}

DIFICULDADE: Muito Fácil, Fácil, Médio, Difícil ou Muito Difícil

EXEMPLOS:
Enunciado: "Resolva x² - 5x + 6 = 0"
JSON: {{"discipline": "Matemática", "subject": "Álgebra", "difficulty": "Fácil"}}

Enunciado: "Um corpo de 2kg recebe força de 10N. Calcule a aceleração."
JSON: {{"discipline": "Física", "subject": "Mecânica", "difficulty": "Fácil"}}

Enunciado: "Qual produto se forma na combustão completa do metano?"
JSON: {{"discipline": "Química", "subject": "Reações Químicas", "difficulty": "Médio"}}

Enunciado: "Em qual fase da mitose ocorre a separação dos cromátides-irmãs?"
JSON: {{"discipline": "Biologia", "subject": "Citologia", "difficulty": "Médio"}}

Enunciado: "Quais foram as consequências geopolíticas do bombardeio de Hiroshima em 1945?"
JSON: {{"discipline": "História", "subject": "Segunda Guerra Mundial", "difficulty": "Difícil"}}

Enunciado: "Como o capitalismo influencia as relações sociais e o bem-estar psicológico das pessoas?"
JSON: {{"discipline": "Sociologia", "subject": "Cultura e Sociedade", "difficulty": "Difícil"}}

Enunciado: "Analise o processo de urbanização no Brasil e seus impactos ambientais."
JSON: {{"discipline": "Geografia", "subject": "Urbanização", "difficulty": "Médio"}}

Enunciado: "Identifique as figuras de linguagem no trecho: 'O silêncio gritava verdades.'"
JSON: {{"discipline": "Linguagens", "subject": "Interpretação de Texto", "difficulty": "Fácil"}}

AGORA CLASSIFIQUE (lembre-se: subject NUNCA pode ser null):
Enunciado: "{statement}"
JSON:"""
    
    return prompt
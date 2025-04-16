# 🎮 No Bombs

**Trabalho final para a disciplina de Desenvolvimento de Jogos**  
**Autor:** Ryan Pimentel de Oliveira  
**Curso:** Ciência da Computação - Universidade Federal de Roraima (UFRR)  

## 📌 Sobre o Projeto

**No Bombs** é um jogo arcade desenvolvido em **Unity**, inspirado no clássico mobile **Fruit Ninja**. O objetivo principal é testar os reflexos do jogador em um ambiente dinâmico e desafiador: clique nas bombas pretas para marcar pontos, mas evite as bombas laranjas, que reduzem sua vida.

O jogo foi projetado com uma **interface simples**, **feedback visual com explosões**, **mecânicas de vida** e **progressão de dificuldade** ao longo de **6 fases principais e uma fase final**. Ideal para quem busca partidas rápidas e envolventes em desktop.

## 🧨 Mecânicas

- Clique em bombas pretas para ganhar pontos.
- Evite clicar em bombas laranjas, pois elas retiram 5 de vida.
- A cada intervalo de tempo, o jogador perde 1 ponto de vida.
- Nas últimas fases, um dinamite especial pode aparecer e recuperar 1 ponto de vida se clicado.
- Sistema de fases com aumento progressivo da dificuldade (mais velocidade, mais spawn, mais penalidades por tempo).
- Condição de derrota: vida igual a 0.

## 🎨 Design e Interface

- Estilo visual **minimalista**, com predominância das cores **branco, preto e laranja**.
- Textos legíveis com bordas pretas para destacar informações de vida, pontos e metas por fase.
- Elementos gráficos (bombas, explosões, fundo e UI) criados e editados no **Photoshop**.

## 🛠 Scripts Principais

- `SmashObjectSpawner.cs`: responsável pelo controle das fases, pontuação, condições de vitória e derrota.
- `ClickToSmash.cs`: controla o comportamento das bombas, incluindo movimentação, interação, explosão e pontuação/vida.

## 🧪 Testes e Avaliação

- Testes internos contínuos e feedback informal com amigos e familiares.
- Ajustes realizados na curva de dificuldade e balanceamento do dinamite.
- Estabilidade geral alcançada sem a necessidade de otimizações profundas, dada a simplicidade do projeto.

## 📷 Imagens

(Imagens do jogo podem ser inseridas aqui, como capturas de tela ou gifs curtos)

## ✅ Conclusão

**No Bombs** cumpriu seu papel como trabalho final da disciplina, oferecendo uma experiência divertida e acessível, com desafios crescentes e jogabilidade intuitiva. O projeto demonstra o domínio do Unity e a aplicação prática dos conceitos aprendidos ao longo do curso.

---

> Desenvolvido com dedicação por **Ryan Pimentel de Oliveira**  
> UFRR – Ciência da Computação  

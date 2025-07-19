# Gerador de Testes

![]()

## Introdução

- O projeto Gerador de Testes tem como objetivo facilitar a criação, organização e geração de testes escolares, com suporte a disciplinas, matérias, questões, alternativas e testes com gabarito em PDF.
- Desenvolvido como um projeto acadêmico, a aplicação utiliza C# com .NET 8.0, com interface web em ASP.NET MVC, seguindo uma arquitetura em camadas que separa responsabilidades e melhora a manutenção e escalabilidade.
- A solução permite que instituições ou professores organizem conteúdos pedagógicos, elaborem avaliações com facilidade e imprimam os testes prontos com gabarito.

---

## Tecnologias

<p align="left">
  <img src="https://skillicons.dev/icons?i=cs" height="50"/>
  <img src="https://skillicons.dev/icons?i=dotnet" height="50"/>
  <img src="https://skillicons.dev/icons?i=visualstudio" height="50"/>
  <img src="https://skillicons.dev/icons?i=html" height="50"/>
  <img src="https://skillicons.dev/icons?i=css" height="50"/>
  <img src="https://skillicons.dev/icons?i=js" height="50"/>
  <img src="https://skillicons.dev/icons?i=bootstrap" height="50"/>
  <img src="https://skillicons.dev/icons?i=git" height="50"/>
  <img src="https://skillicons.dev/icons?i=github" height="50"/>
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/postgresql/postgresql-original.svg" height="45"/>
  <img src="https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/render.svg" height="40"/>
</p>

---

## Funcionalidades

- **Disciplina**
  - Cadastrar, editar, excluir, visualizar

- **Matéria**
  - Associar à disciplina
  - Cadastrar, editar, excluir, visualizar

- **Questões**
  - Cadastrar questões objetivas
  - Associar à matéria
  - Visualizar, editar e excluir

- **Alternativas**
  - Inserir múltiplas alternativas por questão
  - Definir qual é a correta

- Testes
  - Criar testes a partir de questões cadastradas
  - Visualizar, duplicar e excluir testes
  - Gerar teste em PDF com gabarito automático

---

## Protótipo

- Para visualizar a interface e a navegação planejada do sistema, acesse o protótipo clicando aqui:  
🔗 [Protótipo do Gerador de Testes (Excalidraw)](https://excalidraw.com/#room=d76af695e7680ddcb7d5,qzV6yhFQNjteglviVjpe3g)

---

## Como utilizar

1. Clone o repositório ou baixe o código fonte.
2. Abra o terminal ou o prompt de comando e navegue até a pasta raiz
3. Utilize o comando abaixo para restaurar as dependências do projeto.

```
dotnet restore
```

4. Em seguida, compile a solução utilizando o comando:
   
```
dotnet build --configuration Release
```

5. Para executar o projeto compilando em tempo real
   
```
dotnet run --project GeradorDeTestes.ConsoleApp
```

6. Para executar o arquivo compilado, navegue até a pasta `./GeradorDeTestes.WebApp/bin/Release/net8.0/` e execute o arquivo:
   
```
GeradorDeTestes.ConsoleApp.exe
```

## Requisitos

- .NET SDK (recomendado .NET 8.0 ou superior) para compilação e execução do projeto.

- Visual Studio 2022 ou superior (opcional, para desenvolvimento).

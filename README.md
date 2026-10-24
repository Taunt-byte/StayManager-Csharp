# 🏨 Aplicativo para Hotéis — C# / Windows Forms

Aplicação desktop desenvolvida em **C# com Windows Forms (WinForms)** para facilitar o cadastro e o gerenciamento de hóspedes, quartos e hospedagens.

O projeto tem como objetivo praticar conceitos de programação orientada a objetos, desenvolvimento de interfaces gráficas, persistência de dados em JSON e implementação de regras de negócio.

## 📸 Captura de tela

<p align="center">
  <img src="img/Captura-de-tela.png" alt="Interface do aplicativo de gerenciamento de hotéis" width="800" />
</p>

## 🚀 Funcionalidades

### 👤 Gerenciamento de hóspedes
- Cadastro de hóspedes.
- Edição e exclusão de registros.
- Validação de CPF.
- Verificação de CPF duplicado.

### 🛏️ Gerenciamento de quartos
- Cadastro, edição e exclusão de quartos.
- Numeração única para cada quarto.
- Definição de tipo, capacidade e valor da diária.
- Controle do status de ocupação.

### 📋 Gerenciamento de hospedagens
- Realização de check-in para hóspedes cadastrados.
- Seleção apenas de quartos disponíveis.
- Registro de check-out.
- Cálculo do valor total da hospedagem com base nas diárias.

### 📊 Painel de resumo
- Quantidade total de quartos.
- Quantidade de quartos disponíveis.
- Quantidade de quartos ocupados.

## 🛠️ Tecnologias utilizadas

- **C#** — linguagem de programação.
- **Windows Forms (WinForms)** — desenvolvimento da interface gráfica.
- **.NET 8** — plataforma de desenvolvimento.
- **JSON** — armazenamento local dos dados.

## 📋 Requisitos

Antes de executar o projeto, certifique-se de ter instalado:

- Windows 10 ou Windows 11.
- [.NET 8 SDK](https://dotnet.microsoft.com/download).
- [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/vs/) com a carga de trabalho de desenvolvimento para desktop com .NET, caso opte por utilizá-lo.

## ▶️ Como executar

### Opção 1 — Terminal

1. Clone o repositório:

   ```bash
   git clone https://github.com/Taunt-byte/App-hotel-Csharp.git
   ```

2. Entre na pasta do projeto:

   ```bash
   cd HotelApp
   ```

3. Execute a aplicação:

   ```bash
   dotnet run
   ```

### Opção 2 — Visual Studio 2022

1. Abra o arquivo `HotelApp.csproj` no Visual Studio 2022.
2. Aguarde a restauração das dependências.
3. Pressione **F5** para compilar e executar a aplicação.

## 💾 Persistência de dados

Os dados são armazenados localmente em arquivos JSON no diretório de dados da aplicação, dentro de `%AppData%\HotelApp\`.

Essa abordagem permite manter os registros entre execuções sem a necessidade de configurar um servidor de banco de dados.

## 🎯 Objetivos de aprendizagem

Este projeto foi desenvolvido com o propósito de praticar:

- Programação orientada a objetos (POO).
- Criação de interfaces gráficas com WinForms.
- Operações CRUD (Create, Read, Update, Delete).
- Validação de dados e prevenção de duplicidade.
- Separação entre modelos, acesso a dados e regras de negócio.
- Serialização e desserialização de arquivos JSON.

## 🔮 Melhorias futuras

- Implementar autenticação e controle de acesso.
- Adicionar filtros e pesquisa de hóspedes.
- Criar relatórios de ocupação e faturamento.
- Implementar testes automatizados.
- Avaliar a utilização de um banco de dados relacional.

## ⚠️ Observação

Este projeto possui finalidade educacional e de portfólio. Antes de utilizá-lo em um ambiente hoteleiro real, será necessário avaliar aspectos de segurança, controle de acesso, cópias de segurança e proteção de dados pessoais conforme a LGPD.

## 👨‍💻 Autor

Desenvolvido como projeto de estudo em C# e desenvolvimento de aplicações desktop.
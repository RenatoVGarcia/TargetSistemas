# 📌 TargetSistemas – Desafios Técnicos

Este repositório contém as soluções desenvolvidas para o processo seletivo da **Target Sistemas**, organizadas em três desafios independentes.
Todos foram implementados em **C# (.NET 8)**.

---

## 📁 Estrutura do Projeto

```
TargetSistemas-main/
│
├── Desafio1_Comissao/
│   ├── Program.cs
│   ├── vendas.json
│   └── Desafio1_Comissao.csproj
│
├── Desafio2_Estoque/
│   ├── Program.cs
│   ├── estoque.json
│   └── Desafio2_Estoque.csproj
│
├── Desafio3_Juros/
│   ├── Program.cs
│   └── Desafio3_Juros.csproj
│
└── TargetSistemas.sln
```

---

## ✅ Desafio 1 – Cálculo de Comissão

**Objetivo:**
Ler um arquivo `vendas.json` e calcular o valor total de comissão de acordo com o percentual definido.

**Pontos principais da solução:**

* Leitura do JSON usando `System.Text.Json`
* Soma total das vendas
* Cálculo da comissão
* Tratamento de erros e validações

**Execução:**

```bash
dotnet run --project Desafio1_Comissao
```

---

## ✅ Desafio 2 – Validação de Estoque

**Objetivo:**
Ler o arquivo `estoque.json`, identificar itens com estoque abaixo do mínimo e gerar um relatório de alerta.

**Pontos principais da solução:**

* Desserialização de lista de produtos
* Comparação entre estoque atual e mínimo
* Impressão de itens críticos de reposição

**Execução:**

```bash
dotnet run --project Desafio2_Estoque
```

---

## ✅ Desafio 3 – Cálculo de Juros Compostos

**Objetivo:**
Implementar um programa que calcula o valor futuro de um investimento com base em juros compostos.

**Pontos principais da solução:**

* Fórmula de juros compostos:

  ```
  M = P * (1 + i)^n
  ```
* Entrada de valores pelo usuário
* Validações de entrada
* Formatação final do valor

**Execução:**

```bash
dotnet run --project Desafio3_Juros
```

---

## 🧰 Tecnologias Utilizadas

* C# 12 / .NET 8
* System.Text.Json
* Programação estruturada e modular

---

## 👨‍💻 Autor
**Renato Garcia**

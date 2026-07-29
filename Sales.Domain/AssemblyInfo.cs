// Habilita a visualização dos membros internos para o projeto de testes "Sales.Domain.Tests"
// Isso é útil para permitir que os testes acessem membros internos da biblioteca de domínio, sem expô-los publicamente

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Sales.Domain.Tests")]
[assembly: InternalsVisibleTo("Sales.Application")]
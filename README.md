# [WorkTimeManager](https://wtm-api-v1.herokuapp.com)
API приложения с функционалом планирования задач и подсчета времени работы


[![Build status](https://ci.appveyor.com/api/projects/status/sxd26t44a6p285q5?svg=true)](https://ci.appveyor.com/project/melonsound/worktimemanager)

Особенности:
- Регистрация/авторизация в сервисе
- Добавление, редактирование, удаление задач
- Учет времени задач 

Используется:
- ASP.NET Core 5 (Web API)
- Swagger OpenAPI спецификация
- Docker 
- PostgreSQL 

# Build

Requires .NET 5 SDK to build

Install the dependencies and devDependencies and start the server.

```sh
dotnet restore
dotnet build
dotnet run
```


# [WorkTimeManager](https://melonsound.ru/swagger)
API приложения с функционалом планирования задач и подсчета времени работы


[![Build status](https://ci.appveyor.com/api/projects/status/sxd26t44a6p285q5?svg=true)](https://ci.appveyor.com/project/melonsound/worktimemanager)

Особенности:
- Регистрация/авторизация в сервисе
- Добавление задач в избранное
- Добавление, редактирование, удаление задач
- Учет времени задач 

Используется:
- ASP.NET 6 (Web API)
- Swagger OpenAPI спецификация
- Docker 
- PostgreSQL 
- FluentValidation
- Mapster
- Google Drive API v3

# Build

Requires .NET 6 SDK to build

Install the dependencies and devDependencies and start the server.

```sh
dotnet restore
dotnet build
dotnet run
```
# ENV Variables

- MAIN_DB - Infrastructure database connection string (PostgreSQL)
- SECURITY_DB - Security (Microsoft identity) database connection string (PostgreSQL)
- APP_JWT_ISSUERKEY - JWT Issuer key
- APP_GOOGLEDRIVE_FOLDERID - Since the Google Drive API is temporarily unused, the environment variable can be filled with any value.


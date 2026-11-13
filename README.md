# MedicalApi — API частной клиники

ASP.NET Core Web API для пациентов, справок, каталога справок и записи на приём. Данные хранятся в PostgreSQL через EF Core.

## Запуск

Требуется .NET 10 SDK и PostgreSQL на `localhost:5432`. Установите PostgreSQL, создайте пользователя `postgres` и укажите его пароль в `MedicalApi/appsettings.json` (строка подключения `ConnectionStrings:Medical`). Значение `postgres` в файле — пример для локальной учебной установки.

```powershell
dotnet ef database update --project MedicalApi
dotnet run --project MedicalApi
```

При разработке Swagger UI доступен по адресу `/swagger` на порту, который напечатает приложение. EF Core выводит SQL-команды в консоль. Миграции `Init`, `AddPatientEmail` и `AppointmentLocalTime` создают таблицы, добавляют необязательное поле Email и задают тип времени приёма.

## Маршруты

- `GET/POST /api/patients`, `GET/PUT/DELETE /api/patients/{id}` — пациенты; `GET /api/patients?name=ай` выполняет поиск без учёта регистра.
- `GET /api/patients/{id}/spravki` — справки пациента.
- `GET/POST /api/spravki`, `GET/PUT/DELETE /api/spravki/{id}` — справки; доступны фильтры `patientId`, `minPrice` и сортировка `sort=price`.
- `GET/POST /api/spravkaTypes`, `GET/PUT/DELETE /api/spravkaTypes/{id}` — каталог видов справок. Начальные записи добавьте в таблицу через SQL.
- `GET/POST /api/appointments`, `GET/PUT/DELETE /api/appointments/{id}` — записи на приём.
- `GET /api/stats` — число пациентов, справок и сумма выданных справок.

Создайте пациента и заполните таблицу `SpravkaTypes` перед выпуском справок. Удаление пациента с выданными справками или приёмами, а также удаление используемого вида справки запрещено и возвращает 400.

## SQL для задания

- `sql/clinic.sql` — создание и заполнение учебных таблиц, запросы из ручной части.
- `sql/ef-queries.sql` — шаблоны SQL запросов EF Core для PatientsController. После вызовов Swagger замените примеры фактическими строками из консоли.


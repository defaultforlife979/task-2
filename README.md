# MedicalApi — API частной клиники

ASP.NET Core Web API для справок, пациентов, каталога видов справок и записи на приём.

## Запуск

Требуется .NET 10 SDK.

```powershell
dotnet run --project MedicalApi
```

В режиме разработки Swagger UI доступен по адресу `/swagger` на порту, который напечатает приложение. Данные хранятся в памяти и сбрасываются при перезапуске.

## Основные маршруты

- `GET/POST /api/patients`, `GET/PUT/DELETE /api/patients/{id}` — пациенты.
- `GET /api/patients/{id}/spravki` — справки пациента. При удалении пациента его справки и записи на приём удаляются каскадно.
- `GET/POST /api/spravki`, `GET/PUT/DELETE /api/spravki/{id}` — справки; `GET /api/spravki?patientId=1` фильтрует список.
- `GET/POST /api/spravkaTypes`, `GET/PUT/DELETE /api/spravkaTypes/{id}` — каталог справок, заполненный начальными данными.
- `GET/POST /api/appointments`, `GET/PUT/DELETE /api/appointments/{id}` — записи на приём.

Создайте пациента перед созданием справки или записи на приём. Справка получает название и цену из каталога по `spravkaTypeId`.

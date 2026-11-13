-- SQL ниже показывает форму запросов EF Core для действий PatientsController.
-- Значения параметров по умолчанию маскируются в логе как '?'. Снимите реальные
-- строки из консоли dotnet run после вызовов Swagger и замените примеры при сдаче.

-- Swagger: GET /api/patients. Возвращает всю коллекцию; WHERE по Id отсутствует.
SELECT p."Id", p."Address", p."BirthDate", p."Email", p."HasAllergies", p."Name", p."Phone"
FROM "Patients" AS p
ORDER BY p."Name";

-- Swagger: GET /api/patients/{id}. Та же таблица с фильтром и LIMIT 1.
SELECT p."Id", p."Address", p."BirthDate", p."Email", p."HasAllergies", p."Name", p."Phone"
FROM "Patients" AS p
WHERE p."Id" = @__id_0
LIMIT 1;

-- Swagger: POST /api/patients. EF вставляет данные без Id и получает сгенерированный Id
-- через RETURNING; значение появляется в объекте после SaveChangesAsync().
INSERT INTO "Patients" ("Address", "BirthDate", "Email", "HasAllergies", "Name", "Phone")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";

-- Swagger: PUT /api/patients/{id}. EF обновляет только изменившиеся поля.
UPDATE "Patients" SET "Name" = @p0
WHERE "Id" = @p1;

-- Swagger: повторный PUT с теми же значениями не меняет tracked entity,
-- поэтому EF Core не отправляет UPDATE.

-- Swagger: DELETE /api/patients/{id}. Запрос удаления строки.
DELETE FROM "Patients" WHERE "Id" = @p0;

-- Ответы к вопросам:
-- GET all читает полный список, GET по id добавляет WHERE Id = ... и LIMIT 1.
-- Id от POST возвращается базой через RETURNING "Id" в ответе INSERT.
-- Значения параметров скрыты, чтобы не записывать персональные данные в лог.

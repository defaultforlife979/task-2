-- Создать таблицы учебной базы clinic (выполняйте в базе clinic).
CREATE TABLE patients (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    birth_date DATE NOT NULL,
    phone VARCHAR(20)
);

CREATE TABLE spravka_types (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price NUMERIC(10, 2) NOT NULL CHECK (price > 0),
    valid_days INT NOT NULL
);

CREATE TABLE spravki (
    id SERIAL PRIMARY KEY,
    patient_id INT NOT NULL REFERENCES patients(id),
    spravka_type_id INT NOT NULL REFERENCES spravka_types(id),
    price NUMERIC(10, 2) NOT NULL,
    issued_at DATE NOT NULL DEFAULT CURRENT_DATE
);

-- Добавить исходных пациентов.
INSERT INTO patients (name, birth_date, phone) VALUES
    ('Айбек Асанов', '1998-03-12', '0555123456'),
    ('Нургуль Токтосунова', '2003-07-25', '0700987654'),
    ('Бакыт Жумабаев', '1985-11-02', NULL),
    ('Айпери Садыкова', '2001-01-30', '0772345678'),
    ('Тимур Ким', '2006-09-14', '0509111222');

-- Добавить виды справок.
INSERT INTO spravka_types (name, price, valid_days) VALUES
    ('Справка 083', 100, 365),
    ('Санитарная книжка', 750, 365),
    ('Справка 086', 300, 180),
    ('Справка в бассейн', 200, 90);

-- Добавить выданные справки.
INSERT INTO spravki (patient_id, spravka_type_id, price, issued_at) VALUES
    (1, 1, 100, '2026-10-01'),
    (1, 4, 200, '2026-10-15'),
    (2, 2, 750, '2026-10-03'),
    (2, 3, 300, '2026-11-02'),
    (4, 3, 300, '2026-11-05'),
    (5, 4, 200, '2026-11-06'),
    (5, 1, 100, '2026-11-10');

-- Добавить свои данные: ещё минимум 3 пациента.
INSERT INTO patients (name, birth_date, phone) VALUES
    ('Мээрим Осмонова', '1999-04-08', '0777000111'),
    ('Руслан Иманов', '2002-12-19', '0700111222'),
    ('Алина Петрова', '1997-06-21', NULL);

-- Добавить свой вид справки.
INSERT INTO spravka_types (name, price, valid_days) VALUES ('Справка для вождения', 500, 365);

-- Добавить свои справки (после исходных данных и строк выше).
INSERT INTO spravki (patient_id, spravka_type_id, price, issued_at) VALUES
    (6, 5, 500, '2026-11-11'),
    (7, 1, 100, '2026-11-12'),
    (8, 3, 300, '2026-11-12'),
    (6, 4, 200, '2026-11-13'),
    (7, 2, 750, '2026-11-13');

-- Все пациенты по алфавиту.
SELECT * FROM patients ORDER BY name;

-- Пациенты, родившиеся в 2000 году и позже.
SELECT * FROM patients WHERE birth_date >= DATE '2000-01-01' ORDER BY birth_date;

-- Пациенты без телефона (сравнение с NULL через = не работает).
SELECT * FROM patients WHERE phone IS NULL;
-- WHERE phone = NULL не даст совпадений: NULL означает неизвестное значение,
-- его проверяют операторами IS NULL / IS NOT NULL.

-- Поиск имени, чувствительный к регистру.
SELECT * FROM patients WHERE name LIKE '%ай%';
-- Поиск без учёта регистра в PostgreSQL.
SELECT * FROM patients WHERE name ILIKE '%ай%';

-- Справки пациента с id 1, с названием справки.
SELECT s.id, t.name AS spravka, s.price, s.issued_at
FROM spravki s JOIN spravka_types t ON t.id = s.spravka_type_id
WHERE s.patient_id = 1 ORDER BY s.issued_at;

-- Справки, выписанные в ноябре 2026.
SELECT * FROM spravki
WHERE issued_at >= DATE '2026-11-01' AND issued_at < DATE '2026-12-01'
ORDER BY issued_at;

-- Самый дорогой вид справки в каталоге.
SELECT * FROM spravka_types ORDER BY price DESC LIMIT 1;

-- Количество и сумма справок, выписанных в ноябре 2026.
SELECT COUNT(*) AS spravki_count, COALESCE(SUM(price), 0) AS total
FROM spravki
WHERE issued_at >= DATE '2026-11-01' AND issued_at < DATE '2026-12-01';

-- Поднять цену каталога для id 1; уже выданные справки сохраняют свою цену.
UPDATE spravka_types SET price = price + 50 WHERE id = 1;
SELECT s.id, s.price AS sold_price, t.price AS current_catalog_price
FROM spravki s JOIN spravka_types t ON t.id = s.spravka_type_id
WHERE t.id = 1;

-- Удалить одну справку, не связанную с демонстрационными проверками ниже.
DELETE FROM spravki WHERE id = 12;

-- Посчитать количество справок у каждого пациента, включая пациентов без справок.
SELECT p.id, p.name, COUNT(s.id) AS spravki_count
FROM patients p LEFT JOIN spravki s ON s.patient_id = p.id
GROUP BY p.id, p.name ORDER BY p.name;

-- ОЖИДАЕМАЯ ОШИБКА: FK запрещает сослаться на несуществующего пациента.
-- INSERT INTO spravki (patient_id, spravka_type_id, price) VALUES (999, 1, 100);

-- ОЖИДАЕМАЯ ОШИБКА: CHECK запрещает цену <= 0.
-- INSERT INTO spravka_types (name, price, valid_days) VALUES ('Плохая цена', -500, 30);

-- ОЖИДАЕМАЯ ОШИБКА: NOT NULL запрещает пустое имя.
-- INSERT INTO patients (birth_date) VALUES ('2000-01-01');

-- ОЖИДАЕМАЯ ОШИБКА: FK запрещает удалить пациента, на которого ссылаются справки.
-- DELETE FROM patients WHERE id = 1;

-- Номер SERIAL выдаётся последовательностью; неудачные INSERT могут оставить пропуски.
INSERT INTO spravki (patient_id, spravka_type_id, price) VALUES (3, 2, 750);
SELECT * FROM spravki WHERE patient_id = 3 ORDER BY id DESC;


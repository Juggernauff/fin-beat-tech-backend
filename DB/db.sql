/*
Задача 2
Даны таблицы:
Clients - клиенты
(
        	Id bigint, -- Id клиента
        	ClientName nvarchar(200) -- Наименование клиента
);
ClientContacts - контакты клиентов
(
        	Id bigint, -- Id контакта
        	ClientId bigint, -- Id клиента
        	ContactType nvarchar(255), -- тип контакта
        	ContactValue nvarchar(255) --  значение контакта
);
1.	Написать запрос, который возвращает наименование клиентов и кол-во контактов клиентов
2.	Написать запрос, который возвращает список клиентов, у которых есть более 2 контактов

Задача 3 (опционально)
Дана таблица:
Dates - клиенты
(
        	Id bigint,
        	Dt date
);
1.	Написать запрос, который возвращает интервалы для одинаковых Id.
 */

/* Задача 2. Пункт 1. */
SELECT c."ClientName" AS ClientName,
       COUNT(cc."Id") AS NumContacts
FROM "Clients" AS c
LEFT JOIN "ClientContacts" AS cc
    ON c."Id" = cc."ClientId"
GROUP BY c."ClientName"
ORDER BY NumContacts ASC;

/* Задача 2. Пункт 2. */
SELECT c."ClientName" AS ClientName
FROM "Clients" AS c
JOIN "ClientContacts" AS cc
    ON c."Id" = cc."ClientId"
GROUP BY c."ClientName"
HAVING COUNT(cc."Id") > 2;

/* Задача 3. */
WITH CTE AS (
    SELECT
        "Id",
        dt AS startDate,
        LEAD(dt) OVER (PARTITION BY "Id" ORDER BY dt) AS endDate
    FROM
        public."Dates"
)
SELECT
    "Id",
    startDate,
    CASE
        WHEN endDate IS NULL THEN startDate
        ELSE endDate
    END
FROM
    CTE
ORDER BY
    "Id", startDate;
-- Необходимо написать табличную функцию SQL, которая будет возвращать по 
-- ClientId и интервалу дат (тип Date) поденные суммы платежей. Если за 
-- указанный день не было платежей, то функция должна возвращать 0. Интервалы 
-- дат могут охватывать несколько лет.

CREATE TABLE ClientPayments
(
    Id       BIGINT PRIMARY KEY,    -- первичный ключ таблицы
    СlientId BIGINT       NOT NULL, -- Id клиента
    Dt       TIMESTAMP(0) NOT NULL, -- дата платежа
    Amount   MONEY        NOT NULL  -- сумма платежа
);

INSERT INTO ClientPayments
    (Id, СlientId, Dt, Amount)
VALUES (1, 1, '2022-01-03 17:24:00', 100),
       (2, 1, '2022-01-05 17:24:14', 200),
       (3, 1, '2022-01-05 18:23:34', 250),
       (4, 1, '2022-01-07 10:12:38', 50),
       (5, 2, '2022-01-05 17:24:14', 278),
       (6, 2, '2022-01-10 12:39:29', 300);

CREATE OR REPLACE FUNCTION get_daily_payments(
    client_id BIGINT,
    start_date DATE,
    end_date DATE
)
    RETURNS TABLE
            (
                dt  DATE,
                sum MONEY
            )
AS
$$
SELECT d, COALESCE(SUM(cp.amount), 0::MONEY)
FROM generate_series(start_date, end_date, INTERVAL '1 day') d
         LEFT JOIN clientpayments cp
                   ON cp.СlientId = client_id AND cp.dt::DATE = d
GROUP BY d
ORDER BY d;
$$ LANGUAGE sql;

SELECT dt, sum as Сумма
FROM get_daily_payments(1, '2022-01-02', '2022-01-07');

SELECT dt, sum as Сумма
FROM get_daily_payments(2, '2022-01-04', '2022-01-11');
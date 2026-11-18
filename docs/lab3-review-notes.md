# Замечания к третьим лабораторным

Просмотрены ревью [№137](https://github.com/itsecd/enterprise-development/pull/137),
[№159](https://github.com/itsecd/enterprise-development/pull/159),
[№169](https://github.com/itsecd/enterprise-development/pull/169) и
[№143](https://github.com/itsecd/enterprise-development/pull/143).

| Замечание | Что сделано |
| --- | --- |
| [Аналитика должна выполняться в БД](https://github.com/itsecd/enterprise-development/pull/169#discussion_r4614071939) | EF-запросы с фильтрами, EXISTS, GROUP BY, COUNT, SUM и LIMIT; материализация после запроса |
| [Ограничить длины строк](https://github.com/itsecd/enterprise-development/pull/159#discussion_r4601149882) | HasMaxLength в конфигурациях, ограничения в DTO |
| [Уникальные госномер и паспорт](https://github.com/itsecd/enterprise-development/pull/159#discussion_r4601328601) | Уникальные индексы в миграции схемы, проверка ограничения напрямую через DbContext |
| [Не править вручную обратные навигации](https://github.com/itsecd/enterprise-development/pull/159#discussion_r4601453697) | Отслеживание EF, внешние ключи и обычное сохранение изменений |
| [Убрать зависимость API от InMemory](https://github.com/itsecd/enterprise-development/pull/159#discussion_r4601342345) | API ссылается на Infrastructure.MySql, InMemory используется только unit-тестами |
| [Не загружать ненужные навигации](https://github.com/itsecd/enterprise-development/pull/137#discussion_r4613025660) | DTO используют FK; у заказа загружается только список работ, нужный в ответе; сумма считается отдельным SQL-запросом |
| [Явный пароль и имя тома Aspire](https://github.com/itsecd/enterprise-development/pull/137#discussion_r4613142126) | Секретный параметр mysql-password, именованный том autoservice-mysql-data |
| [Без строк подключения в коде](https://github.com/itsecd/enterprise-development/pull/169#discussion_r4614140838) | Переменные окружения и user-secrets; Aspire передаёт ConnectionStrings__autoservice |
| [Нарушение FK должно возвращать 409](https://github.com/itsecd/enterprise-development/pull/169#discussion_r4614715359) | Общий обработчик DbUpdateException с кодами MySQL, без раскрытия SQL |
| [Факультативные интеграционные тесты должны использовать Aspire](https://github.com/itsecd/enterprise-development/pull/169#discussion_r4614174119) | Отдельный проект AspireTests, DistributedApplicationTestingBuilder и запуск настоящего AppHost |

Вторая работа [№96](https://github.com/itsecd/enterprise-development/pull/96) получила approve.
Сохранены её контракты и маршруты API.

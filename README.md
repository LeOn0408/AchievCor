# AchievCor
# Первый запуск

## 1️⃣ Настройка окружения

Заполни `.env` в корне проекта:

Auth__SecurityKey=YOUR_32+_CHAR_SECRET_KEY
Admin__Login=admin
Admin__Password=admin123
Admin__Email=admin@localhost
DB_CONNECTION_STRING=Host=localhost;Port=5432;Database=achievcore;Username=postgres;Password=postgres;


> ⚠️ Ключ `Auth__SecurityKey` должен быть не менее 32 символов (256 бит).

---

## 2️⃣ Применение миграций и создание администратора

При первом запуске приложение автоматически:
- Применяет миграции EF Core;
- Создаёт администратора (`Admin__Login`);
- Добавляет роль `Admin`.

---

## 3️⃣ Запуск приложения


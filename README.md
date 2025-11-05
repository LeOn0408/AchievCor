# AchievCor
Cистему геймификации для корпоративной среды с интеграцией в Битрикс24 (и другими системами в будущем).  
Проект имеет бэкенд на ASP.NET Core и фронтенд на Vue 3.

Цель: мотивация сотрудников через систему благодарностей, наград за прохождение курсов, рейтингов и лидербордов.

Технический стек:

- Backend: ASP.NET Core Web API, Entity Framework Core, MySQL
- Frontend: Vue 3 , Vite
- Интеграция: Bitrix24 REST API
- Деплой: Docker + Nginx

### Версия dev (текущая)

- Авторизация пользователей/роли
# Первый запуск

## 1️ Настройка окружения

Заполни `.env` в корне проекта:

`Auth__SecurityKey=YOUR_32+_CHAR_SECRET_KEY`
`Admin__Login=admin`
`Admin__Password=admin123`
`Admin__Email=admin@localhost`
`DB_CONNECTION_STRING=Host=localhost;Port=5432;Database=achievcore;Username=postgres;Password=postgres;`


> ⚠️ Ключ `Auth__SecurityKey` должен быть не менее 32 символов (256 бит).

---

## 2️ Применение миграций и создание администратора

При первом запуске приложение автоматически:
- Применяет миграции EF Core;
- Создаёт администратора (`Admin__Login`);
- Добавляет роль `Admin`.

---

## 3️ Запуск приложения


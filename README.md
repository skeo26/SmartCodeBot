# 🤖 SmartCodeBot – Telegram-бот для изучения программирования

SmartCodeBot — это Telegram-бот, предназначенный для интерактивного обучения языкам программирования **C#** и **SQL**. Он предлагает теоретические материалы, тесты и помощь от нейросети, оформленные в удобный интерфейс прямо в Telegram.

---

## 🚀 Возможности

- Выбор языка программирования (C# / SQL)
- Изучение теории по темам
- Тестирование пройденного материала
- Сохранение прогресса пользователя
- Интеграция с нейросетью DeepSeek AI
- Удобное взаимодействие через inline и reply-кнопки

---

## ⚙️ Установка и запуск

1. Клонируйте репозиторий:

   ```bash
   git clone https://github.com/yourusername/SmartCodeBot.git
   cd SmartCodeBot
   ```

2. Откройте проект в Visual Studio (или Rider/VS Code, если поддерживает .NET 7).

3. Перейдите к файлу:

   ```
   SmartCodeBot\bin\Debug\net7.0\appsettings.json
   ```

   и укажите следующие параметры:

   ```json
   {
     "BotConfiguration": {
       "BotToken": "ВАШ_ТОКЕН_ОТ_BOTFATHER",
       "ConnectionString": "СТРОКА_ПОДКЛЮЧЕНИЯ_К_ВАШЕЙ_БАЗЕ_ДАННЫХ",
       "ApiKey": "ВАШ_API_КЛЮЧ_НЕЙРОСЕТИ_ФОРМАТА io-v2-..."
     }
   }
   ```

   > 🔐 **Важно:**  
   > - `BotToken` — получить в Telegram через [@BotFather](https://t.me/BotFather).  
   > - `ConnectionString` — строка подключения к БД (например, PostgreSQL или SQL Server).  
   > - `ApiKey` — ключ для DeepSeek (например: `io-v2-...`).

4. Постройте и запустите проект из Visual Studio.

---

## 🧠 Технологии

- C#
- Telegram.Bot API
- Clean Architecture
- PostgreSQL / SQL Server
- Асинхронное программирование
- DeepSeek API

---

## 🧩 Архитектура проекта

```
SmartCodeBot/
├── CommandProccesors/   # Обработчики команд
├── Commands/            # Обработчики команд
├── DataAccess/          # Работа с данными
├── Handlers/            # Обработчики
├── KeyboardsMarkup/     # Клавиатуры
├── Models/              # БЛ
├── Providers/           # Провайдеры данных
├── RegisterCommand/     # Регистратор команд
├── Services/            # Сервисы
├── Tests/               # Файлы в формате HTML для тестов
├── Topics/              # Материалы в формате HTML
└── appsettings.json     # Конфигурация
```

---

## 🧪 Примеры использования

- Введите `/start` в Telegram, чтобы начать
- Выберите язык → раздел → тему
- Изучите теорию и пройдите тест
- Используйте кнопку "SmartCode помощник" для общения с AI

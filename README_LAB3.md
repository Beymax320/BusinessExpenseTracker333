# Лабораторна робота №3 — SQLite + Repository

Варіант 2: трекер бізнес-витрат для ФОП.

## Запуск
1. Відкрити BusinessExpenseTracker.csproj у Visual Studio 2022 з .NET MAUI workload та .NET 9 SDK.
2. Відновити NuGet-пакети (sqlite-net-pcl, CommunityToolkit.Mvvm).
3. Вибрати Android Emulator або підключений Android-пристрій і запустити.

## Таблиця Expenses
- Id — INTEGER PRIMARY KEY AUTOINCREMENT
- Name — TEXT NOT NULL, MaxLength(150)
- Amount — зберігається ORM з типу decimal; перевіряйте точність при фінансових розрахунках
- Category — TEXT NOT NULL, MaxLength(100)
- Date — DateTime, дата додавання

Файл `expenses.db3` створюється в `FileSystem.AppDataDirectory`. `DatabaseService` є Singleton і містить асинхронні CRUD-методи. ViewModel не звертаються до SQLite напряму. Чернетки введення зберігаються в Preferences, як у лабораторній №2.

## Перевірка
1. Додайте витрату.
2. Відкрийте її, змініть дані та збережіть.
3. Додайте іншу витрату та видаліть її з підтвердженням.
4. Повністю завершіть застосунок і відкрийте знову: попередні витрати повинні залишитися.

Для звіту зробіть скріншоти кожного кроку, додайте 3 реальні запити до LLM, git-коміти та посилання на репозиторій.

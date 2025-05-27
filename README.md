# Game-Home-ha

## Описание проекта

**Game-home-ha** — это консольное C# приложение для управления игровой платформой, позволяющее пользователям отслеживать свои достижения в различных играх, добавлять друзей и взаимодействовать с игровыми платформами. Проект демонстрирует объектно-ориентированное проектирование с использованием баз данных, в котором реализованы основные концепции ООП — наследование, полиморфизм и инкапсуляция.

---

## Основные возможности

- Управление платформами (например, Steam, Epic Games и др.)
- Добавление и хранение информации об играх, относящихся к каждой платформе
- Учёт достижений, доступных в каждой игре
- Ведение списка пользователей с возможностью добавления друзей
- Отслеживание, какие достижения были разблокированы каждым пользователем и когда
- Реализация связей между пользователями, играми и достижениями с помощью базы данных

---

## Технические детали

- **Язык:** C#
- **Тип приложения:** Консольное приложение
- **База данных:** Любая реляционная СУБД (например, SQLite, SQL Server, PostgreSQL)
- **Объектно-ориентированное проектирование:**
  - 7+ классов: User, Game, Platform, Achievement, UserAchievement, FriendLink и др.
  - Использованы ключевые механизмы ООП: наследование, полиморфизм, инкапсуляция
- **Диаграммы UML:**
  - Диаграмма классов — для описания структуры и связей классов (файл в Project/Uml)
  - Диаграмма последовательностей — для отображения взаимодействия объектов в ключевых сценариях (будет добавлена)
  - Диаграмма вариантов использования — для отображения функций и действий пользователей (будет добавлена)

---

## Структура папок по этапам

Проект разделен на этапы, каждому из которых соответствует отдельная папка:

*   **stage 0:** Выбор темы (описано в этом README)
*   **stage 1:** Проектирование системы (UML-диаграммы в Project/Uml)
*   **stage 2:** Создание базы данных (SQL скрипты и описание в sql/)
*   **stage 3:** Реализация слоя представления (консольное приложение в stage 3/ConsoleApp1)
*   **stage 4:** Реализация слоя работы с БД (классы для взаимодействия с БД в stage 4/)
*   **stage 5:** Реализация бизнес-слоя (классы бизнес-логики в stage 5/)

Описание каждого этапа и его содержимое находится в файлах `README.md` внутри соответствующих папок `stage X/`.

---

## Планы на будущее

- Добавление графического интерфейса пользователя (GUI)
- Расширение возможностей социального взаимодействия (чат, групповые достижения)
- Интеграция с внешними игровыми API для автоматического обновления достижений

---

![UML Diagram](https://github.com/user-attachments/assets/1998fd9c-b240-41df-99bd-ce5f6c090db3)


### Описание структуры базы данных 

**Таблицы и связи:**

1. **Platform**
    
    - `id` (PK)
        
    - `name` (название платформы)
        
    - Связь: один ко многим с таблицей **Game**.
        
2. **Account**
    
    - `id` (PK)
        
    - `username`, `email` (уникальные)
        
    - Связь: один к одному с таблицей **User**.
        
3. **User**
    
    - `id` (PK)
        
    - `account_id` (FK → Account.id)
        
    - `password`
        
    - Связи:
        
        - один к одному с **Account** и **EpicAccount**,
            
        - один ко многим с **GameSession**,
            
        - многие ко многим с **User** (через **FriendLink**) и **Achievement** (через **UserAchievement**).
            
4. **EpicAccount**
    
    - `id` (PK)
        
    - `user_id` (FK → User.id, уникальный)
        
    - Связь: один к одному с **User**.
        
5. **Game**
    
    - `id` (PK)
        
    - `platformid` (FK → Platform.id)
        
    - Связи:
        
        - один ко многим с **GameSession** и **Achievement**.
            
6. **GameSession**
    
    - `id` (PK)
        
    - `userid` (FK → User.id), `gameid` (FK → Game.id)
        
    - Связь: многие ко одному с **User** и **Game**.
        
7. **Achievement**
    
    - `id` (PK)
        
    - `gameid` (FK → Game.id)
        
    - Связь: многие ко многим с **User** через **UserAchievement**.
        
8. **UserAchievement**
    
    - Составной PK (`userid`, `achievementid`)
        
    - Связь: многие ко многим между **User** и **Achievement**.
        
9. **FriendLink**
    
    - Составной PK (`Userid`, `friendid`)
        
    - Связь: многие ко многим между **User** и **User**.


### SQL скрипт БД

```
CREATE TABLE Platform (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE Account (
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE "User" (
    id SERIAL PRIMARY KEY,
    account_id INT UNIQUE NOT NULL,
    password VARCHAR(100) NOT NULL,
    FOREIGN KEY (account_id) REFERENCES Account(id)
);

CREATE TABLE EpicAccount (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    user_id INT UNIQUE NOT NULL,
    FOREIGN KEY (user_id) REFERENCES "User"(id)
);

CREATE TABLE Game (
    id SERIAL PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    genre VARCHAR(50),
    platformid INT NOT NULL,
    FOREIGN KEY (platformid) REFERENCES Platform(id)
);

-- Создание таблицы GameSession
CREATE TABLE GameSession (
    id SERIAL PRIMARY KEY,
    userid INT NOT NULL,
    gameid INT NOT NULL,
    StartTime TIMESTAMP NOT NULL,
    durationMinutes INT NOT NULL,
    FOREIGN KEY (userid) REFERENCES "User"(id),
    FOREIGN KEY (gameid) REFERENCES Game(id)
);

CREATE TABLE Achievement (
    id SERIAL PRIMARY KEY,
    gameid INT NOT NULL,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    FOREIGN KEY (gameid) REFERENCES Game(id)
);

CREATE TABLE UserAchievement (
    userid INT NOT NULL,
    achievementid INT NOT NULL,
    unlockedAt TIMESTAMP NOT NULL,
    PRIMARY KEY (userid, achievementid),
    FOREIGN KEY (userid) REFERENCES "User"(id),
    FOREIGN KEY (achievementid) REFERENCES Achievement(id)
);

CREATE TABLE FriendLink (
    Userid INT NOT NULL,
    friendid INT NOT NULL,
    PRIMARY KEY (Userid, friendid),
    FOREIGN KEY (Userid) REFERENCES "User"(id),
    FOREIGN KEY (friendid) REFERENCES "User"(id)
);
```
### Этап 4: Реализация слоя работы с БД

#### 1. Модели данных (Models)
Создайте классы, которые будут отражать структуру таблиц в базе данных.

C#

// User.cs
```public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
```
// Game.cs
```
public class Game
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public int PlatformId { get; set; }
}

// GameSession.cs
public class GameSession
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int GameId { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
}
```

Объяснение:
- Каждый класс соответствует таблице в БД.
- Свойства (Id, Username и т.д.) — это колонки таблиц.

---

#### 2. Репозитории (Repositories)
Классы, отвечающие за взаимодействие с БД.

C#

// UserRepository.cs
```
using Npgsql;
using System;

public class UserRepository
{
    private readonly string _connectionString;

  public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Создание пользователя
    public void Create(User user)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new NpgsqlCommand(
                "INSERT INTO Account (username, email) VALUES (@u, @e);" +
                "INSERT INTO \"User\" (account_id, password) VALUES (currval(pg_get_serial_sequence('account', 'id')), @p)", 
                conn);
            cmd.Parameters.AddWithValue("u", user.Username);
            cmd.Parameters.AddWithValue("e", user.Email);
            cmd.Parameters.AddWithValue("p", user.Password);
            cmd.ExecuteNonQuery();
        }
    }

    // Получение пользователя по ID
    public User GetById(int id)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new NpgsqlCommand(
                "SELECT u.id, a.username, a.email, u.password " +
                "FROM \"User\" u " +
                "JOIN Account a ON u.account_id = a.id " +
                "WHERE u.id = @id", 
                conn);
            cmd.Parameters.AddWithValue("id", id);
            using (var reader = cmd.ExecuteReader())
            {
                return reader.Read() ? new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Email = reader.GetString(2),
                    Password = reader.GetString(3)
                } : null;
            }
        }
    }
}
```
Объяснение:
- UserRepository инкапсулирует логику работы с таблицами Account и User.
- Create — добавляет запись в БД.
- GetById — возвращает пользователя по ID.

---

#### 3. Обработка ошибок
C#


// DatabaseException.cs
```
public class DatabaseException : Exception
{
    public DatabaseException(string message, Exception inner) 
        : base(message, inner) { }
}
```
// Пример использования:
```
try
{
    // Вызов метода репозитория
}
catch (PostgresException ex) when (ex.SqlState == "23505")
{
    throw new DatabaseException("Дубликат данных: email уже существует.", ex);
}
catch (NpgsqlException ex)
{
    throw new DatabaseException("Ошибка подключения к БД", ex);
}
```
---

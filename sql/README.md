## Этап 2: Создание базы данных

На этом этапе была спроектирована и создана база данных для приложения.

### Требования этапа:

*   **Проектирование структуры базы данных:** Разработана схема БД с более чем 7 таблицами и связями типов один к одному, один ко многим и многие ко многим. Определены первичные и внешние ключи.
*   **Создание базы данных:** Выбрана СУБД (PostgreSQL, согласно SQL скрипту) и написан SQL скрипт для создания таблиц и связей.
*   **Интеграция с проектом:** Настроено подключение к БД в C# приложении (логика подключения будет в коде).
*   **Формат сдачи:** SQL скрипт добавлен в репозиторий. Структура базы данных описана в этом файле.

### Структура базы данных:

В базе данных реализовано не менее 7 таблиц с отношениями:
- Один-к-одному
- Один-ко-многим
- Многие-ко-многим

Таблицы и связи (согласно UML-диаграмме):

1.  **Platform**: id (PK), name. Связь: один ко многим с Game.
2.  **Account**: id (PK), username, email. Связь: один к одному с User.
3.  **User**: id (PK), account_id (FK), password. Связи: один к одному с Account и EpicAccount, один ко многим с GameSession, многие ко многим с User (через FriendLink) и Achievement (через UserAchievement).
4.  **EpicAccount**: id (PK), name, email, user_id (FK, unique). Связь: один к одному с User.
5.  **Game**: id (PK), title, genre, platformid (FK). Связи: один ко многим с GameSession и Achievement.
6.  **GameSession**: id (PK), userid (FK), gameid (FK), StartTime, durationMinutes. Связь: многие ко одному с User и Game.
7.  **Achievement**: id (PK), gameid (FK), name, description. Связь: многие ко многим с User через UserAchievement.
8.  **UserAchievement**: Составной PK (userid, achievementid), unlockedAt. Связь: многие ко многим между User и Achievement.
9.  **FriendLink**: Составной PK (Userid, friendid). Связь: многие ко многим между User и User.

### SQL скрипт БД:

```sql
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
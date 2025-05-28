using System;
using GameHubConsole.Services;
using GameHubConsole.Models;

namespace GameHubConsole
{
    class Program
    {
        private static readonly UserService _userService = new UserService();
        private static readonly GameService _gameService = new GameService();
        private static readonly GameSessionService _sessionService = new GameSessionService();
        private static readonly FriendService _friendService = new FriendService();

        static void Main()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\n=== Game Hub ===");
                    Console.WriteLine("1. Добавить пользователя");
                    Console.WriteLine("2. Просмотреть все игры");
                    Console.WriteLine("3. Добавить игровую сессию");
                    Console.WriteLine("4. Просмотреть друзей пользователя");
                    Console.WriteLine("5. Добавить друга");
                    Console.WriteLine("6. Просмотреть игровые сессии пользователя");
                    Console.WriteLine("7. Выход");
                    Console.Write("> ");

                    var input = Console.ReadLine();
                    if (!int.TryParse(input, out int choice) || choice < 1 || choice > 7)
                    {
                        Console.WriteLine("Ошибка: введите число от 1 до 7");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            AddUser();
                            break;
                        case 2:
                            ViewGames();
                            break;
                        case 3:
                            AddGameSession();
                            break;
                        case 4:
                            ViewFriends();
                            break;
                        case 5:
                            AddFriend();
                            break;
                        case 6:
                            ViewUserSessions();
                            break;
                        case 7:
                            return;
                        default:
                            Console.WriteLine("Неизвестная команда");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        static void AddUser()
        {
            Console.Write("Введите username: ");
            var username = Console.ReadLine()!;
            Console.Write("Введите email: ");
            var email = Console.ReadLine()!;
            Console.Write("Введите пароль: ");
            var password = Console.ReadLine()!;

            _userService.AddUser(username, email, password);
            Console.WriteLine("Пользователь создан!");
        }

        static void ViewGames()
        {
            var games = _gameService.GetAllGames();
            Console.WriteLine("\nСписок игр:");
            foreach (var game in games)
            {
                Console.WriteLine($"[{game.Id}] {game.Title} ({game.Genre})");
            }
        }

        static void AddGameSession()
        {
            Console.Write("ID пользователя: ");
            var userId = int.Parse(Console.ReadLine()!);
            Console.Write("ID игры: ");
            var gameId = int.Parse(Console.ReadLine()!);
            Console.Write("Дата начала (ГГГГ-ММ-ДД ЧЧ:ММ): ");
            var startTime = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Длительность (минуты): ");
            var duration = int.Parse(Console.ReadLine()!);

            _sessionService.AddGameSession(userId, gameId, startTime, duration);
            Console.WriteLine("Игровая сессия добавлена!");
        }

        static void ViewFriends()
        {
            Console.Write("ID пользователя: ");
            var userId = int.Parse(Console.ReadLine()!);

            var friends = _friendService.GetUserFriends(userId);
            Console.WriteLine("\nДрузья пользователя:");
            foreach (var friend in friends)
            {
                Console.WriteLine($"[{friend.Id}] {friend.Username}");
            }
        }

        static void AddFriend()
        {
            Console.Write("ID пользователя: ");
            var userId = int.Parse(Console.ReadLine()!);
            Console.Write("ID друга: ");
            var friendId = int.Parse(Console.ReadLine()!);

            _friendService.AddFriend(userId, friendId);
            Console.WriteLine("Друг добавлен!");
        }

        static void ViewUserSessions()
        {
            Console.Write("ID пользователя: ");
            var userId = int.Parse(Console.ReadLine()!);

            var sessions = _sessionService.GetUserSessions(userId);
            Console.WriteLine("\nИгровые сессии пользователя:");
            foreach (var session in sessions)
            {
                Console.WriteLine($"Игра: {session.GameTitle}, Начало: {session.StartTime}, Длительность: {session.DurationMinutes} мин.");
            }
        }
    }
}

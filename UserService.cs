using System;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Регистрация нового пользователя
    public bool RegisterUser(string username, string email, string password)
    {
        // Проверка на существование пользователя с таким email или username
        // Пока пропущено для простоты, предполагая уникальность на уровне БД

        var newUser = new User
        {
            Username = username,
            Email = email,
            Password = password // В реальном приложении пароль должен быть хеширован
        };

        try
        {
            _userRepository.Create(newUser);
            Console.WriteLine("Пользователь успешно зарегистрирован.");
            return true;
        }
        catch (DatabaseException ex)
        {
            Console.WriteLine($"Ошибка регистрации: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка при регистрации: {ex.Message}");
            return false;
        }
    }

    // Вход пользователя
    public User LoginUser(string email, string password)
    {
        // В реальном приложении здесь должна быть логика аутентификации
        // (поиск пользователя по email и проверка хеша пароля)

        // Пока упрощено: ищем пользователя по email (не реализовано в репозитории пока)
        // и просто проверяем совпадение пароля.

        // Для демонстрации, временно используем GetById (нужно будет изменить)
        // В реальной ситуации потребуется метод GetByEmail в UserRepository
        // Для текущей реализации, просто возвращаем пользователя, если пароль совпадает (неправильно для продакшена)

        // Примечание: UserRepository.GetById сейчас ищет по id, а не email. 
        // Этот метод нужно будет доработать или создать новый в UserRepository.

        Console.WriteLine("Функция входа пока не полностью реализована.");
        return null; // Временно

        // Пример того, как это могло бы выглядеть (требует доработки UserRepository):
        /*
        try
        {
            var user = _userRepository.GetByEmail(email);
            if (user != null && user.Password == password) // Проверка хеша пароля в реальном приложении
            {
                Console.WriteLine($"Вход выполнен успешно, {user.Username}!");
                return user;
            }
            else
            {
                Console.WriteLine("Неверный email или пароль.");
                return null;
            }
        }
        catch (DatabaseException ex)
        {
            Console.WriteLine($"Ошибка входа: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка при входе: {ex.Message}");
            return null;
        }
        */
    }

    // Добавление друга
    public bool AddFriend(User currentUser, User friendUser)
    {
        // Логика добавления связи дружбы в FriendLink таблице
        // Требует реализации FriendLinkRepository и соответствующих методов

        Console.WriteLine("Функция добавления в друзья пока не реализована.");
        return false;
    }
} 
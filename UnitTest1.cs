using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Ihatedthiswork.Tests
{
    // Класс пользователя
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }

    // Контекст базы данных
    public class DBforISGameContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DBforISGameContext(DbContextOptions<DBforISGameContext> options) : base(options) { }
    }

    // Сервис для работы с пользователями
    public class UserService
    {
        private readonly DBforISGameContext _dbContext;

        public UserService(DBforISGameContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _dbContext.Users.AnyAsync(u => u.UserId == userId);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }

    public class UserServiceTests
    {
        // Вспомогательный метод для создания контекста In-Memory
        private DBforISGameContext CreateInMemoryContext(List<User> users)
        {
            var options = new DbContextOptionsBuilder<DBforISGameContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;

            var context = new DBforISGameContext(options);
            context.Users.AddRange(users);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task UserExistsAsync_ReturnsTrue_WhenUserExists()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Name = "John Doe" },
                new User { UserId = 2, Name = "Jane Smith" }
            };
            var context = CreateInMemoryContext(users);
            var service = new UserService(context);

            // Act
            var result = await service.UserExistsAsync(1);

            // Assert
            Assert.True(result);  // Используем Assert.True вместо Assert.IsTrue
        }

        [Fact]
        public async Task UserExistsAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Name = "John Doe" },
                new User { UserId = 2, Name = "Jane Smith" }
            };
            var context = CreateInMemoryContext(users);
            var service = new UserService(context);

            // Act
            var result = await service.UserExistsAsync(3);

            // Assert
            Assert.False(result);  // Используем Assert.False вместо Assert.IsFalse
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Name = "John Doe" },
                new User { UserId = 2, Name = "Jane Smith" }
            };
            var context = CreateInMemoryContext(users);
            var service = new UserService(context);

            // Act
            var result = await service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);  // Используем Assert.NotNull для проверки на null
            Assert.Equal(1, result.UserId);  // Проверка на UserId
            Assert.Equal("John Doe", result.Name);  // Проверка на Name
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Name = "John Doe" },
                new User { UserId = 2, Name = "Jane Smith" }
            };
            var context = CreateInMemoryContext(users);
            var service = new UserService(context);

            // Act
            var result = await service.GetUserByIdAsync(3);

            // Assert
            Assert.Null(result);  // Проверка на null, если пользователь не найден
        }
    }
}

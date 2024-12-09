using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UI.Models;
using static System.Net.WebRequestMethods;
namespace UI.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly ILogger<WorkoutService> _logger;
        private readonly HttpClient _httpClient;

        public WorkoutService(ILogger<WorkoutService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }
        public async Task<WorkoutProgram> GetWorkoutProgramByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7047/api/WorkoutProgram/{id}");
            // Читаем контент ответа
            var content = await response.Content.ReadAsStringAsync();
            // Проверяем успешность ответа
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Неуспешный ответ: {StatusCode}", response.StatusCode);
                throw new Exception($"Ошибка при получении деталей программы тренировок: {response.StatusCode}");
            }
            try
            {
                // Настраиваем параметры для десериализации
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Игнорируем регистр названий свойств
                };

                // Выполняем десериализацию
                var workoutProgram = JsonSerializer.Deserialize<WorkoutProgram>(content, options);

                // Проверяем, что результат не null
                if (workoutProgram == null)
                {
                    _logger.LogWarning("Ответ API успешно десериализован, но данных нет.");
                    throw new Exception("Ответ API успешно десериализован, но данных нет.");
                }

                _logger.LogInformation("Десериализация успешна. Id программы тренировок: {Count}", workoutProgram.Id);
                return workoutProgram;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Ошибка десериализации ответа");
                throw new Exception("Не удалось десериализовать данные из API.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Общая ошибка при обработке ответа API");
                throw;
            }
        }
        public async Task<List<WorkoutProgram>> GetWorkoutProgramsAsync()
        {
            //_logger.LogInformation("Метод GetWorkoutProgramsAsync вызван");

            //var response = await _httpClient.GetAsync("https://localhost:7047/api/WorkoutProgram");

            //var content = await response.Content.ReadAsStringAsync();
            //_logger.LogInformation("Ответ API: {Content}", content);

            //if (!response.IsSuccessStatusCode)
            //{
            //    _logger.LogWarning("Неуспешный ответ: {StatusCode}", response.StatusCode);
            //    throw new Exception($"Ошибка при получении программ тренировок: {response.StatusCode}");
            //}

            //try
            //{
            //    return JsonSerializer.Deserialize<List<WorkoutProgram>>(content);
            //}
            //catch (JsonException ex)
            //{
            //    _logger.LogError(ex, "Ошибка десериализации ответа");
            //    throw new Exception("Не удалось десериализовать данные из API.");
            //}

            _logger.LogInformation("Метод GetWorkoutProgramsAsync вызван");

            var response = await _httpClient.GetAsync("https://localhost:7047/api/WorkoutProgram");

            // Читаем контент ответа
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Ответ API: {Content}", content);

            // Проверяем успешность ответа
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Неуспешный ответ: {StatusCode}", response.StatusCode);
                throw new Exception($"Ошибка при получении программ тренировок: {response.StatusCode}");
            }

            try
            {
                // Настраиваем параметры для десериализации
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Игнорируем регистр названий свойств
                };

                // Выполняем десериализацию
                var workoutPrograms = JsonSerializer.Deserialize<List<WorkoutProgram>>(content, options);

                // Проверяем, что результат не null
                if (workoutPrograms == null)
                {
                    _logger.LogWarning("Ответ API успешно десериализован, но данных нет.");
                    throw new Exception("Ответ API успешно десериализован, но данных нет.");
                }

                _logger.LogInformation("Десериализация успешна. Найдено программ тренировок: {Count}", workoutPrograms.Count);
                return workoutPrograms;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Ошибка десериализации ответа");
                throw new Exception("Не удалось десериализовать данные из API.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Общая ошибка при обработке ответа API");
                throw;
            }



        }
        public async Task<WorkoutProgram> CreateWorkoutProgramAsync(WorkoutProgram workoutProgram)
        {
            _logger.LogInformation("Создание программы тренировок: {Name}", workoutProgram.Name);

            try
            {
                // Логгирование отправляемого объекта
                _logger.LogDebug("Отправляемые данные: {WorkoutProgram}", JsonSerializer.Serialize(workoutProgram));

                // Отправка POST-запроса
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7047/api/WorkoutProgram", workoutProgram);

                // Логгирование ответа
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Ответ API: {Content}", content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Ошибка при создании программы тренировок: {StatusCode}", response.StatusCode);
                    throw new Exception($"Ошибка при создании программы тренировок: {response.StatusCode}, Ответ: {content}");
                }

                // Десериализация ответа
                var createdProgram = JsonSerializer.Deserialize<WorkoutProgram>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (createdProgram == null)
                {
                    _logger.LogWarning("Ответ API успешно получен, но данных нет.");
                    throw new Exception("Ответ API успешно получен, но данных нет.");
                }

                _logger.LogInformation("Программа создана успешно: {Id}", createdProgram.Id);
                return createdProgram;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании программы тренировок");
                throw;
            }
        }
    }
}

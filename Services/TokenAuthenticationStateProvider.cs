using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Buffers.Text;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace UI.Services
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private const string TokenKey = "authToken";

        public TokenAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        //        Получает токен из локального хранилища.
        //Если токен отсутствует, возвращает "пустое" состояние аутентификации(не аутентифицирован).
        //Если токен найден, устанавливает его в заголовки Authorization для всех последующих запросов и создает ClaimsPrincipal на основе токена.
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(TokenKey);

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }
        //        Логика:

        //Сохраняет токен в локальном хранилище.
        //Создает ClaimsPrincipal на основе токена.
        //Уведомляет Blazor о смене состояния с помощью NotifyAuthenticationStateChanged.
        public async Task MarkUserAsAuthenticated(string token)
        {
            await _localStorage.SetItemAsync(TokenKey, token);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
        }
        //        Логика:

        //Удаляет токен из локального хранилища.
        //Сбрасывает состояние аутентификации на "не аутентифицирован".
        //Уведомляет об изменении состояния.
        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.RemoveItemAsync(TokenKey);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }
        //        Логика:

        //Разделяет JWT на части(заголовок, полезная нагрузка, подпись).
        //Декодирует полезную нагрузку(Base64).
        //Десериализует JSON в словарь key-value.
        //Преобразует каждый элемент в Claim.
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            Console.WriteLine($"Received token: {jwt}");

            if (string.IsNullOrEmpty(jwt))
            {
                throw new ArgumentException("JWT token is null or empty");
            }

            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            // Нормализация строки Base64 (замена символов и добавление недостающих символов "=")
            payload = payload.Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            var jsonBytes = Convert.FromBase64String(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }
    }
}

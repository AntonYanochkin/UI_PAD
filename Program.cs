using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UI;
using UI.Services;
using UI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//for jwt - „то делает?
//ƒобавл€ет основные службы авторизации в Blazor WebAssembly. Ёто позвол€ет использовать политики авторизации, проверку ролей и другие механизмы, которые обеспечивают контроль доступа к различным част€м приложени€.
builder.Services.AddAuthorizationCore();


//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IWorkoutService, WorkoutService>();

//–егистрирует HttpClient как сервис с областью действи€ Scoped (это значит, что один и тот же экземпл€р клиента будет использоватьс€ в рамках одного HTTP-запроса).
//BaseAddress указывает базовый URL дл€ всех запросов. «десь ты должен заменить "https://your-api-url.com" на реальный адрес API, с которым будет работать приложение.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7081") });

//TokenAuthenticationStateProvider Ч это пользовательский класс (ты должен его реализовать), который будет управл€ть состо€нием аутентификации пользовател€. ќн обычно отвечает за:
//—охранение и извлечение JWT-токенов.
//”ведомление Blazor о том, когда пользователь вошел или вышел из системы.
//AuthenticationStateProvider Ч это стандартный интерфейс в Blazor, который используетс€ дл€ предоставлени€ текущего состо€ни€ аутентификации в приложении. ћы регистрируем наш TokenAuthenticationStateProvider как его реализацию
builder.Services.AddScoped<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();

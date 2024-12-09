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

//for jwt - ��� ������?
//��������� �������� ������ ����������� � Blazor WebAssembly. ��� ��������� ������������ �������� �����������, �������� ����� � ������ ���������, ������� ������������ �������� ������� � ��������� ������ ����������.
builder.Services.AddAuthorizationCore();


//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IWorkoutService, WorkoutService>();

//������������ HttpClient ��� ������ � �������� �������� Scoped (��� ������, ��� ���� � ��� �� ��������� ������� ����� �������������� � ������ ������ HTTP-�������).
//BaseAddress ��������� ������� URL ��� ���� ��������. ����� �� ������ �������� "https://your-api-url.com" �� �������� ����� API, � ������� ����� �������� ����������.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7081") });

//TokenAuthenticationStateProvider � ��� ���������������� ����� (�� ������ ��� �����������), ������� ����� ��������� ���������� �������������� ������������. �� ������ �������� ��:
//���������� � ���������� JWT-�������.
//����������� Blazor � ���, ����� ������������ ����� ��� ����� �� �������.
//AuthenticationStateProvider � ��� ����������� ��������� � Blazor, ������� ������������ ��� �������������� �������� ��������� �������������� � ����������. �� ������������ ��� TokenAuthenticationStateProvider ��� ��� ����������
builder.Services.AddScoped<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();


# 📅 Calendar App (.NET MAUI)

Este é um aplicativo de calendário desenvolvido com .NET MAUI, usando o padrão MVVM e banco de dados SQLite para armazenar dados localmente.

---

## ✅ Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio 2022 ou 2025 com a carga de trabalho **.NET MAUI**
- Android SDK e ADB instalados (geralmente vêm com o VS)
- Dispositivo ou emulador Android

---

## 🚀 Como executar o projeto

1. Clone o repositório:
   ```bash
   git clone https://github.com/usuario/calendar-app.git
   cd calendar-app
   ```

2. Execute no emulador ou dispositivo:
   ```bash
   dotnet build -t:Run -f net8.0-android
   ```

Ou abra o projeto no **Visual Studio**, selecione `Android Emulator` e clique em **Run** (▶️).

---

## 📦 Como gerar o APK

1. No terminal, execute:
   ```bash
   dotnet publish -f net8.0-android -c Release
   ```

2. O APK será gerado em:
   ```
   bin/Release/net8.0-android/publish/
   ```

3. Para instalar no seu dispositivo Android:
   - Conecte via USB e rode:
     ```bash
     adb install bin/Release/net8.0-android/publish/nome-do-apk.apk
     ```
   - Ou copie o arquivo manualmente para o celular e instale.

---

## ✨ Funcionalidades

- Exibição dos dias do mês em grade
- Navegação entre meses
- Seleção de dias com persistência local
- Splash screen personalizada com imagem

---

## 📁 Estrutura do projeto

```
├── ViewModels/         # Lógica de exibição (MVVM)
├── Models/             # Estruturas de dados (ex: DayItem)
├── Views/              # Telas (CalendarPage, SplashScreen)
├── Resources/          # Fontes, imagens, estilos
├── Platforms/          # Configurações específicas de cada plataforma
├── App.xaml / App.xaml.cs
└── Calendar.csproj     # Arquivo do projeto MAUI
```

--------------

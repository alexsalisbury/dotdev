# dotdev

Personal site built with Blazor WebAssembly, Azure Functions, and Azure Static Web Apps. Features an interactive hex grid homepage and a real-time status dashboard rendered as a periodic table.

## Projects

| Project | Description |
|---|---|
| **Client** | Blazor WebAssembly frontend |
| **Api** | Azure Functions API — serves element and server data |
| **HubFunctions** | Azure Functions SignalR hub — broadcasts real-time status updates |
| **Core** | Shared C# class library — domain models for HexPath and Element/Status |
| **Core.Tests** | xUnit + bunit test project (255 tests) |

## Features

- **Hex grid** (`/`) — Interactive honeycomb navigation with JS-driven console animation
- **Status dashboard** (`/status`) — Periodic table of servers with live status via SignalR

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Azure Functions Core Tools](https://www.npmjs.com/package/azure-functions-core-tools)
- [Azure Static Web Apps CLI](https://www.npmjs.com/package/@azure/static-web-apps-cli)

### Running locally

1. Copy `Api/local.settings.example.json` to `Api/local.settings.json` and fill in your values.

2. Start the Blazor client:
    ```bash
    cd Client
    dotnet run
    ```

3. Start the API:
    ```bash
    cd Api
    func start
    ```

4. Start the SignalR hub functions:
    ```bash
    cd HubFunctions
    func start
    ```

5. Optionally, proxy everything through the SWA CLI:
    ```bash
    swa start http://localhost:5000 --api-location http://localhost:7071
    ```
    Then open `http://localhost:4280`.

### Visual Studio 2022

Open the solution, right-click → **Configure Startup Projects**, set **Api**, **Client**, and **HubFunctions** to **Start**, then press **F5**.

## Tests

```bash
dotnet test Core.Tests/Core.Tests.csproj
```

Tests use [bunit](https://bunit.dev) for Blazor component testing and [Moq](https://github.com/moq/moq4) for mocking. Coverage includes Core domain models, Honeycomb grid logic, all Client components, and page-level rendering.

## Deploy

Deployed to [Azure Static Web Apps](https://docs.microsoft.com/azure/static-web-apps) via GitHub Actions.

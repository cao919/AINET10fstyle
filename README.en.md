# AINET10fstyle

AINET10fstyle is an enterprise AI assistant system built with a layered architecture, featuring four core functional modules: AI Agent interaction, Knowledge Hub (RAG), Data Analysis (NL2SQL), and Tool Invocation (MCP). It supports natural language queries, cross-system operations, and visualization report generation. The system uses ASP.NET Core as the backend framework, Semantic Kernel as the AI framework, Qdrant as the vector database, and supports private deployment. Security is ensured through permission control, and the system adopts a containerized cloud-native deployment solution, providing an intelligent interaction layer for enterprise existing systems.

## Core Functional Modules

### 🤖 AI Gateway (AiGateway)
Intelligent conversation management system supporting multi-turn sessions, intent routing, and context aggregation.
- **Session Management**: Support for multi-turn conversations with session history and context maintenance
- **Intent Recognition**: Automatic user intent identification and routing to appropriate processing modules
- **Conversation Templates**: Support for predefined conversation templates for rapid scenario-specific interaction building
- **Multi-Model Support**: Configuration and management of multiple language models

### 📚 Knowledge Hub (RAG)
Retrieval-Augmented Generation based knowledge management system supporting document upload, vectorized storage, and intelligent retrieval.
- **Knowledge Base Management**: Create and manage multiple knowledge bases
- **Document Processing**: Support for uploading and parsing various document formats including PDF and TXT
- **Vectorized Storage**: Use Qdrant vector database for document vector storage
- **Intelligent Retrieval**: Document retrieval based on semantic similarity

### 📊 Data Analysis (NL2SQL)
Natural Language to SQL data analysis module supporting multiple data sources and visualization reports.
- **Natural Language Queries**: Convert natural language to SQL queries
- **Multi-Data Source Support**: Support for various database types (SQL Server, PostgreSQL, MySQL, etc.)
- **Data Visualization**: Automatic generation of charts and data tables
- **SQL Security Protection**: Built-in SQL injection protection mechanisms

### 🔧 Tool Invocation (MCP)
Model Context Protocol service supporting external tool integration and cross-system operations.
- **MCP Server Management**: Configuration and management of MCP server connections
- **Tool Discovery**: Automatic discovery and utilization of tools provided by MCP servers
- **Cross-System Integration**: Integration with enterprise existing systems

## Project Structure

```
src/
├── Zilor.AICopilot.AppHost              # Application host module (.NET Aspire)
├── Zilor.AICopilot.HttpApi              # HTTP API interface layer
├── Zilor.AICopilot.MigrationWorkApp     # Database migration and seed data initialization
│
├── Zilor.AICopilot.Core.AiGateway       # AI Gateway domain models
├── Zilor.AICopilot.Core.DataAnalysis    # Data Analysis domain models
├── Zilor.AICopilot.Core.McpServer       # MCP Server domain models
├── Zilor.AICopilot.Core.Rag             # RAG Knowledge Hub domain models
│
├── Zilor.AICopilot.AiGatewayService     # AI Gateway service layer
│   ├── Agents/                          # AI agent definitions
│   ├── Commands/                        # CQRS commands
│   ├── Queries/                         # CQRS queries
│   ├── Workflows/                       # Workflow definitions
│   └── Plugins/                         # AI plugins
├── Zilor.AICopilot.RagService           # RAG service layer
├── Zilor.AICopilot.RagWorker            # RAG background processing service
├── Zilor.AICopilot.DataAnalysisService  # Data Analysis service layer
├── Zilor.AICopilot.McpService           # MCP service layer
├── Zilor.AICopilot.IdentityService      # Identity authentication service
│
├── Zilor.AICopilot.EntityFrameworkCore  # Data access layer (EF Core)
├── Zilor.AICopilot.Dapper               # Dapper data access
├── Zilor.AICopilot.Embedding            # Vector embedding service
├── Zilor.AICopilot.Visualization        # Data visualization components
├── Zilor.AICopilot.AgentPlugin          # Agent plugin framework
├── Zilor.AICopilot.Infrastructure       # Infrastructure layer
├── Zilor.AICopilot.EventBus             # Event bus
│
├── Zilor.AICopilot.SharedKernel         # Shared kernel (domain base classes, repository interfaces, etc.)
├── Zilor.AICopilot.Services.Common      # Common services
├── Zilor.AICopilot.Services.Contracts   # Service contracts
└── Zilor.AICopilot.ServiceDefaults      # Service default configurations
```

## Technology Stack

### Backend Framework
- **.NET 8** - Primary development framework
- **ASP.NET Core Minimal API** - Web API framework
- **Entity Framework Core** - ORM data access
- **MediatR** - CQRS command and query processing

### AI Framework
- **Semantic Kernel** - Microsoft AI development framework
- **SK Agents** - Agent framework
- **SK Plugins** - Plugin system

### Data Storage
- **PostgreSQL** - Primary database
- **Qdrant** - Vector database
- **Redis** - Caching (optional)

### Infrastructure
- **.NET Aspire** - Cloud-native application orchestration
- **Docker** - Containerized deployment
- **OpenTelemetry** - Distributed tracing and monitoring
- **JWT** - Authentication

## Installation and Running

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/) (for running Qdrant and PostgreSQL)
- PostgreSQL database
- Qdrant vector database

### Environment Configuration

1. Clone the project
```bash
git clone <repository-url>
cd AINET10fstyle
```

2. Configure database connection strings
Edit `src/Zilor.AICopilot.HttpApi/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=AICopilot;Username=postgres;Password=your_password"
  }
}
```

3. Start infrastructure services (using Docker)
```bash
# Start PostgreSQL
docker run -d --name postgres \
  -e POSTGRES_DB=AICopilot \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=your_password \
  -p 5432:5432 postgres:15

# Start Qdrant
docker run -d --name qdrant \
  -p 6333:6333 \
  -v qdrant_storage:/qdrant/storage \
  qdrant/qdrant
```

### Build the Project

```bash
dotnet restore
dotnet build
```

### Run Database Migrations

Navigate to the `src/Zilor.AICopilot.MigrationWorkApp` directory and run:

```bash
cd src/Zilor.AICopilot.MigrationWorkApp
dotnet run
```

This will automatically execute database migrations and seed initial data (including default language models, conversation templates, etc.).

### Start Services

#### Option 1: Using .NET Aspire (Recommended)
```bash
cd src/Zilor.AICopilot.AppHost
dotnet run
```
This will start all services and automatically configure service discovery.

#### Option 2: Start Separately
```bash
# Start Web API
cd src/Zilor.AICopilot.HttpApi
dotnet run

# Start RAG Worker (in another terminal)
cd src/Zilor.AICopilot.RagWorker
dotnet run
```

## API Usage Examples

### User Registration
```bash
POST /api/identity/register
Content-Type: application/json

{
  "username": "admin",
  "password": "password123"
}
```

### Create Session
```bash
POST /api/aigateway/sessions
Content-Type: application/json
Authorization: Bearer <token>

{
  "title": "New Session"
}
```

### Send Message
```bash
POST /api/aigateway/sessions/{sessionId}/messages
Content-Type: application/json
Authorization: Bearer <token>

{
  "content": "Hello, please introduce this system"
}
```

### Create Knowledge Base
```bash
POST /api/rag/knowledgebases
Content-Type: application/json
Authorization: Bearer <token>

{
  "name": "Product Documentation",
  "description": "Company product related documents"
}
```

### Upload Document
```bash
POST /api/rag/documents
Content-Type: multipart/form-data
Authorization: Bearer <token>

{
  "knowledgeBaseId": "<knowledge-base-id>",
  "file": <file>
}
```

## Architecture Design

### Layered Architecture
```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer (HttpApi)              │
├─────────────────────────────────────────────────────────────┤
│ AI Gateway │ RAG Service │ Data Analysis │ MCP Service │ Identity │
├─────────────────────────────────────────────────────────────┤
│                    Application Service Layer                 │
├─────────────────────────────────────────────────────────────┤
│ Core.AiGateway │ Core.Rag │ Core.DataAnalysis │ Core.McpServer │
├─────────────────────────────────────────────────────────────┤
│                    Domain Layer                              │
├─────────────────────────────────────────────────────────────┤
│ EntityFrameworkCore │ Dapper │ Embedding │ Infrastructure    │
├─────────────────────────────────────────────────────────────┤
│                    Infrastructure Layer                      │
└─────────────────────────────────────────────────────────────┘
```

### AI Workflow
```
User Input
    ↓
Intent Recognition (Intent Routing)
    ↓
┌─────────────┬─────────────┬─────────────┐
↓             ↓             ↓             ↓
Knowledge   Data Analysis Tool Invocation Chat
Retrieval   (NL2SQL)      (MCP)
(RAG)
↓             ↓             ↓             ↓
Context Aggregation ←──────────────────────┘
    ↓
Final Response Generation
    ↓
User
```

## Development Guide

### Add New AI Plugin

1. Create a plugin class inheriting from `AgentPluginBase`:
```csharp
public class MyPlugin : AgentPluginBase
{
    [KernelFunction("function_name")]
    [Description("Function description")]
    public async Task<string> MyFunctionAsync(string param)
    {
        // Implementation logic
    }
}
```

2. Register the plugin:
```csharp
services.AddAgentPlugin<MyPlugin>();
```

### Add New MCP Server

Configure MCP server information in the database, and the system will automatically discover and connect:
```sql
INSERT INTO "McpServerInfos" ("Id", "Name", "TransportType", "Endpoint", "IsActive")
VALUES (gen_random_uuid(), 'MyServer', 'Sse', 'http://localhost:3000/sse', true);
```

## Contribution Guidelines

Contributions and documentation improvements are welcome. Please follow these steps:

1. Fork the project
2. Create a new branch (`git checkout -b feature/new-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin feature/new-feature`)
5. Create a Pull Request

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

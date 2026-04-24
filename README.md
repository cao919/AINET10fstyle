# AINET10fstyle

企业AI助理系统采用分层架构设计，包含智能体交互、知识中枢（RAG）、数据分析（NL2SQL）和工具调用（MCP）四大核心功能模块，支持自然语言查询、跨系统操作和可视化报表生成。技术选型上选用ASP.NET Core后端框架、Semantic Kernel AI框架、Qdrant向量数据库，并支持私有化部署。系统通过权限控制确保安全性，采用容器化云原生部署方案，为企业提供覆盖现有系统的智能化交互层。

## 核心功能模块

### 🤖 AI 网关 (AiGateway)
智能对话管理系统，支持多轮会话、意图路由和上下文聚合。
- **会话管理**：支持多轮对话，维护会话历史和上下文
- **意图识别**：自动识别用户意图并路由到相应处理模块
- **对话模板**：支持预定义对话模板，快速构建特定场景交互
- **多模型支持**：支持配置和管理多种语言模型

### 📚 知识中枢 (RAG)
基于检索增强生成的知识管理系统，支持文档上传、向量化存储和智能检索。
- **知识库管理**：创建和管理多个知识库
- **文档处理**：支持 PDF、TXT 等多种格式文档上传和解析
- **向量化存储**：使用 Qdrant 向量数据库存储文档向量
- **智能检索**：基于语义相似度的文档检索

### 📊 数据分析 (NL2SQL)
自然语言转 SQL 的数据分析模块，支持多数据源和可视化报表。
- **自然语言查询**：将自然语言转换为 SQL 查询
- **多数据源支持**：支持多种数据库类型（SQL Server、PostgreSQL、MySQL 等）
- **数据可视化**：自动生成图表和数据表格
- **SQL 安全防护**：内置 SQL 注入防护机制

### 🔧 工具调用 (MCP)
模型上下文协议服务，支持外部工具集成和跨系统操作。
- **MCP 服务器管理**：配置和管理 MCP 服务器连接
- **工具发现**：自动发现和使用 MCP 服务器提供的工具
- **跨系统集成**：与企业现有系统进行集成

## 项目结构

```
src/
├── Zilor.AICopilot.AppHost              # 应用程序主宿主模块（.NET Aspire）
├── Zilor.AICopilot.HttpApi              # HTTP API 接口层
├── Zilor.AICopilot.MigrationWorkApp     # 数据库迁移与种子数据初始化
│
├── Zilor.AICopilot.Core.AiGateway       # AI 网关领域模型
├── Zilor.AICopilot.Core.DataAnalysis    # 数据分析领域模型
├── Zilor.AICopilot.Core.McpServer       # MCP 服务器领域模型
├── Zilor.AICopilot.Core.Rag             # RAG 知识中枢领域模型
│
├── Zilor.AICopilot.AiGatewayService     # AI 网关服务层
│   ├── Agents/                          # AI 智能体定义
│   ├── Commands/                        # CQRS 命令
│   ├── Queries/                         # CQRS 查询
│   ├── Workflows/                       # 工作流定义
│   └── Plugins/                         # AI 插件
├── Zilor.AICopilot.RagService           # RAG 服务层
├── Zilor.AICopilot.RagWorker            # RAG 后台处理服务
├── Zilor.AICopilot.DataAnalysisService  # 数据分析服务层
├── Zilor.AICopilot.McpService           # MCP 服务层
├── Zilor.AICopilot.IdentityService      # 身份认证服务
│
├── Zilor.AICopilot.EntityFrameworkCore  # 数据访问层（EF Core）
├── Zilor.AICopilot.Dapper               # Dapper 数据访问
├── Zilor.AICopilot.Embedding            # 向量嵌入服务
├── Zilor.AICopilot.Visualization        # 数据可视化组件
├── Zilor.AICopilot.AgentPlugin          # 智能体插件框架
├── Zilor.AICopilot.Infrastructure       # 基础设施层
├── Zilor.AICopilot.EventBus             # 事件总线
│
├── Zilor.AICopilot.SharedKernel         # 共享核心（领域基类、仓储接口等）
├── Zilor.AICopilot.Services.Common      # 公共服务
├── Zilor.AICopilot.Services.Contracts   # 服务契约
└── Zilor.AICopilot.ServiceDefaults      # 服务默认配置
```

## 技术栈

### 后端框架
- **.NET 8** - 主要开发框架
- **ASP.NET Core Minimal API** - Web API 框架
- **Entity Framework Core** - ORM 数据访问
- **MediatR** - CQRS 命令和查询处理

### AI 框架
- **Semantic Kernel** - 微软 AI 开发框架
- **SK Agents** - 智能体框架
- **SK Plugins** - 插件系统

### 数据存储
- **PostgreSQL** - 主数据库
- **Qdrant** - 向量数据库
- **Redis** - 缓存（可选）

### 基础设施
- **.NET Aspire** - 云原生应用编排
- **Docker** - 容器化部署
- **OpenTelemetry** - 分布式追踪和监控
- **JWT** - 身份认证

## 安装与运行

### 前提条件

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/)（用于运行 Qdrant 和 PostgreSQL）
- PostgreSQL 数据库
- Qdrant 向量数据库

### 环境配置

1. 克隆项目
```bash
git clone <repository-url>
cd AINET10fstyle
```

2. 配置数据库连接字符串
编辑 `src/Zilor.AICopilot.HttpApi/appsettings.Development.json`：
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=AICopilot;Username=postgres;Password=your_password"
  }
}
```

3. 启动基础设施服务（使用 Docker）
```bash
# 启动 PostgreSQL
docker run -d --name postgres \
  -e POSTGRES_DB=AICopilot \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=your_password \
  -p 5432:5432 postgres:15

# 启动 Qdrant
docker run -d --name qdrant \
  -p 6333:6333 \
  -v qdrant_storage:/qdrant/storage \
  qdrant/qdrant
```

### 构建项目

```bash
dotnet restore
dotnet build
```

### 运行数据库迁移

进入 `src/Zilor.AICopilot.MigrationWorkApp` 目录并运行：

```bash
cd src/Zilor.AICopilot.MigrationWorkApp
dotnet run
```

这将自动执行数据库迁移并初始化种子数据（包括默认语言模型、对话模板等）。

### 启动服务

#### 方式一：使用 .NET Aspire（推荐）
```bash
cd src/Zilor.AICopilot.AppHost
dotnet run
```
这将启动所有服务并自动配置服务发现。

#### 方式二：单独启动
```bash
# 启动 Web API
cd src/Zilor.AICopilot.HttpApi
dotnet run

# 启动 RAG Worker（在另一个终端）
cd src/Zilor.AICopilot.RagWorker
dotnet run
```

## API 使用示例

### 用户注册
```bash
POST /api/identity/register
Content-Type: application/json

{
  "username": "admin",
  "password": "password123"
}
```

### 创建会话
```bash
POST /api/aigateway/sessions
Content-Type: application/json
Authorization: Bearer <token>

{
  "title": "新会话"
}
```

### 发送消息
```bash
POST /api/aigateway/sessions/{sessionId}/messages
Content-Type: application/json
Authorization: Bearer <token>

{
  "content": "你好，请介绍一下这个系统"
}
```

### 创建知识库
```bash
POST /api/rag/knowledgebases
Content-Type: application/json
Authorization: Bearer <token>

{
  "name": "产品文档",
  "description": "公司产品相关文档"
}
```

### 上传文档
```bash
POST /api/rag/documents
Content-Type: multipart/form-data
Authorization: Bearer <token>

{
  "knowledgeBaseId": "<knowledge-base-id>",
  "file": <file>
}
```

## 架构设计

### 分层架构
```
┌─────────────────────────────────────────────────────────────┐
│                        表现层 (HttpApi)                      │
├─────────────────────────────────────────────────────────────┤
│  AI网关服务  │  RAG服务  │  数据分析服务  │  MCP服务  │ 身份服务 │
├─────────────────────────────────────────────────────────────┤
│                      应用服务层                              │
├─────────────────────────────────────────────────────────────┤
│  Core.AiGateway │ Core.Rag │ Core.DataAnalysis │ Core.McpServer │
├─────────────────────────────────────────────────────────────┤
│                      领域层                                  │
├─────────────────────────────────────────────────────────────┤
│  EntityFrameworkCore │ Dapper │ Embedding │ Infrastructure   │
├─────────────────────────────────────────────────────────────┤
│                      基础设施层                              │
└─────────────────────────────────────────────────────────────┘
```

### AI 工作流
```
用户输入
    ↓
意图识别 (Intent Routing)
    ↓
┌─────────────┬─────────────┬─────────────┐
↓             ↓             ↓             ↓
知识检索    数据分析      工具调用      闲聊对话
(RAG)       (NL2SQL)      (MCP)
↓             ↓             ↓             ↓
上下文聚合 ←───────────────┴─────────────┘
    ↓
最终响应生成
    ↓
用户
```

## 开发指南

### 添加新的 AI 插件

1. 创建插件类继承 `AgentPluginBase`：
```csharp
public class MyPlugin : AgentPluginBase
{
    [KernelFunction("function_name")]
    [Description("函数描述")]
    public async Task<string> MyFunctionAsync(string param)
    {
        // 实现逻辑
    }
}
```

2. 注册插件：
```csharp
services.AddAgentPlugin<MyPlugin>();
```

### 添加新的 MCP 服务器

在数据库中配置 MCP 服务器信息，系统会自动发现并连接：
```sql
INSERT INTO "McpServerInfos" ("Id", "Name", "TransportType", "Endpoint", "IsActive")
VALUES (gen_random_uuid(), 'MyServer', 'Sse', 'http://localhost:3000/sse', true);
```

## 贡献指南

欢迎贡献代码和改进文档。请遵循以下步骤：

1. Fork 项目
2. 创建新分支 (`git checkout -b feature/new-feature`)
3. 提交更改 (`git commit -am 'Add some feature'`)
4. 推送分支 (`git push origin feature/new-feature`)
5. 创建 Pull Request

## 许可证

本项目采用 MIT 许可证。详情请查看 [LICENSE](LICENSE) 文件。

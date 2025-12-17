# AINET10fstyle

企业AI助理系统采用分层架构设计，包含智能体交互、知识中枢（RAG）、数据分析（NL2SQL）和工具调用（MCP）三大核心功能模块，支持自然语言查询、跨系统操作和可视化报表生成。技术选型上选用ASP.NET Core后端框架、Semantic Kernel AI框架、Qdrant向量数据库，并支持私有化部署。系统通过权限控制确保安全性，采用容器化云原生部署方案，为企业提供覆盖现有系统的智能化交互层。

## 项目结构

- **Zilor.AICopilot.AppHost** - 应用程序的主宿主模块，用于启动服务。
- **Zilor.AICopilot.EntityFrameworkCore** - 数据访问层，包含数据库上下文、迁移脚本及依赖注入配置。
- **Zilor.AICopilot.HttpApi** - 提供 HTTP API 接口，包含控制器、模型及基础结构类。
- **Zilor.AICopilot.IdentityService** - 身份认证服务模块，处理用户创建等操作。
- **Zilor.AICopilot.MigrationWorkApp** - 数据库迁移与初始化模块，用于执行数据库迁移和种子数据填充。
- **Zilor.AICopilot.ServiceDefaults** - 提供通用服务默认配置，如健康检查、OpenTelemetry 配置等。
- **Zilor.AICopilot.SharedKernel** - 公共核心模块，包含通用接口、结果封装和消息处理。

## 功能特性

- 基于 .NET Minimal API 的轻量级服务架构。
- 支持用户注册的身份认证系统。
- 使用 Entity Framework Core 进行数据库迁移和管理。
- 支持 OpenTelemetry 进行分布式追踪。
- 提供健康检查和默认服务配置。
- 使用 MediatR 模式进行命令和查询处理。

## 安装与运行

### 前提条件

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- 数据库（支持 Entity Framework Core，如 SQLite、PostgreSQL 或 SQL Server）

### 构建项目

```bash
dotnet restore
dotnet build
```

### 运行数据库迁移

进入 `src/Zilor.AICopilot.MigrationWorkApp` 目录并运行：

```bash
dotnet run
```

这将自动执行数据库迁移并初始化种子数据。

### 启动 Web API

进入 `src/Zilor.AICopilot.HttpApi` 目录并运行：

```bash
dotnet run
```

服务将在默认端口上启动，你可以通过 `/api/identity/register` 等接口进行访问。

## 使用示例

### 用户注册

发送 POST 请求到 `/api/identity/register` 接口：

```json
{
  "username": "example",
  "password": "password"
}
```

## 贡献指南

欢迎贡献代码和改进文档。请遵循以下步骤：

1. Fork 项目
2. 创建新分支 (`git checkout -b feature/new-feature`)
3. 提交更改 (`git commit -am 'Add some feature'`)
4. 推送分支 (`git push origin feature/new-feature`)
5. 创建 Pull Request


## dev4
引入AI网关领域模型、服务、数据访问与API支持
新增AiGateway领域聚合根（会话、消息、语言模型、对话模板）及其EF Core配置，实现CQRS用例与权限注解。扩展EntityFrameworkCore支持聚合根持久化与通用仓储，增加数据库迁移。新增AiGatewayController开放相关API。完善权限校验、依赖注入与项目结构，为AI相关业务开发奠定基础。
## 许可证

本项目采用 MIT 许可证。详情请查看 [LICENSE](LICENSE) 文件。
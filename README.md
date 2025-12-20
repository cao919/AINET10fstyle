# AINET10fstyle
企业AI助理系统采用分层架构设计，包含智能体交互、知识中枢（RAG）、数据分析（NL2SQL）和工具调用（MCP）三大核心功能模块，支持自然语言查询、跨系统操作和可视化报表生成。技术选型上选用ASP.NET Core后端框架、Semantic Kernel AI框架、Qdrant向量数据库，并支持私有化部署。系统通过权限控制确保安全性，采用容器化云原生部署方案，为企业提供覆盖现有系统的智能化交互层。

该项目的代码结构表明它是一个基于 .NET 的微服务架构应用程序，主要功能包括 AI 网关服务、身份验证服务、RAG（Retrieval-Augmented Generation）服务等。该项目使用了多种现代软件开发技术，包括但不限于：

- 领域驱动设计（DDD）
- 命令查询职责分离（CQRS）使用 MediatR 模式进行命令和查询处理。
- 事件驱动架构（EDA）
- Entity Framework Core 用于数据访问
- ASP.NET Core 用于构建 Web API
- JWT 用于身份验证和授权
- 向量数据库用于知识检索
- 提供健康检查和默认服务配置。
![输入图片说明](image01.png)
![输入图片说明](image.png)

### 主要组件说明：

#### 1. **AI 网关服务 (AiGatewayService)**
   - 提供与 AI 模型交互的核心功能，如创建会话、发送用户消息、管理对话模板和语言模型。
   - 包含插件系统，允许扩展 AI 功能（如 `TimeAgentPlugin`）。
   - 使用工作流（Workflow）处理复杂的业务逻辑，如意图路由、知识检索和最终处理。

#### 2. **身份验证服务 (IdentityService)**
   - 提供用户注册、登录和角色管理功能。
   - 使用 JWT 生成身份验证令牌。

#### 3. **RAG 服务 (RagService)**
   - 提供知识库管理、文档上传和搜索功能。
   - 支持多种文档格式（如 PDF、TXT、MD 等）的解析和向量化存储。

#### 4. **数据库访问 (EntityFrameworkCore)**
   - 使用 Entity Framework Core 进行数据库操作。
   - 包含多个聚合根（如 `LanguageModel`, `ConversationTemplate`, `Session`, `KnowledgeBase` 等）。
   - 数据库（支持 Entity Framework Core，如 SQLite、PostgreSQL 或 SQL Server）
#### 5. **基础设施 (Infrastructure)**
   - 提供 JWT 生成、本地文件存储等通用功能。

#### 6. **迁移工作应用 (MigrationWorkApp)**
   - 用于数据库迁移和种子数据初始化。

#### 7. **RAG 工作者 (RagWorker)**
   - 后台服务，负责处理文档上传后的解析、分块、向量化和存储。

### 构建与运行

该项目使用 .NET 10 SDK 进行构建。要运行该项目，请确保已安装 .NET 10 SDK，并按照以下步骤操作：

服务将在默认端口上启动，你可以通过 `/api/identity/register` 等接口进行访问。

## 使用示例

### 用户注册

发送 POST 请求到 `/api/identity/register` 接口：

```json
{
  "username": " ",
  "password": " "
}
```

1. **恢复依赖项**:
   ```bash
   dotnet restore
   ```

2. **构建项目**:
   ```bash
   dotnet build
   ```

3. **运行迁移工作应用**（用于初始化数据库）:
   ```bash
   dotnet run --project src/Zilor.AICopilot.MigrationWorkApp
   ```

4. **运行主应用程序**:
   ```bash
   dotnet run --project src/Zilor.AICopilot.AppHost
   ```

### 许可证

该项目使用 MIT 许可证。有关详细信息，请参阅 `LICENSE` 文件。

## 贡献指南

欢迎贡献代码和改进文档。请遵循以下步骤：

1. Fork 项目
2. 创建新分支 (`git checkout -b feature/new-feature`)
3. 提交更改 (`git commit -am 'Add some feature'`)
4. 推送分支 (`git push origin feature/new-feature`)
5. 创建 Pull Request

### 联系方式

957801754

如果您有任何问题或建议，请联系项目维护者。
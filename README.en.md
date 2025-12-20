The code structure of this project indicates it is a .NET-based microservices application, primarily featuring AI Gateway Service, Identity Service, RAG (Retrieval-Augmented Generation) Service, and more. The project leverages multiple modern software development technologies, including but not limited to:

- Domain-Driven Design (DDD)
- Command Query Responsibility Segregation (CQRS)
- Event-Driven Architecture (EDA)
- Entity Framework Core for data access
- ASP.NET Core for building Web APIs
- JWT for authentication and authorization
- Vector databases for knowledge retrieval

### Main Component Descriptions:

#### 1. **AI Gateway Service (AiGatewayService)**
   - Provides core functionalities for interacting with AI models, such as creating sessions, sending user messages, managing conversation templates, and language models.
   - Includes a plugin system to extend AI capabilities (e.g., `TimeAgentPlugin`).
   - Uses workflows to handle complex business logic, such as intent routing, knowledge retrieval, and final processing.

#### 2. **Identity Service (IdentityService)**
   - Offers user registration, login, and role management functionalities.
   - Generates authentication tokens using JWT.

#### 3. **RAG Service (RagService)**
   - Provides knowledge base management, document upload, and search capabilities.
   - Supports parsing and vectorized storage of various document formats (e.g., PDF, TXT, MD, etc.).

#### 4. **Data Access (EntityFrameworkCore)**
   - Utilizes Entity Framework Core for database operations.
   - Includes multiple aggregate roots (e.g., `LanguageModel`, `ConversationTemplate`, `Session`, `KnowledgeBase`, etc.).

#### 5. **Infrastructure**
   - Provides common utilities such as JWT generation and local file storage.

#### 6. **Migration Work App (MigrationWorkApp)**
   - Used for database migrations and seed data initialization.

#### 7. **RAG Worker (RagWorker)**
   - A background service responsible for parsing, chunking, vectorizing, and storing documents after upload.

### Build and Run

This project is built using the .NET 10 SDK. To run the project, ensure that the .NET 10 SDK is installed, then follow these steps:

1. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

2. **Build the project**:
   ```bash
   dotnet build
   ```

3. **Run the Migration Work App** (to initialize the database):
   ```bash
   dotnet run --project src/Zilor.AICopilot.MigrationWorkApp
   ```

4. **Run the main application**:
   ```bash
   dotnet run --project src/Zilor.AICopilot.AppHost
   ```

### License

This project is licensed under the MIT License. For details, see the `LICENSE` file.

### Contribution

Contributions are welcome! Please refer to the contribution guidelines for information on how to participate in this project.

### Contact

If you have any questions or suggestions, please contact the project maintainers.
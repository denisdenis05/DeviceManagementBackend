# Device Management System - Backend

A robust and secure API for tracking mobile devices, specifications, and usage. This project follows clean architecture and SOLID principles to ensure maintainability and scalability.

### Repository Links
- **Backend**: [https://github.com/denisdenis05/DeviceManagementBackend](https://github.com/denisdenis05/DeviceManagementBackend)
- **Frontend**: [https://github.com/denisdenis05/DeviceManagementFrontend](https://github.com/denisdenis05/DeviceManagementFrontend)

---

## Technical Stack

- **Framework**: .NET 10
- **Database**: MongoDB
- **Authentication**: JWT (JSON Web Tokens)
- **AI**: Integration with LM Studio (OpenAI-compatible)
- **Testing**: xUnit, Moq, and FluentAssertions

---

## Technical Approach

The project is structured into three main layers to keep the code clean and easy to test:

1. **API Layer**: Handles HTTP requests and responses. It manages authentication through JWT and uses request/response models to keep the data transfer predictable.
2. **Business Layer**: This is where I handle the search ranking logic, AI description generation, and data mapping using Data Transfer Objects.
3. **Data Layer**: Manages the connection to MongoDB. It uses the Repository pattern to abstract the database logic, making it easier to change or mock during testing.

### Project Steps
- **Database Management**: I use MongoDB with custom scripts (`init_db.js` and `seed_data.js`) to set up the collection structure and provide demo data. These scripts are idempotent, meaning they can be run multiple times without causing issues.
- **Security**: Access is restricted to authenticated users. I implemented a secure Registration and Login flow, and restricted sensitive actions (like device assignment) to the logged-in user.
- **Custom Search Integration**: I built a scoring engine. It breaks down search queries into tokens and ranks results based on where the match was found (e.g., a match in the Name field scores higher than one in the RAM field).
- **Automated Testing**: To ensure reliability, I achieved 100% path coverage for the core business logic. Every possible logical branch in my code is verified by a unit test.

---

## AI Integration

The system includes an intelligent assistant that generates device descriptions and provides a chat interface to query the inventory.

- **Current Implementation**: I used **LM Studio** running **Qwen 3.5 2B**. The AI layer is separated as an API and can be easily swapped for another provider.
- **Retrieval-Augmented Generation (RAG)**: I implemented a RAG flow that provides the full device list as context to the LLM. This allows the assistant to answer questions about the inventory with real-time data.
- **Interactive Component Triggering**: The assistant is designed to include device IDs in a specific format (e.g., `[ID]`) when matching devices are found. This allows the frontend to automatically detect these IDs and render interactive management cards directly in the chat.
- **Compatibility**: The integration uses an **OpenAI-compatible schema**, making it easy to swap the local model for any other provider (like OpenAI or Anthropic).
- **Configuration**: You can change the API endpoint and model settings in the `appsettings.json` file.

---

## How to Run

### 1. Database Setup
Ensure MongoDB is running locally, then run the initialization scripts:
```bash
# From the project root
mongosh "mongodb://localhost:27017/database_name" DeviceManagement.Data/DbFiles/init_db.js
mongosh "mongodb://localhost:27017/database_name" DeviceManagement.Data/DbFiles/seed_data.js
```

### 2. Run the LLM
I opted for `Qwen3.5:2B` model running on `LM Studio`. It is possible to use any other model that is compatible with OpenAI API. Modify `appsettings.json` to use a different model.

### 3. Modify the `appsettings.json` accordingly
Working example, assuming local mongodb and LM Studio running on default port:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DeviceManagementDatabase": "mongodb://127.0.0.1:27017"
  },
  "MongoDbSettings": {
    "DatabaseName": "DeviceManagementDb"
  },
  "JwtSettings": {
    "Key": "SuperSecretKeyForDeviceManagementSystemThatIsVeryLong",
    "Issuer": "DeviceManagementIssuer",
    "Audience": "DeviceManagementAudience",
    "ExpiryInDays": 7
  },
  "AiSettings": {
    "BaseUrl": "http://localhost:1234/",
    "ApiKey": "",
    "ModelName": "qwen3.5-2b",
    "DescriptionGeneratorIntervalInSeconds": 30
  }
}
```

### 4. Run the API
```bash
dotnet restore
dotnet run --project DeviceManagement.API
```
The API will start at `http://localhost:5000`.

### 5. Running Tests
```bash
dotnet test
```

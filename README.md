## Todo List Application

### Overview
This is a fully functional Todo List application developed as part of the EPAM training program's capstone project. The app offers a robust platform for managing tasks, todo lists, and user authentication, built with a modern tech stack. It supports creating, editing, and tracking tasks with detailed views, leveraging two separate databases for added complexity and functionality.

### Features
- **User Authentication**: Sign In, Sign Up, Forgot Password, Reset Password
- **Task Management**: Create, edit, and delete tasks
- **List Organization**: Categorize tasks into todo lists
- **Task Tracking**: View detailed task status and updates
- **Notifications**: Alerts for overdue tasks
- **Interface**: Responsive and user-friendly design

### Tech Stack
- **Backend**: ASP.NET Core Web API
- **Frontend**: ASP.NET MVC
- **Database**: MSSQL
- **ORM**: Entity Framework Core (Code First, Fluent API)
- **Mapping**: AutoMapper
- **Authentication**: JWT Auth
- **Validation**: FluentValidation
- **Querying**: LINQ
- **Security**: Role-based Access
- **Error Handling**: Exception Handling
- **Testing**: Unit Testing (planned)

### Installation
1. Use the `git_clone.txt` command to clone the repository.
2. Navigate to the project directory:
cd ToDoList-EPAM-Capstone-Project

3. Restore dependencies:
dotnet restore

4. Update `appsettings.json` with your database connection string.
5. Apply migrations to set up the database:
dotnet ef database update

6. Run the application:
dotnet run


### Usage
- Access the app at `https://localhost:5001` (or your configured URL).
- Register a new account or log in with existing credentials.
- Create and manage tasks, organize them into lists, and monitor their status.

### Project Structure
- **Controllers**: Handle API requests and business logic.
- **Models**: Define database schemas and entities.
- **Views**: Razor views for the MVC frontend.
- **Services**: Contain business logic and helper methods.
- **Data**: Database context and migration files.

### Contributing
This project was developed as an individual capstone challenge by EPAM trainees. Originality is required, so contributions are not accepted. However, feel free to fork and adapt for personal learning.

### Acknowledgments
- Grateful to EPAM for the training and support.
- Inspired by the need for efficient task management solutions.

### Future Improvements
- Implement unit tests for full coverage.
- Enhance mobile responsiveness.
- Add advanced notification features.

### Contact
For queries, reach out via the EPAM training platform or your preferred contact method.

### Repository
- [GitHub Repository](https://github.com/Siddiqjonov/ToDoList-EPAM-Capstone-Project.git)

© 2025 - Todo List App

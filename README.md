# AbcCompany

AbcCompany is an ASP.NET 8 application designed to manage purchase orders along with their associated products and customers. The system includes functionality for registering new orders, integrates logging using Serilog, and supports unit testing with XUnit.

## Technologies Used

- **ASP.NET 8**: The main framework for API development.
- **Dapper**: Micro-ORM for interacting with the database.
- **SQL Server**: The database used for storing system information.
- **Serilog**: A logging library for managing logs.
- **XUnit**: A testing framework for unit tests.

## Features

- Register new orders with associated products and customers.
- Log system activities and errors using Serilog.
- Comprehensive unit tests for critical components.

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- An IDE such as Visual Studio or Visual Studio Code

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Dalben06/AbcCompany.git
   cd AbcCompany
   ```

2. Restore the project dependencies:
   ```bash
   dotnet restore
   ```

3. Set up the SQL Server database:
   - Update the connection string in the `appsettings.json` file.
   - execute scripts on Sqls folder:

4. Run the application:
   ```bash
   dotnet run
   ```


## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

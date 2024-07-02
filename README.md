# E-Commerce API

This project is a simple e-commerce API built with ASP.NET Core. It allows merchants to create stores, add products, and manage VAT and shipping costs. Users can add products to their cart and get the total price.

## Requirements

- .NET 5 or later
- SQL Server

## Setup

1. Clone the repository:
    ```bash
    git clone <repository-url>
    cd ECommerceAPI
    ```

2. Update the connection string in `appsettings.json` to point to your SQL Server instance.

3. Run the migrations to set up the database:
    ```bash
    dotnet ef database update
    ```

4. Run the application:
    ```bash
    dotnet run
    ```

## APIs

### User

- **Create User**
    ```
    POST /api/user
    {
        "username": "exampleuser",
        "email": "user@example.com"
    }
    ```

- **Get User Carts**
    ```
    GET /api/user/1/carts
    ```

### Merchant

- **Create Store**
    ```
   

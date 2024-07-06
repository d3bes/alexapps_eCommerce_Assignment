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
# API Documentation

## Account

### Register User
- **Endpoint:** `POST /api/account/register`
- **Description:** Registers a new user.
- **Request Body:**
    ```json
    {
        "username": "exampleuser",
        "email": "user@example.com",
        "password": "password123"
    }
    ```
- **Response:** Success or error message indicating the result of the registration attempt.

### Login User
- **Endpoint:** `POST /api/account/login`
- **Description:** Logs in a user and returns a JWT token if the credentials are valid.
- **Request Body:**
    ```json
    {
        "email": "user@example.com",
        "password": "password123"
    }
    ```
- **Response:** User details including a JWT token or an error message for invalid login attempts.

## Admin

### Get All Stores
- **Endpoint:** `GET /api/admin/all/stores`
- **Description:** Retrieves a list of all stores with their associated merchants and products.
- **Response:** List of stores.

### Create Merchant
- **Endpoint:** `POST /api/admin/CreateMerchant`
- **Description:** Registers a new merchant and associates it with an existing user.
- **Request Body:**
    ```json
    {
        "email": "merchant@example.com",
        "storeName": "Example Store",
        "isVatIncluded": false,
        "vatPercentage": 15
    }
    ```
- **Response:** Success or error message indicating the result of the merchant registration attempt.

### Add Admin
- **Endpoint:** `POST /api/admin/Add`
- **Description:** Registers a new admin user.
- **Request Body:**
    ```json
    {
        "username": "adminuser",
        "email": "admin@example.com",
        "password": "Admin_pwd_12345"
    }
    ```
- **Response:** Success or error message indicating the result of the admin registration attempt.

### Remove Store
- **Endpoint:** `DELETE /api/admin/{storeID}/Remove`
- **Description:** Deletes a store based on the provided store ID.
- **Response:** Success or error message indicating the result of the store removal attempt.

## Merchant

### Get Store
- **Endpoint:** `GET /api/merchant/Store`
- **Description:** Retrieves the details of the current merchant's store.
- **Response:** Store details.

### Add Product
- **Endpoint:** `POST /api/merchant/AddProduct`
- **Description:** Adds a new product to the merchant's store.
- **Request Body:**
    ```json
    {
        "nameEn": "Product Name",
        "nameAr": "اسم المنتج",
        "price": 100.00,
        "quantity": 10,
        "description": "Product Description"
    }
    ```
- **Response:** The details of the added product or an error message if the product could not be added.

### Get Merchant Products
- **Endpoint:** `GET /api/merchant/Products`
- **Description:** Retrieves a list of products associated with the current merchant's store.
- **Response:** List of products.

### Update Store
- **Endpoint:** `PUT /api/merchant/updateStore`
- **Description:** Updates the details of a product in the merchant's store.
- **Request Body:**
    ```json
    {
        "nameEn": "Updated Product Name",
        "nameAr": "اسم المنتج المحدث",
        "price": 120.00,
        "quantity": 15,
        "description": "Updated Product Description"
    }
    ```
- **Response:** The details of the updated product or an error message if the update could not be performed.

### Remove Product
- **Endpoint:** `DELETE /api/merchant/Product/{productID}/Remove`
- **Description:** Deletes a product based on the provided product ID.
- **Response:** Success or error message indicating the result of the product removal attempt.

### Toggle VAT Inclusion
- **Endpoint:** `POST /api/merchant/ToggelVat`
- **Description:** Toggles the VAT inclusion status for the merchant's store.
- **Response:** The updated details of the merchant including the new VAT inclusion status.

## User

### Add To Cart
- **Endpoint:** `POST /api/user/AddToCart`
- **Description:** Adds a product to the user's cart.
- **Request Body:**
    ```json
    {
        "productId": 1,
        "quantity": 2
    }
    ```
- **Response:** Success or error message indicating the result of adding the product to the cart.

### Get Cart
- **Endpoint:** `GET /api/user/Cart`
- **Description:** Retrieves the current user's cart details, including products, quantities, VAT, shipping cost, and total cost.
- **Response:** Details of the cart including products, quantities, VAT, shipping cost, and total cost.

# E-Commerce API

This project is a simple e-commerce API built with ASP.NET Core. It allows merchants to create stores, add products, and manage VAT and shipping costs. Users can add products to their cart and get the total price.

## Requirements

- .NET 8
- SQL Server

## Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/d3bes/alexapps_eCommerce_Assignment.git
   
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

## Program Cycle

### After Running Migrations
- **Default Admin Account:**
  - **Email:** `admin@example.com`
  - **Password:** `Admin_pwd_12345`

## Admin Functionalities

### Add Another Admin
- **Description:** Admins can register new administrators.

### Create Merchant by Adding Role 'Merchant' to User
- **Description:** Admins can register new merchants by assigning the 'merchant' role to a user.

### Remove Merchant
- **Description:** Admins can delete existing merchants.

## Merchant Functionalities

### Add Product with English and Arabic Names
- **Description:** Merchants can add products to their store. The program validates if the product name contains Arabic characters.

## User Functionalities

### Add Products to Cart
- **Description:** Users can add products to their cart.

### Get Total Number with VAT Calculation
- **Description:** Users can retrieve the total number of products in their cart, including VAT calculation based on merchant settings.

# API Documentation

## Account

### Register

- **Endpoint:** `POST /api/account/register`
- **Description:** Registers a new user.
- **Request Body:**
  ```json
  {
    "username": "exampleuser",
    "email": "user@example.com",
    "password": "password"
  }
  ```
- **Response:** Success message indicating the user was registered or an error message if the registration failed.

### Login

- **Endpoint:** `POST /api/account/login`
- **Description:** Logs in an existing user.
- **Request Body:**
  ```json
  {
    "email": "user@example.com",
    "password": "password"
  }
  ```
- **Response:** JWT token and user details or an error message if the login failed.

```json
{
  "email": "user@example.com",
  "userName": "user_example_467",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eOiJ.........",
  "role": ["User"]
}
```

## Admin

### Get All Stores

- **Endpoint:** `GET /api/admin/all/stores`
- **Description:** Retrieves a list of all stores.
- **Response:** List of stores.

```json
[
  {
    "id": 1,
    "storeName": "Store Name",
    "isVatIncluded": false,
    "email": ,
    "shippingCost": 45,
    "vatPercentage": 12,
    "user": {
      "id": "1697453b-61c6-473d-8ab1-a8699e6bce66",
      "fullName": " user",
      "userName": " user",
      "email": "User@example.com",
      "phoneNumber": null
    },
    "products": [
      {
        "id": 6,
        "nameEn": "product 2",
        "nameAr": "منتج 2",
        "quantity": 45,
        "descriptionEn": "test product",
        "descriptionAr": "منتج اختبار",
        "price": 30
      },
       { } ,
       { } ,
    ]
  },
  {

  }
]

```

### Register Merchant//Add store

- **Endpoint:** `POST /api/admin/CreateMerchant`
- **Description:** Registers a new merchant.
- **Request Body:**
  ```json
  {
    "storeName": "exampleStore",
    "email": "merchant@example.com",
    "isVatIncluded": true,
    "vatPercentage": 5.0
  }
  ```
- **Response:** Success message indicating the merchant was registered or an error message if the registration failed.

### Add Admin

- **Endpoint:** `POST /api/admin/Add`
- **Description:** Registers a new admin.
- **Request Body:**
  ```json
  {
    "username": "adminuser",
    "email": "admin@mail.com",
    "password": "Admin_pwd_12345"
  }
  ```
- **Response:** Success message indicating the admin was registered or an error message if the registration failed.

### Remove Store

- **Endpoint:** `DELETE /api/admin/{storeID}/Remove`
- **Description:** Deletes a specific store.
- **Request Parameters:**
  - `storeID`: The ID of the store to delete.
- **Response:** Success message or error message indicating the result of the deletion attempt.

## Merchant

### Get Store

- **Endpoint:** `GET /api/merchant/Store`
- **Description:** Retrieves the current merchant's store information.
- **Response:** Details of the store.

```json
{
    "id": 1,
    "storeName": "Store Name",
    "isVatIncluded": false,
    "email": ,
    "shippingCost": 45,
    "vatPercentage": 12,
    "user": {
      "id": "1697453b-61c6-473d-8ab1-a8699e6bce66",
      "fullName": " user",
      "userName": " user",
      "email": "User@example.com",
      "phoneNumber": null
    },
    "products": [
      {
        "id": 6,
        "nameEn": "product 2",
        "nameAr": "منتج 2",
        "quantity": 45,
        "descriptionEn": "test product",
        "descriptionAr": "منتج اختبار",
        "price": 30
      },
       { } ,
       { } ,
    ]
  }
```

### Add Product

- **Endpoint:** `POST /api/merchant/AddProduct`
- **Description:** Adds a new product to the merchant's store.
- **Request Body:**
  ```json
  {
    "nameEn": "Product Name",
    "nameAr": "اسم المنتج",
    "price": 100.0,
    "quantity": 10,
    "descriptionEn": "Product Description",
    "descriptionAr": "وصف المنتج"
    
  }
  ```
- **Response:** Details of the added product or an error message if the addition failed.

### Get Merchant Products

- **Endpoint:** `GET /api/merchant/Products`
- **Description:** Retrieves a list of products from the current merchant's store.
- **Response:** List of products.
```json
[
  {
    "id": 6,
    "nameEn": "product 2",
    "nameAr": "منتج 2",
    "quantity": 45,
    "descriptionEn": "test product",
    "descriptionAr": "منتج اختبار",
    "price": 30
  },
  {
    "id": 8,
    "nameEn": "Test product",
    "nameAr": "تجربة",
    "quantity": 10,
    "descriptionEn": "test product for remove()",
    "descriptionAr": "منتج تجربة",
    "price": 10
  }
]
```

### Update Product

- **Endpoint:** `POST /api/merchant/updateProduct`
- **Description:** Adds a new product to the merchant's store.
- **Request Body:**
  ```json
  {
    "nameEn": "Product Name",
    "nameAr": "اسم المنتج",
    "price": 100.0,
    "quantity": 10,
    "descriptionEn": "Product Description",
    "descriptionAr": "وصف المنتج"
    
  } ```
- **Response:** update product object

### Remove Product

- **Endpoint:** `DELETE /api/merchant/Product/{productID}/Remove`
- **Description:** Deletes a specific product from the merchant's store.
- **Request Parameters:**
  - `productID`: The ID of the product to delete.
- **Response:** Success message or error message indicating the result of the deletion attempt.

### Toggle VAT Inclusion

- **Endpoint:** `POST /api/merchant/ToggleVat`
- **Description:** Toggles whether VAT is included in the merchant's pricing.
- **Response:** Details of the updated store or an error message if the toggle failed.

```json
{
  "email": "merchant@example.com",
  "storeName": "Store Name",
  "shippingCost": 45,
  "isVatIncluded": true,
  "vatPercentage": 12
}
```

## User


### Get User Cart

- **Endpoint:** `GET /api/user/1/carts`
- **Description:** Retrieves the carts associated with the user ID.
- **Response:** List of carts or an error message if the carts could not be retrieved.
```json 
{
  "cart": [
    {
      "id": 4,
      "productId": 8,
      "nameEn": "Test product",
      "nameAr": "تجربة",
      "quantity": 6,
      "price": 10,
      "totalPrice": 60
    },
    {
      "id": 5,
      "productId": 9,
      "nameEn": "Arabic Prouduct",
      "nameAr": "منتج جديد",
      "quantity": 6,
      "price": 4,
      "totalPrice": 24
    }
  ],
  "vat":12.6 ,
  "shippingCost": 45,
  "cartTotal": 141.6
}
```

### Add to Cart

- **Endpoint:** `POST /api/user/AddToCart`
- **Description:** Adds a product to the user's cart.
- **Request Body:**
  ```json
  {
    "productId": 1,
    "quantity": 2
  }
  ```
- **Response:** Success message indicating the product was added to the cart or an error message if the addition failed.



### Delete Cart Item

- **Endpoint:** `DELETE /api/user/CartItem/{itemID}`
- **Description:** Deletes a specific item from the user's cart.
- **Request Parameters:**
  - `itemID`: The ID of the cart item to delete.
  - `productID`: The ID of the product to delete
- **Response:** Success message or error message indicating the result of the deletion attempt.

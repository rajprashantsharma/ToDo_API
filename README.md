# ToDo_API

## Overview

This is a RESTful API built with .NET 8 for managing a simple to-do list. It allows users to create, update, delete, and retrieve to-do items. The API is secured with Basic Authentication and includes features such as input validation, filtering, and sorting of to-do items.

## Features

- **Create a To-Do Item:**
  - `POST /todos`
  - Allows users to create a new to-do item by providing a title and optional fields like description, due date, and completion status.

- **Get All To-Do Items:**
  - `GET /todos`
  - Retrieves a list of all to-do items.

- **Get a To-Do Item by ID:**
  - `GET /todos/{id}`
  - Retrieves a specific to-do item by its unique ID.

- **Update a To-Do Item:**
  - `PUT /todos/{id}`
  - Updates the title, description, due date, and completion status of an existing to-do item by its ID.

- **Delete a To-Do Item:**
  - `DELETE /todos/{id}`
  - Deletes a to-do item by its ID.

- **Mark a To-Do Item as Complete:**
  - `PATCH /todos/{id}/complete`
  - Marks a to-do item as complete.

- **Filtering and Sorting:**
  - `GET /todos/filtered`
  - Allows filtering to-do items based on completion status and sorting by due date (ascending or descending).

## Authentication

The API uses **Basic Authentication** to secure its endpoints. You need to provide the `Authorization` header in your requests with valid credentials to access most of the endpoints. Only the `GET /todos` endpoint is publicly accessible without authentication.

- **Username**: `admin`
- **Password**: `password`

# API Endpoints

| Method | Endpoint                   | Description                          | Auth Required |
|--------|-----------------------------|--------------------------------------|---------------|
| POST   | /todos                      | Create a new to-do item              | Yes           |
| GET    | /todos                      | Get all to-do items                  | No            |
| GET    | /todos/{id}                 | Get a specific to-do item by ID      | Yes           |
| PUT    | /todos/{id}                 | Update a specific to-do item by ID   | Yes           |
| DELETE | /todos/{id}                 | Delete a specific to-do item by ID   | Yes           |
| PATCH  | /todos/{id}/complete        | Mark a to-do item as complete        | Yes           |
| GET    | /todos/filtered             | Get filtered and sorted to-do items  | Yes           |

# Data Storage
The to-do items are stored in-memory for simplicity. This means that the data is lost when the application is restarted. The storage mechanism can be easily extended to persist data to a file or a database in the future.

## Running the Project

### Clone the Repository

```bash
git clone https://github.com/rajprashantsharma/ToDo_API
cd ToDo_API
```
### Install Dependencies
Ensure that you have .NET 8 SDK installed. Restore dependencies by running:
```bash
dotnet restore
```
### Build the Project
```bash
dotnet build
```
### Run the Application
```bash
dotnet run
```

### Swagger Documentation
When the application is running, navigate to http://localhost:<port>/swagger in your browser to explore the API using Swagger UI.

### Unit Testing
Unit tests are written using xUnit and Moq. The tests cover the core functionality of the API and ensure that authentication, creation, updating, and deletion of to-do items work as expected.

## To run the tests, execute:
```bash
dotnet test
```
### Key Unit Tests:
- **Authentication**: Verifies that Basic Authentication works correctly.
- **CRUD Operations**: Verifies the creation, retrieval, updating, and deletion of to-do items.
- **Filtering and Sorting**: Ensures that filtering by completion status and sorting by due date works as expected.
- **Error Handling**: Verifies that appropriate error messages and status codes are returned for invalid requests.

### Requirements
- **.NET 8 SDK**
- **xUnit for unit testing**
 - **Moq for mocking dependencies**

### Bonus Features
 - **Marking to-do items as completed.
- ** Filtering to-do items by completion status and sorting by due date.
- ** Basic authentication for secured endpoints.
- ** Input validation for required fields like Title.

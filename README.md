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
git clone [<repository-url>](https://github.com/rajprashantsharma/ToDo_API)
cd ToDo_API


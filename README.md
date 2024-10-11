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

### Example Authorization Header

```plaintext
Authorization: Basic YWRtaW46cGFzc3dvcmQ=





### How to use the `README.md` file:
1. **Clone the repository** and place this `README.md` file in the root folder of your project.
2. **Make sure to replace `<repository-url>`** with your actual repository URL when sharing with others.
3. This file includes detailed instructions about running the project, authentication, testing, and planned future improvements.

Let me know if you need any further customization!

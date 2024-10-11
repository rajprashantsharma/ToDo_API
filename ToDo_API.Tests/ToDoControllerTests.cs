using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo_API.Commands;
using ToDo_API.Commands.ToDo_API.Commands;
using ToDo_API.Controllers;
using ToDo_API.Models;
using ToDo_API.Queries;
using ToDo_API.Responses;

namespace ToDo_API.Tests
{
    public class ToDoControllerTests
    {
        private readonly Mock<IMediator> _Mock;
        private readonly ToDoController _controller;

        public ToDoControllerTests()
        {
            _Mock = new Mock<IMediator>();
            _controller = new ToDoController(_Mock.Object);
        }
        /// <summary>
        /// //////////Test for GetAll Method://////////////
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAll_Item_With_StatusCode200()
        {
            var Todo = new List<ToDoItem>
            { new ToDoItem { Id = Guid.NewGuid(), Title = "Code Test 1", Completed = true, },
                { new ToDoItem { Id = Guid.NewGuid(), Title = "Code Test 2", Completed = false, }
            }
                 };

            var response = Response<List<ToDoItem>>.CreateSuccess(Todo, "To Do items retrieved successfully");

            _Mock.Setup(m => m.Send(It.IsAny<GetAllToDosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);
           
            var finalresult = await _controller.GetAll();
            var okResult = Assert.IsType<OkObjectResult>(finalresult);
            var returnValue = Assert.IsAssignableFrom<Response<List<ToDoItem>>>(okResult.Value);

            Assert.Equal(2, returnValue.Data.Count);
            Assert.Equal("Code Test 1", returnValue.Data[0].Title);
        }
        [Fact]
        public async Task GetAll_When_No_ItemsExist_ReturnsNotFound()
        {
            
            var response = Response<List<ToDoItem>>.Failure("To Do items not found", System.Net.HttpStatusCode.NotFound);

            _Mock.Setup(m => m.Send(It.IsAny<GetAllToDosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

           
            var result = await _controller.GetAll();

           
            Assert.IsType<NotFoundObjectResult>(result);
        }
        [Fact]
        public async Task GetAll_WhenExceptionIsThrown_ReturnsInternalServerError()
        {
           
            _Mock.Setup(m => m.Send(It.IsAny<GetAllToDosQuery>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Something went wrong"));

        
            var result = await _controller.GetAll();

      
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var returnValue = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.Equal("An error occurred while retrieving the To Do items", returnValue.Message);
        }
        ///// <summary>
        ///// //////////Test for GetById Method:://////////////
        ///// </summary>
        ///// <returns></returns>
        [Fact]
        public async Task GetById_WhenItemExists_ReturnsOk()
        {
           
            var todoItem = new ToDoItem { Id = Guid.NewGuid(), Title = "Task 1", Completed = false };
            var response = Response<ToDoItem>.CreateSuccess(todoItem, "To Do item retrieved successfully");

            _Mock.Setup(m => m.Send(It.IsAny<GetToDoByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

           
            var result = await _controller.GetById(todoItem.Id);

           
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<ToDoItem>>(okResult.Value);
            Assert.Equal(todoItem.Id, returnValue.Data.Id);
            Assert.Equal("Task 1", returnValue.Data.Title);
        }
        [Fact]
        public async Task GetById_WhenItemDoesNotExist_ReturnsNotFound()
        {
            
            var response = Response<ToDoItem>.Failure("To Do item not found", System.Net.HttpStatusCode.NotFound);

            _Mock.Setup(m => m.Send(It.IsAny<GetToDoByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

            
            var result = await _controller.GetById(Guid.NewGuid());

            
            Assert.IsType<NotFoundObjectResult>(result);
        }
        [Fact]
        public async Task GetById_WhenExceptionIsThrown_ReturnsInternalServerError()
        {
            _Mock.Setup(m => m.Send(It.IsAny<GetToDoByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Something went wrong"));

            var result = await _controller.GetById(Guid.NewGuid());

           
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var returnValue = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.Equal("An error occurred while retrieving the To Do item", returnValue.Message);
        }

        ////////
        /////Test for Create Method
        ////////
        /////
        /// <summary>
        /// Scenario 1: Successful creation of a to-do item (201 Created).
        /// </summary>
        [Fact]
        public async Task Create_WhenItemIsValid_ReturnsCreated()
        {
            
            var newToDo = new ToDoItem { Id = Guid.NewGuid(), Title = "New Task", Description="test", Completed = false };
            var command = new CreateToDoCommand(newToDo.Title, newToDo.Description, newToDo.DueDate, newToDo.Completed );
            var response = Response<ToDoItem>.CreateSuccess(newToDo, "To Do item created successfully");

            _Mock.Setup(m => m.Send(It.IsAny<CreateToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

            
            var result = await _controller.Create(command);

            
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Response<ToDoItem>>(createdAtActionResult.Value);

            Assert.Equal("New Task", returnValue.Data.Title);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(newToDo.Id, returnValue.Data.Id);
        }

        /// <summary>
        /// Scenario 2: Returns Bad Request (400) when the command is null.
        /// </summary>
        [Fact]
        public async Task Create_WhenCommandIsNull_ReturnsBadRequest()
        {
           
            var result = await _controller.Create(null);

           
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<ToDoItem>>(badRequestResult.Value);

            Assert.Equal("Title is required", returnValue.Message);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        /// <summary>
        /// Scenario 3: Returns Bad Request (400) when Title is missing in the command.
        /// </summary>
        [Fact]
        public async Task Create_WhenTitleIsMissing_ReturnsBadRequest()
        {
            
            //var command = new CreateToDoCommand { Title = "" };
            var newToDo = new ToDoItem { Id = Guid.NewGuid(), Title = "", Description = "test", Completed = false };
            var command = new CreateToDoCommand(newToDo.Title, newToDo.Description, newToDo.DueDate, newToDo.Completed);

           
            var result = await _controller.Create(command);

            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<ToDoItem>>(badRequestResult.Value);

            Assert.Equal("Title is required", returnValue.Message);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        /// <summary>
        /// Scenario 4: Returns Internal Server Error (500) when an exception occurs during creation.
        /// </summary>
        [Fact]
        public async Task Create_WhenExceptionIsThrown_ReturnsInternalServerError()
        {
            var newToDo = new ToDoItem { Id = Guid.NewGuid(), Title = "New Task", Description = "test", Completed = false };
            var command = new CreateToDoCommand(newToDo.Title, newToDo.Description, newToDo.DueDate, newToDo.Completed);

            _Mock.Setup(m => m.Send(It.IsAny<CreateToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Something went wrong"));

           
            var result = await _controller.Create(command);

           
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var returnValue = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.Equal("An error occurred while creating the To Do item", returnValue.Message);
        }

        /// <summary>
        /// Additional Scenario 5: If mediator returns a null result (unexpected behavior).
        /// </summary>
        [Fact]
        public async Task Create_WhenMediatorReturnsNull_ReturnsInternalServerError()
        {
            var newToDo = new ToDoItem { Id = Guid.NewGuid(), Title = "New Task", Description = "test", Completed = false };
            var command = new CreateToDoCommand(newToDo.Title, newToDo.Description, newToDo.DueDate, newToDo.Completed);

            _Mock.Setup(m => m.Send(It.IsAny<CreateToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Response<ToDoItem>)null); 

          
            var result = await _controller.Create(command);

           
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var returnValue = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.Equal("An error occurred while creating the To Do item", returnValue.Message);
        }

        /// <summary>
        /// Scenario 1: Successful update of a to-do item (200 OK).
        /// </summary>
        [Fact]
        public async Task Update_WhenItemIsValid_ReturnsOk()
        {
           
            var updatedToDo = new ToDoItem { Id = Guid.NewGuid(), Title = "Updated Task", Completed = true };
            var command = new UpdateToDoCommand(updatedToDo.Title, updatedToDo.Description, updatedToDo.DueDate, updatedToDo.Completed);
            var response = Response<ToDoItem>.CreateSuccess(updatedToDo, "To Do item updated successfully");

            _Mock.Setup(m => m.Send(It.IsAny<UpdateToDoCommandWrapper>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

           
            var result = await _controller.Update(updatedToDo.Id, command);

           
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<ToDoItem>>(okResult.Value);

            Assert.Equal("Updated Task", returnValue.Data.Title);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(updatedToDo.Id, returnValue.Data.Id);
        }

        /// <summary>
        /// Scenario 2: Returns Bad Request (400) when the command is null.
        /// </summary>
        [Fact]
        public async Task Update__WhenCommandIsNull_ReturnsBadRequest()
        {
            
            var result = await _controller.Update(Guid.NewGuid(), null);

            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<ToDoItem>>(badRequestResult.Value);

            Assert.Equal("Title is required", returnValue.Message);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        /// <summary>
        /// Scenario 3: Returns Bad Request (400) when Title is missing in the command.
        /// </summary>
        [Fact]
        public async Task Update_WhenTitleIsMissing_ReturnsBadRequest()
        {
            var updatedToDo = new ToDoItem { Id = Guid.NewGuid(), Title = "", Completed = true };
            var command = new UpdateToDoCommand(updatedToDo.Title, updatedToDo.Description, updatedToDo.DueDate, updatedToDo.Completed);
           
           
            var result = await _controller.Update(Guid.NewGuid(), command);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<ToDoItem>>(badRequestResult.Value);

            Assert.Equal("Title is required", returnValue.Message);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        /// <summary>
        /// Scenario 4: Returns Internal Server Error (500) when an exception occurs during update.
        /// </summary>
        [Fact]
        public async Task Update_WhenExceptionIsThrown_ReturnsInternalServerError()
        {
            var updatedToDo = new ToDoItem { Id = Guid.NewGuid(), Title = "Updated Task", Description = "Updated Description", Completed = true };
            var command = new UpdateToDoCommand(updatedToDo.Title, updatedToDo.Description, updatedToDo.DueDate, updatedToDo.Completed);

         
            _Mock.Setup(m => m.Send(It.IsAny<UpdateToDoCommandWrapper>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Something went wrong"));

           
            var result = await _controller.Update(Guid.NewGuid(), command);

            
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var returnValue = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.Equal("An error occurred while updating the To Do item", returnValue.Message);
        }

        /// <summary>
        /// Additional Scenario 5: If mediator returns a null result (unexpected behavior).
        /// </summary>
        [Fact]
        public async Task Update_ReturnsInternalServerError_WhenMediatorReturnsNull()
        {
           
            var updatedToDo = new ToDoItem { Id = Guid.NewGuid(), Title = "Updated Task", Description = "Updated Description", Completed = true };
            var command = new UpdateToDoCommand(updatedToDo.Title, updatedToDo.Description, updatedToDo.DueDate, updatedToDo.Completed);



            _Mock.Setup(m => m.Send(It.IsAny<UpdateToDoCommandWrapper>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Response<ToDoItem>)null); // Simulating unexpected null response.

           
            var result = await _controller.Update(Guid.NewGuid(), command);

           
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var returnValue = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.Equal("An error occurred while updating the To Do item", returnValue.Message);
        }
        /// <summary>
        /// Scenario 1: Successful deletion of a to-do item (200 OK).
        /// </summary>
        [Fact]
        public async Task Delete_ReturnsOk_WhenItemIsDeleted()
        {
           
            var response = Response<bool>.CreateSuccess(true, "To Do item deleted successfully");

            _Mock.Setup(m => m.Send(It.IsAny<DeleteToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

           
            var result = await _controller.Delete(Guid.NewGuid());

           
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(okResult.Value);

            Assert.True(returnValue.Data);
            Assert.Equal(200, okResult.StatusCode);
        }

        /// <summary>
        /// Scenario 2: Returns NotFound (404) when the item to delete does not exist.
        /// </summary>
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenItemDoesNotExist()
        {
           
            var response = Response<bool>.Failure("To Do item not found", System.Net.HttpStatusCode.NotFound);

            _Mock.Setup(m => m.Send(It.IsAny<DeleteToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

           
            var result = await _controller.Delete(Guid.NewGuid());

           
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(notFoundResult.Value);

            Assert.False(returnValue.Data);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("To Do item not found", returnValue.Message);
        }

        /// <summary>
        /// Scenario 3: Returns Internal Server Error (500) when an exception is thrown during deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
           
            _Mock.Setup(m => m.Send(It.IsAny<DeleteToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Something went wrong"));

           
            var result = await _controller.Delete(Guid.NewGuid());

           
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var returnValue = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.Equal("An error occurred while deleting the To Do item", returnValue.Message);
        }

        /// <summary>
        /// Scenario 4: Returns Internal Server Error (500) when mediator returns null.
        /// </summary>
        [Fact]
        public async Task Delete_ReturnsInternalServerError_WhenMediatorReturnsNull()
        {
           
            _Mock.Setup(m => m.Send(It.IsAny<DeleteToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Response<bool>)null); // Simulate unexpected null response.

           
            var result = await _controller.Delete(Guid.NewGuid());

           
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var returnValue = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.Equal("An error occurred while deleting the To Do item", returnValue.Message);
        }
        /// <summary>
        /// Scenario 1: Successfully mark a to-do item as complete (200 OK).
        /// </summary>
        [Fact]
        public async Task MarkAsComplete_ReturnsOk_WhenItemIsMarkedAsComplete()
        {
           
            var response = Response<bool>.CreateSuccess(true, "To Do item marked as complete");

            _Mock.Setup(m => m.Send(It.IsAny<CompleteToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

           
            var result = await _controller.MarkAsComplete(Guid.NewGuid());

           
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(okResult.Value);

            Assert.True(returnValue.Data);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("To Do item marked as complete", returnValue.Message);
        }

        /// <summary>
        /// Scenario 2: Returns NotFound (404) when the item to mark as complete does not exist.
        /// </summary>
        [Fact]
        public async Task MarkAsComplete_ReturnsNotFound_WhenItemDoesNotExist()
        {
           
            var response = Response<bool>.Failure("To Do item not found", System.Net.HttpStatusCode.NotFound);

            _Mock.Setup(m => m.Send(It.IsAny<CompleteToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

           
            var result = await _controller.MarkAsComplete(Guid.NewGuid());

           
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(notFoundResult.Value);

            Assert.False(returnValue.Data);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("To Do item not found", returnValue.Message);
        }

        /// <summary>
        /// Scenario 3: Returns Internal Server Error (500) when an exception is thrown during marking the item as complete.
        /// </summary>
        [Fact]
        public async Task MarkAsComplete_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
           
            _Mock.Setup(m => m.Send(It.IsAny<CompleteToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Something went wrong"));

           
            var result = await _controller.MarkAsComplete(Guid.NewGuid());

           
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var returnValue = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.Equal("An error occurred while marking the To Do item as complete", returnValue.Message);
        }

        /// <summary>
        /// Scenario 1: Successfully retrieve filtered and sorted ToDo items (200 OK).
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOk_WhenItemsAreFilteredAndSorted()
        {
           
            var filteredToDoList = new List<ToDoItem>
    {
        new ToDoItem { Id = Guid.NewGuid(), Title = "Task 1", DueDate = DateTime.Now.AddDays(1), Completed = false },
        new ToDoItem { Id = Guid.NewGuid(), Title = "Task 2", DueDate = DateTime.Now.AddDays(2), Completed = true }
    };

            
            var response = Response<List<ToDoItem>>.CreateSuccess(filteredToDoList, "Filtered ToDo items retrieved successfully");

            _Mock.Setup(m => m.Send(It.IsAny<GetFilteredToDosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

           
            var result = await _controller.GetAll(completed: true, sortBy: "dueDate", sortOrder: "asc");

           
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<List<ToDoItem>>>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(2, returnValue.Data.Count());
            Assert.Equal("Filtered ToDo items retrieved successfully", returnValue.Message);
        }


        /// <summary>
        /// Scenario 2: Returns NotFound (404) when no items match the filter criteria.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoItemsAreFound()
        {
           
            var response = Response<List<ToDoItem>>.Failure("No ToDo items found", System.Net.HttpStatusCode.NotFound);

            _Mock.Setup(m => m.Send(It.IsAny<GetFilteredToDosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

           
            var result = await _controller.GetAll(completed: false, sortBy: "dueDate", sortOrder: "asc");

           
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Response<List<ToDoItem>>>(notFoundResult.Value);

            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("No ToDo items found", returnValue.Message);
        }


        /// <summary>
        /// Scenario 3: Returns Bad Request (400) when an exception occurs during filtering and sorting.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsBadRequest_WhenExceptionIsThrown()
        {
           
            _Mock.Setup(m => m.Send(It.IsAny<GetFilteredToDosQuery>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Something went wrong"));

           
            var result = await _controller.GetAll(completed: true, sortBy: "dueDate", sortOrder: "asc");

           
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<List<ToDoItem>>>(badRequestResult.Value);

            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("An error occurred while retrieving the To Do items", returnValue.Message);
        }
    }
}







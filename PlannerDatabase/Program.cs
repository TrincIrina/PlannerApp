// See https://aka.ms/new-console-template for more information
using PlannerDatabase.Model;
using PlannerDatabase.Repository;
using PlannerDatabase.Communication;
using PlannerDatabase.Service;

//Console.WriteLine("Hello, World!");

ITodoListRepository todoList = new TodoService();
IDealRepository dealRepository = new DealService();
IItemRepository itemRepository = new ItemService();

Server server = new Server("127.0.0.1", 1024, todoList, dealRepository, itemRepository);
server.Run();

using PlannerDatabase.Communication;
using PlannerDatabase.Model;
using PlannerDatabase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PlannerDatabase.Service
{
    internal class ClientProcessor
    {
        private readonly ITodoListRepository _listRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IItemRepository _itemRepository;        
        public ClientProcessor(
            ITodoListRepository listRepository,
            IDealRepository dealRepository,
            IItemRepository itemRepository)
        {
            _listRepository = listRepository;
            _dealRepository = dealRepository;
            _itemRepository = itemRepository;            
        }
        // ProcessClient - цикл обработки одного клиента
        public void ProcessClient(Socket client)
        {
            using (ICommunicator communicator = new FixedBufferCommunicator(client))
            {
                try
                {
                    bool isExit = false;
                    // цикл обработки клиента
                    while (!isExit)
                    {
                        List<ToDoList> toDoLists = new List<ToDoList>();
                        List<Deal> deals = new List<Deal>();
                        // отправить списки дел клиенту
                        //toDoLists = _listRepository.ListAll();                        
                        //foreach (var list in toDoLists)
                        //{
                        //    communicator.Send(list.ToString());
                        //}
                        string message = communicator.Receive();
                        string title = string.Empty;
                        switch (message)
                        {
                            case "to-do lists":
                                // отправить списки дел клиенту
                                toDoLists = _listRepository.ListAll();
                                //communicator.Send(toDoLists.ToString());
                                foreach (var list in toDoLists)
                                {
                                    communicator.Send(list.ToString());
                                }
                                break;                            
                            case "add":
                                // получить название списка дел
                                title = communicator.Receive();
                                // добавить список в репозиторий
                                ToDoList todo = new ToDoList
                                {
                                    Title = title
                                };
                                _listRepository.Add(todo);
                                // отправить списки дел клиенту
                                toDoLists = _listRepository.ListAll();                                
                                foreach (var list in toDoLists)
                                {
                                    communicator.Send(list.ToString());
                                }
                                break;
                            case "delete":
                                // получить название списка дел
                                title = communicator.Receive();
                                // удалить список из репозитория
                                _listRepository.DeleteByName(title);
                                // отправить списки дел клиенту
                                toDoLists = _listRepository.ListAll();                                
                                foreach (var list in toDoLists)
                                {
                                    communicator.Send(list.ToString());
                                }
                                break;
                            case "change":
                                // получить название существующего списка дел,
                                // название списка дела для замены
                                message = communicator.Receive();
                                string[] titles = message.Split('.');
                                // редактировать список
                                _listRepository.Update(titles[0], titles[1]);
                                // отправить списки дел клиенту
                                toDoLists = _listRepository.ListAll();                                
                                foreach (var list in toDoLists)
                                {
                                    communicator.Send(list.ToString());
                                }
                                break;
                            case "exit":
                                //communicator.Send("Bye-bye");
                                isExit = true;
                                break;
                            case "to-do list":
                                // получить название списка дел
                                title = communicator.Receive();
                                // получить список дел
                                deals = _dealRepository.ListAllByToDoList(title);
                                // отправить список дел клиенту
                                foreach (var d in deals)
                                {
                                    communicator.Send(d.ToString());
                                }
                                break;
                            case "mark completed":
                                //
                                message = communicator.Receive();
                                string[] strings = message.Split('/');
                                Deal deal = _dealRepository.GetByName(strings[0]);
                                deal.IsDone = true;
                                _dealRepository.Update(deal);
                                break;
                            case "add deal":
                                message = communicator.Receive();
                                
                                break;
                            case "delete deal":
                                // получить название дела
                                message = communicator.Receive();
                                // удалить дело из списка
                                _dealRepository.DeleteByName(message);
                                // отправить список дел клиенту
                                deals = _dealRepository.ListAllByToDoList(title);
                                foreach (var d in deals)
                                {
                                    communicator.Send(d.ToString());
                                }
                                break;
                            case "edit deal":
                                message = communicator.Receive();

                                break;
                            default:
                                communicator.Send("Unknown command");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[server[clientProcessor]]> ex occurred: {ex.Message}");
                }                
            }
        }
    }        
}

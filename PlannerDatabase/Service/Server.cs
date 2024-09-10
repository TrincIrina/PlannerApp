using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PlannerDatabase.Repository;

namespace PlannerDatabase.Service
{
    // Server - реализация многопользовательского сервера
    internal class Server
    {
        private readonly Socket _serverSocket;  // сокет сервера        
        private readonly ClientProcessor _clientProcessor;  // обработчик клиентов

        // конструктор
        public Server(string serverIpStr, 
                      int serverPort, 
                      ITodoListRepository listRepository, 
                      IDealRepository dealRepository, 
                      IItemRepository itemRepository)
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIpStr), serverPort);
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(serverEndPoint);
            
            _serverSocket.Listen();
            
            _clientProcessor = new ClientProcessor(listRepository, 
                                                   dealRepository, 
                                                   itemRepository);
            Console.WriteLine($"[server]> server started on {serverIpStr}:{serverPort}");
        }

        // Run - запуск сервера (вечный цикл)
        public void Run()
        {
            while (true)
            {
                // 1. ожидание очередного подключения
                Console.WriteLine($"[server]> waiting for incoming connections");
                Socket client = _serverSocket.Accept();

                //создать ClientProcessor и отправить клиента в работу                
                string message = $"You ({client.RemoteEndPoint}) connected to server.";
                Console.WriteLine($"[server]> {message}");
                client.Send(Encoding.UTF8.GetBytes(message));
                // запустить обработку клиента в отдельном потоке
                ThreadPool.QueueUserWorkItem(_ => _clientProcessor.ProcessClient(client));

            }
        }

    }
}

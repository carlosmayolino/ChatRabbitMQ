using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_BackEnd
{
    public class Subscribe : ISubscribe
    {
        public Subscribe()
        {
            //EscutarFila();
            Inscrever("proc_arquivo");
        }

        private void Inscrever(string nomeFila)
        {
            var con = CriarConexao();
            var cn = CriarCanal(nomeFila, con);
            EscutarCanal(cn, nomeFila);
        }


        private IConnection CriarConexao(string host = "localhost")
        {
            var factory = new ConnectionFactory{ HostName = host, UserName = "guest", Password = "guest" };
            return factory.CreateConnection();
        }

        private IModel CriarCanal(string nomeFila, IConnection conexao)
        {
            var canal = conexao.CreateModel();
            canal.QueueDeclare(nomeFila, false, false, false, null);

            return canal;

        }
        private void EscutarCanal(IModel canal, string nomeFila)
        {
            var _consumer = new EventingBasicConsumer(canal);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var msg = Encoding.UTF8.GetString(body);

                SalvarNoBanco(msg);
            };
            canal.BasicConsume(queue: nomeFila, autoAck: true, consumer: _consumer);
        }

        private bool SalvarNoBanco(string dados)
        {
            Debug.WriteLine(dados);
            return true;
        }
        public void EscutarFila()

        {
            string nomeFila = "proc_arquivo";

            var factory = new ConnectionFactory { HostName = "localhost" };

            using (var con = factory.CreateConnection())
            {
                using (var channel = con.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: nomeFila,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );


                    var _consumer = new EventingBasicConsumer(channel);

                    _consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var msg = Encoding.UTF8.GetString(body);

                        //SAVAR NO BANCO
                    };

                    channel.BasicConsume(queue: nomeFila, autoAck: true, consumer: _consumer);

                }
            }
        }
    }

    public interface ISubscribe
    {
    }
}

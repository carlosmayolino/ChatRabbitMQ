using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using WepApp_Front.Models;

namespace WepApp_Front.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public void UploadArquivo()
        {
            try
            {
                var arq = Request.Form.Files.FirstOrDefault();

                if (arq != null)
                {
                    using (var read = arq.OpenReadStream())
                    {
                        byte[] dados = new byte[read.Length];
                        read.Read(dados, 0, (int)read.Length);

                        EnviarParaProcessamento(dados);

                    }


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void EnviarParaProcessamento(byte[] arquivo)
        {
            string nomeFila = "proc_arquivo";
            var factory = new ConnectionFactory { HostName = "localhost" };

            try
            {
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

                        //var msg = Encoding.UTF8.GetBytes(msg);
                        channel.BasicPublish(
                                exchange: "",
                                routingKey: nomeFila,
                                basicProperties: null,
                                body: arquivo
                                );
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Akka.Actor;
using Akka.Event;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Petabridge.Node
{
    public class PongActor : ReceiveActor
    {
        public PongActor()
        {
            var log = Context.GetLogger();

            Receive<string>(msg =>
            {
                Count++;
                if (Last != -1 && Last != Thread.CurrentThread.ManagedThreadId)
                {
                    log.Error("Not Same Thread on Actor!!!");
                }

                log.Info($"Hello World {Count} {Sender.Path}");
                Last = Thread.CurrentThread.ManagedThreadId;
                
            });
        }

        public int Last { get; private set; } = -1;
        public int Count { get; set; }

        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new PongActor());
        }


    }
}
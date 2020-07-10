using Akka.Actor;
using Akka.Event;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Petabridge.App
{
    internal class PongActor : ReceiveActor
    {
        public PongActor()
        {
            var log = Context.GetLogger();

            Receive<string>(msg =>
            {
                if (Last != -1 && Last != Thread.CurrentThread.ManagedThreadId)
                {
                    log.Error("Not Same Thread on Actor!!!");
                }

                log.Info("Hello World");
                Last = Thread.CurrentThread.ManagedThreadId;
            });
        }

        public int Last { get; private set; } = -1;

        internal static Props Props()
        {
            return Akka.Actor.Props.Create(() => new PongActor());
        }


    }
}
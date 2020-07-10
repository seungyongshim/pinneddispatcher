using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Bootstrap.Docker;
using Akka.Configuration;
using Akka.Routing;
using Petabridge.Cmd.Cluster;
using Petabridge.Cmd.Host;
using Petabridge.Cmd.Remote;
using Petabridge.Node;

namespace Petabridge.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("app.conf")).BootstrapFromDocker();
            var actorSystem = ActorSystem.Create("ClusterSystem", config);

            var pbm = PetabridgeCmd.Get(actorSystem);
            pbm.RegisterCommandPalette(ClusterCommands.Instance);
            pbm.RegisterCommandPalette(RemoteCommands.Instance);
            pbm.Start(); // begin listening for PBM management commands


            var pongActor = actorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance)
                .WithDispatcher("single-dispatcher")
                , "PongActorGroup");

            while (true)
            {
                for(int i = 0; i < 10; i++)
                    pongActor.Tell("hello");
                await Task.Delay(5000);
                actorSystem.Log.Info("Sending...");
            }

            await actorSystem.WhenTerminated;
        }
    }
   
}

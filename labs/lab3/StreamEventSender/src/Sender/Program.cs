// Based on the sample at https://github.com/Azure/azure-event-hubs/tree/master/samples/DotNet/Microsoft.Azure.EventHubs/SampleSender

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using pelazem.rndgen;
using pelazem.util;

namespace Sender
{
	class Program
	{
		private static string _event_hub_conn_string = "";

		private static volatile bool _keepRunning = true;

		static void Main(string[] args)
		{
			if (args.Length != 1)
			{
				Console.WriteLine("Please pass your Event Hub's connection string (enclosed in double quotes on the command line) for a SAS policy that allows Send.");
				Console.WriteLine("Ensure you use the connection string for your specific Event Hub, NOT the parent Event Hub Namespace.");

				return;
			}

			_event_hub_conn_string = args[0];

			Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
				e.Cancel = true;
				_keepRunning = false;
			};

			Console.WriteLine("Running. Press Ctrl-C to end.");

			Run().Wait();

			Console.WriteLine("Exiting.");
		}

		private static async Task Run()
		{
			TripMessageGenerator generator = new TripMessageGenerator();

			// Creates an EventHubsConnectionStringBuilder object from a the connection string, and sets the EntityPath.
			// Typically the connection string should have the Entity Path in it, but for the sake of this simple scenario
			// we are using the connection string from the namespace.
			var connectionStringBuilder = new EventHubsConnectionStringBuilder(_event_hub_conn_string);

			EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

			while(_keepRunning)
			{
				string message = generator.GetMessage();

				try
				{
					Console.WriteLine(message);
					Console.WriteLine();

					await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
				}
				catch (Exception ex)
				{
					Console.WriteLine();
					Console.WriteLine("ERROR! " + message);
					Console.WriteLine(ex.Message);
					Console.WriteLine(ex.StackTrace);
					Console.WriteLine();
				}

				await Task.Delay(Converter.GetInt32(RandomGenerator.Numeric.GetUniform(250, 2500)));
			}

			await eventHubClient.CloseAsync();
		}
	}
}

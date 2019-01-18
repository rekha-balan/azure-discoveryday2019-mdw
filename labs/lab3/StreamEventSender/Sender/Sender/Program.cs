// Based on the sample at https://github.com/Azure/azure-event-hubs/tree/master/samples/DotNet/Microsoft.Azure.EventHubs/SampleSender

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;

namespace Sender
{
	class Program
	{
		private const string EVENT_HUB_CONN_STRING = "PROVIDE";
		private const string EVENT_HUB_NAME = "PROVIDE";

		private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.None };

		private static volatile bool _keepRunning = true;

		static void Main(string[] args)
		{
			Console.WriteLine("Starting. Press Ctrl-C to end.");

			Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
				e.Cancel = true;
				_keepRunning = false;
			};

			Run().Wait();

			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}

		private static async Task Run()
		{
			// Creates an EventHubsConnectionStringBuilder object from a the connection string, and sets the EntityPath.
			// Typically the connection string should have the Entity Path in it, but for the sake of this simple scenario
			// we are using the connection string from the namespace.
			var connectionStringBuilder = new EventHubsConnectionStringBuilder(EVENT_HUB_CONN_STRING) { EntityPath = EVENT_HUB_NAME };

			EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

			while(_keepRunning)
			{
				string message = GetMessage();

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
				}

				await Task.Delay(50);
			}

			await eventHubClient.CloseAsync();
		}

		private static string GetMessage()
		{
			TripMessage message = new TripMessage();

			message.trip_type = 1;
			message.trip_year = DateTime.Now.Year.ToString();
			message.trip_month = string.Format("{0:MM}", DateTime.Now);
			message.taxi_type = "Yellow";
			message.vendor_id = 1;
			message.pickup_datetime = DateTime.Now.AddMinutes(-30);
			message.dropoff_datetime = message.pickup_datetime.AddMinutes(15);
			message.passenger_count = 2;
			message.trip_distance = 5;
			message.rate_code_id = 1;
			message.store_and_fwd_flag = "";
			message.pickup_location_id = 66;
			message.dropoff_location_id = 99;
			message.pickup_longitude = "77.7777";
			message.pickup_latitude = "33.3333";
			message.dropoff_longitude = "77.9999";
			message.dropoff_latitude = "33.6666";
			message.payment_type = 2;
			message.fare_amount = 13;
			message.extra = 1.5;
			message.mta_tax = 2.2;
			message.tip_amount = 3;
			message.tolls_amount = 1.75;
			message.improvement_surcharge = 0.6;
			message.ehail_fee = 0.99;

			message.customer_sentiment = 2;
			message.customer_comments = "The trip took too long. The taxi was uncomfortable: it was too cold, the seats were dirty, and the car smelled bad. The driver was a purple two-headed alien with a spitting problem, which was mildly disconcerting to say the least! Overall it was gross.";

			return JsonConvert.SerializeObject(message, _jsonSerializerSettings);
		}
	}
}

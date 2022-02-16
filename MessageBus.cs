using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Brat.Drivers {
    public class MessageBus : Driver {
        public MessageBus(string name) : base(name) {}
        public override DriverAPI GetAPI(ProcessorHost host = null) {
            MessageBusAPI result;
            result = new MessageBusAPI(this, host);
            return result;
        }
        public bool SendMessage(JObject message) {
            string event_name = InstanceName.ToUpper().Replace(" ", "_");
            Event eventObject = new Event(
                message, event_name
            );
            AddEvent(eventObject);
            return true;
        }
    }
    public partial class MessageBusAPI : DriverAPI {
        private readonly MessageBus Bus;
        public bool SendMessage(dynamic message) {
            if (message is not JObject) message = JObject.FromObject(message);
            message["orgin"] = Context.Script;
            return Bus.SendMessage(message);
        }
        public MessageBusAPI(MessageBus bus, ProcessorHost host) : base(host) {
            Bus = bus;
        }
    }
}

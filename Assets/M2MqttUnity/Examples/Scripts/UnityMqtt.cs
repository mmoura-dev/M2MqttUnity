using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

namespace M2MqttUnity.Examples
{
    public class UnityMqtt : M2MqttUnityClient
    {
        [Header("MQTT broker configuration")]
        [Tooltip("IP address or URL of the host running the broker")]
        public string brokerAddress = "localhost";
        [Tooltip("Port where the broker accepts connections")]
        public int brokerPort = 1883;
        [Tooltip("Use encrypted connection")]
        public bool isEncrypted = false;
        [Header("Connection parameters")]
        [Tooltip("Connection to the broker is delayed by the the given milliseconds")]
        public int connectionDelay = 500;
        [Tooltip("Connection timeout in milliseconds")]
        public int timeoutOnConnection = MqttSettings.MQTT_CONNECT_TIMEOUT;
        [Tooltip("Connect on startup")]
        public bool autoConnect = true;
        [Tooltip("UserName for the MQTT broker. Keep blank if no user name is required.")]
        public string mqttUserName = null;
        [Tooltip("Password for the MQTT broker. Keep blank if no password is required.")]
        public string mqttPassword = null;

        private List<string> messagesBuffer = new List<string>();


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (messagesBuffer.Count > 0)
            {
                foreach (string msg in messagesBuffer)
                {
                    ProcessMessage(msg);
                }
                messagesBuffer.Clear();
            }
        }

        private void OnDestroy()
        {
            base.Disconnect();
        }

        // Subscribe methods
        protected override void SubscribeTopics()
        {
            // client.Subscribe(new string[] { "M2MQTT_Unity/test" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            return;
        }

        protected override void UnsubscribeTopics()
        {
            // client.Unsubscribe(new string[] { "M2MQTT_Unity/test" });
            return;
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);
            StoreMessage(msg);
        }

        private void StoreMessage(string eventMsg)
        {
            messagesBuffer.Add(eventMsg);
        }

        private void ProcessMessage(string msg)
        {
            // TODO: Observer
            return;
        }

        // Connection callbacks
        protected override void OnConnecting()
        {
            base.OnConnecting();
            Debug.Log("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            Debug.Log("Connected to broker on " + brokerAddress + "\n");
        }

        public void Publish(string topic, string msg)
        {
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log($"Message \"{msg}\" published on \"{topic}\"\n");
        }
    }
}

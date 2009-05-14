﻿/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://code.google.com/p/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nmqtt
{
    /// <summary>
    /// Represents an MQTT message that contains a fixed header, variable header and message body.
    /// </summary>
    /// <remarks>
    /// Messages roughly look as follows.
    /// <code>
    /// ----------------------------
    /// | Header, 2-5 Bytes Length |
    /// ----------------------------
    /// | Variable Header          |
    /// | n Bytes Length           |
    /// ----------------------------
    /// | Message Payload          |
    /// | 256MB minus VH Size      |
    /// ----------------------------
    /// </code>
    /// </remarks>
    public class MqttMessage
    {
        /// <summary>
        /// The header of the MQTT Message. Contains metadata about the message
        /// </summary>
        public virtual MqttHeader Header { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttMessage"/> class.
        /// </summary>
        /// <remarks>
        /// Only called via the MqttMessage.Create operation during processing of an Mqtt message stream.
        /// </remarks>
        public MqttMessage()
            {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttMessage"/> class.
        /// </summary>
        /// <param name="header">The header of the message.</param>
        /// <param name="payload">The payload of the message.</param>
        public MqttMessage(MqttHeader header)
        {
            Header = header;
        }

        /// <summary>
        /// Creates a new instance of an MQTT Message based on a raw message bytes.
        /// </summary>
        /// <param name="messageBytes">The message bytes.</param>
        /// <returns></returns>
        public static MqttMessage CreateFrom(IEnumerable<byte> messageBytes)
        {
            using (MemoryStream messageStream = new MemoryStream(messageBytes.ToArray<byte>()))
            {
                return CreateFrom(messageStream);
            }
        }

        /// <summary>
        /// Creates a new instance of an MQTT Message based on a raw message stream.
        /// </summary>
        /// <param name="messageStream">The message stream.</param>
        /// <returns>An MqttMessage containing details of the message.</returns>
        public static MqttMessage CreateFrom(Stream messageStream)
        {
            MqttHeader header = new MqttHeader();

            // pass the input stream sequentially through the component deserialization(create) methods
            // to build a full MqttMessage.
            header = new MqttHeader(messageStream);

            MqttMessage message = MqttMessageFactory.GetMessage(header, messageStream);

            return message;
        }

        /// <summary>
        /// ss the message to the supplied stream.
        /// </summary>
        /// <param name="messageStream">The stream to write the message to.</param>
        public virtual void WriteTo(Stream messageStream)
        {
            Header.WriteTo(messageStream);
        }

        /// <summary>
        /// Reads a message from the supplied stream.
        /// </summary>
        /// <param name="messageStream">The message stream.</param>
        public virtual void ReadFrom(Stream messageStream)
        {
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("MQTTMessage of type ");
            sb.AppendLine(this.GetType().ToString());

            sb.Append(Header.ToString());

            return sb.ToString();
        }
    }
}

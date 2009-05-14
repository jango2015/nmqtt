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
using Xunit;
using Nmqtt;

namespace NmqttTests
{
    public class FluentMqttHeaderTests
    {
        [Fact]
        public void FluentMqttHeader_SettingDuplicate()
        {
            MqttHeader header = new MqttHeader().IsDuplicate();
            Assert.True(header.Duplicate);
        }

        [Fact]
        public void FluentMqttHeader_SettingQos()
        {
            MqttHeader header = new MqttHeader().WithQos(MqttQos.AtMostOnce);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, header.Qos);
        }

        [Fact]
        public void FluentMqttHeader_SettingMessageType()
        {
            MqttHeader header = new MqttHeader().AsType(MqttMessageType.PublishComplete);
            Assert.Equal<MqttMessageType>(MqttMessageType.PublishComplete, header.MessageType);
        }

        [Fact]
        public void FluentMqttHeader_SettingRetain()
        {
            MqttHeader header = new MqttHeader().ShouldBeRetained();
            Assert.True(header.Retain);
        }

        [Fact]
        public void FluentMqttHeader_UseAll()
        {
            MqttHeader header
                = new MqttHeader()
                    .AsType(MqttMessageType.PublishComplete)
                    .WithQos(MqttQos.AtMostOnce)
                    .IsDuplicate()
                    .ShouldBeRetained();

            Assert.Equal<MqttMessageType>(MqttMessageType.PublishComplete, header.MessageType);
            Assert.True(header.Retain);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, header.Qos);
            Assert.True(header.Duplicate);
        }

    }
}

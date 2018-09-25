using System;
using System.Collections.Generic;
using System.Text;

namespace EventHub.Core
{
    public interface IEventHandler
    {
        void On(ISender sender, IEvent @event);
    }

    public interface IEventHandler<TSender, TEvent> : IEventHandler
        where TSender : ISender
        where TEvent : IEvent
    {
        void On(TSender sender, TEvent @event);
    }

    public interface IEvent
    {
    }

    public interface ISender
    {
    }

    public interface IRecipient
    {
    }

    public interface IProducer
    {
    }

    public interface IConsumer
    {

    }
}

using NebzzClient.DTOs;
using System;

namespace NebzzClient
{
    public abstract class Handler
    {
        internal readonly MessageType TypeHandledEnum;
        internal Type TypeHandled { get; private protected set; }
        internal Handler(MessageType typeHandled)
        {
            TypeHandledEnum = typeHandled;
        }

        internal abstract void Handle(object v);
    }

    public abstract class Handler<T> : Handler
    {

        internal Handler(MessageType typeHandled) : base(typeHandled)
        {
            TypeHandled = typeof(T);
        }

        internal override void Handle(object value)
        {
            Handle((T) value);
        }

        internal abstract void Handle(T body);
    }
}
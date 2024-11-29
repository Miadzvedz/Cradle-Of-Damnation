using System;

namespace Exceptions
{
    public class ObjectNotFoundOnSceneException : Exception
    {
        public ObjectNotFoundOnSceneException(string message) : base(message) 
        {
        }
        
    }
}

using System;

namespace ImageUploader.Gateway.Exceptions
{
    public class ImageUploaderException : Exception
    {
        private readonly string _errorCode;
        private readonly string _errorMessage;

        public ImageUploaderException(string message, Exception exception) : base(message, exception)
        {
        }

        public ImageUploaderException(string message) : base(message)
        {
        }

        public ImageUploaderException(string message, string errorCode, string errorMessage) : base(message)
        {
            _errorCode = errorCode;
            _errorMessage = errorMessage;
        }
    }
}
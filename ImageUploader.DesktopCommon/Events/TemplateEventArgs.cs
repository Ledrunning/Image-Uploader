using System;

namespace ImageUploader.DesktopCommon.Events
{
    public delegate void TemplateEventHandler<T>(TemplateEventArgs<T> templateEventArgs);

    public class TemplateEventArgs<T> : EventArgs
    {
        public TemplateEventArgs(T genericObject)
        {
            GenericObject = genericObject;
        }

        public T GenericObject { get; set; }
    }
}
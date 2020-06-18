namespace Safecare.BeiaDeviceDriver
{
    internal class DeviceMessageBuffer
    {
        public string Message { get; private set; } = "";

        public void Update(string newBuffer)
        {
            Message = newBuffer.Replace("\\", ""); ;
        }
    }
}

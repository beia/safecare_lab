using System;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Managers;

namespace BeiaDeviceDriver
{
    /// <summary>
    /// Class for working with one metadata stream session
    /// TODO: Implement request for fetching metadata
    /// </summary>
    internal class BeiaDeviceDriverMetadataStreamSession : BaseBeiaDeviceDriverStreamSession
    {
        public BeiaDeviceDriverMetadataStreamSession(ISettingsManager settingsManager, BeiaDeviceDriverConnectionManager connectionManager, Guid sessionId, string deviceId, Guid streamId, int channel) :
            base(settingsManager, connectionManager, sessionId, deviceId, streamId)
        {
            // TODO: Set Channel to correct channel number
            Channel = 1;
        }

        protected override bool GetLiveFrameInternal(TimeSpan timeout, out BaseDataHeader header, out byte[] data)
        {
            data = null;
            header = null;

            // TODO: Implement request for fetching data from device

            if (data.Length == 0)
            {
                return false;
            }
            DateTime dt = DateTime.UtcNow;// TODO: If a timestamp is provided by device, use that instead
            header = new MetadataHeader()
            {
                Length = (ulong)data.Length,
                SequenceNumber = _sequence++,
                Timestamp = dt
            };
            return true;
        }
    }
}

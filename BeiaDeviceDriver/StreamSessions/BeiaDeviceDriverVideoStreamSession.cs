using System;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Managers;

namespace BeiaDeviceDriver
{
    /// <summary>
    /// Class for working with one video stream session
    /// TODO: Implement request for fetching video data
    /// </summary>
    internal class BeiaDeviceDriverVideoStreamSession : BaseBeiaDeviceDriverStreamSession
    {

        public BeiaDeviceDriverVideoStreamSession(ISettingsManager settingsManager, BeiaDeviceDriverConnectionManager connectionManager, Guid sessionId, string deviceId, Guid streamId) :
            base(settingsManager, connectionManager, sessionId, deviceId, streamId)
        {
            // TODO: Set Channel to correct channel number
            Channel = 1;
        }

        protected override bool GetLiveFrameInternal(TimeSpan timeout, out BaseDataHeader header, out byte[] data)
        {
            header = null;
            data = null;

            // TODO: Implement request for fetching data from device

            if (data == null || data.Length == 0)
            {
                return false;
            }
            DateTime dt = DateTime.UtcNow; // TODO: If a timestamp is provided by device, use that instead
            // TODO: Update to reflect actual data
            header = new VideoHeader()
            {
                CodecType = VideoCodecType.JPEG,
                Length = (ulong)data.Length,
                SequenceNumber = _sequence++,
                SyncFrame = true,
                TimestampSync = dt,
                TimestampFrame = dt
            };
            return true;
        }
    }
}

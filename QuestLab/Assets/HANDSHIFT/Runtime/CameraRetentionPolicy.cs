using System;
using System.Collections.Generic;
using System.Linq;

namespace HandShift.Quest
{
    public interface IEncryptedLocalStore
    {
        IEnumerable<StoredCameraRecord> ReadMetadata();
        void Write(StoredCameraRecord record);
        void Delete(string id);
        void DeleteAll();
    }

    [Serializable]
    public sealed class StoredCameraRecord
    {
        public string Id;
        public long CapturedUtcTicks;
        public byte[] EncryptedPayload;
    }

    /// Retention enforcement for any explicitly retained, encrypted local camera-derived data.
    /// Raw frames should not be retained by default. Implement storage with Android Keystore.
    public sealed class CameraRetentionPolicy
    {
        public static readonly TimeSpan MaxAge = TimeSpan.FromDays(7);
        private readonly IEncryptedLocalStore store;
        private readonly Func<DateTime> utcNow;

        public CameraRetentionPolicy(IEncryptedLocalStore store, Func<DateTime> utcNow = null)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            this.utcNow = utcNow ?? (() => DateTime.UtcNow);
        }

        public int PurgeExpired()
        {
            var cutoff = utcNow().Subtract(MaxAge).Ticks;
            var expired = store.ReadMetadata().Where(x => x.CapturedUtcTicks <= cutoff).ToList();
            foreach (var item in expired) store.Delete(item.Id);
            return expired.Count;
        }

        public void DeleteEverything() => store.DeleteAll();
    }
}

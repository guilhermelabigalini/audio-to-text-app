using System.IO;

namespace AudioToTextService.Core
{
    public class TempFileStream : FileStream
    {
        public TempFileStream(string path, FileMode mode, FileAccess access)
            : base(path, mode, access)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            TryToDeleteTempFile();
        }

        public override void Close()
        {
            base.Close();
            TryToDeleteTempFile();
        }

        private void TryToDeleteTempFile()
        {
            if (File.Exists(this.Name))
            {
                File.Delete(this.Name);
            }
        }
    }
}

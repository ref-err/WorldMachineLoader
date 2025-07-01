using System.IO;
using System.Text;

namespace WorldMachineLoader.Utils
{
    public class DualWritter : TextWriter
    {
        private readonly TextWriter originalConsole;
        private readonly StreamWriter fileWriter;

        public override Encoding Encoding => Encoding.UTF8;

        public DualWritter(TextWriter original, StreamWriter fileWriter)
        {
            this.originalConsole = original;
            this.fileWriter = fileWriter;
        }

        public override void WriteLine(string value)
        {
            originalConsole.WriteLine(value);
            fileWriter.WriteLine(value);
        }

        public override void Write(char value)
        {
            originalConsole.Write(value);
            originalConsole.Write(value);
        }

        public override void Flush()
        {
            originalConsole.Flush();
            fileWriter.Flush();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fileWriter.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

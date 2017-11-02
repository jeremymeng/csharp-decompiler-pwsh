using System.Management.Automation;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;

namespace CSharpDecompilerProvider
{
    internal class CSharpDecompilerDriveInfo : PSDriveInfo
    {
        CSharpDecompiler _decompiler;

        public CSharpDecompilerDriveInfo(PSDriveInfo drive, DriveParameters parameters)
            :base(drive)
        {
            _decompiler = new CSharpDecompiler(parameters.AssemblyPath, new DecompilerSettings());
        }

        public CSharpDecompiler CSharpDecompiler
        {
            get
            {
                return this._decompiler;
            }
        }
    }
}
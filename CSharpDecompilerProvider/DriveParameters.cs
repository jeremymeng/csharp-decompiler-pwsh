using System;
using System.Management.Automation;

namespace CSharpDecompilerProvider
{
    public class DriveParameters
    {
        [Parameter(Mandatory = true)]
        public string AssemblyPath { get; set; }
    }
}

using ICSharpCode.Decompiler.TypeSystem;
using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace CSharpDecompilerProvider
{
    [CmdletProvider("CSharpDecompiler", ProviderCapabilities.None)]
    public class Provider : ContainerCmdletProvider
    {
        const string GLOBAL_NAMESPACE_TEXT = "<global>";

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            var driveParameters = this.DynamicParameters as DriveParameters;
            return new CSharpDecompilerDriveInfo(drive, driveParameters);
        }

        protected override object NewDriveDynamicParameters()
        {
            return new DriveParameters();
        }

        protected override void GetItem(string path)
        {
            var node = GetNodeFromPath(path);
            if (node != null)
            {
                WriteItemObject(node, path, node.IsContainer);
            }
        }

        private BaseNode GetNodeFromPath(string path)
        {
            var drive = this.PSDriveInfo as CSharpDecompilerDriveInfo;
            var types = drive.CSharpDecompiler.TypeSystem.Compilation.MainAssembly.GetAllTypeDefinitions();
            var namespaces = types.Select(t => t.Namespace)
                .Distinct()
                .Select(n => string.IsNullOrEmpty(n) ? GLOBAL_NAMESPACE_TEXT : n);
            if (string.IsNullOrEmpty(path))
            {
                return new ModuleNode(drive.CSharpDecompiler.TypeSystem.Compilation.MainAssembly.AssemblyName, null, drive.CSharpDecompiler)
                {
                    Namespaces = namespaces.Select(n => new NamespaceNode(n, null, drive.CSharpDecompiler))
                };
            }

            var parts = path.Split('/');
            if (parts.Length == 1)
            {
                var @namespace = parts[0];

                if (namespaces.Contains(@namespace))
                {
                    var ns = new NamespaceNode(@namespace, null, drive.CSharpDecompiler)
                    {
                        Types = types
                            .Where(t => !t.FullTypeName.IsNested &&
                                (@namespace.Equals(GLOBAL_NAMESPACE_TEXT, StringComparison.Ordinal)
                                    ? string.IsNullOrEmpty(t.Namespace)
                                    : t.Namespace.Equals(@namespace, StringComparison.Ordinal)))
                            .Select(t => new TypeNode(t.Name, t, drive.CSharpDecompiler))
                        };
                    return ns;
                }
            }
            else
            {
                if (parts[0].Equals(GLOBAL_NAMESPACE_TEXT))
                {
                    parts = parts.Skip(1).ToArray();
                }
                var typeName = string.Join(".", parts);
                var type = types.SingleOrDefault(t => t.FullName.Equals(typeName, StringComparison.Ordinal));
                if (type != null)
                {
                    return new TypeNode(type.Name, type, drive.CSharpDecompiler)
                    {
                        NestedTypes = type.NestedTypes.Select(t => new TypeNode(t.Name, t, drive.CSharpDecompiler)),
                        Members = type.Members.Select(m => new MemberNode(m.Name, m, drive.CSharpDecompiler))
                    };
                }

                // is it a member?
                typeName = string.Join(".", parts.Take(parts.Length - 1));
                var memberName = parts.Last();
                type = types.SingleOrDefault(t => t.FullName.Equals(typeName, StringComparison.Ordinal));
                if (type != null)
                {
                    var member = type.Members.SingleOrDefault(m => m.Name.Equals(memberName));
                    if (member != null)
                    {
                        return new MemberNode(member.Name, member, drive.CSharpDecompiler);
                    }
                }
            }

            return null;
        }

        protected override bool ItemExists(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return true;
            }

            var drive = this.PSDriveInfo as CSharpDecompilerDriveInfo;
            var types = drive.CSharpDecompiler.TypeSystem.Compilation.MainAssembly.GetAllTypeDefinitions();
            var parts = path.Split('/');
            if (parts.Length == 1)
            {
                var namespaces = types.Select(t => t.Namespace)
                    .Distinct()
                    .Select(n => string.IsNullOrEmpty(n) ? GLOBAL_NAMESPACE_TEXT : n);

                var @namespace = parts[0];
                if (namespaces.Contains(@namespace))
                {
                    return true;
                }
            }
            else
            {
                if (parts[0].Equals(GLOBAL_NAMESPACE_TEXT))
                {
                    parts = parts.Skip(1).ToArray();
                }
                var typeName = string.Join(".", parts);
                var type = types.SingleOrDefault(t => t.FullName.Equals(typeName, StringComparison.Ordinal));
                if (type != null)
                {
                    return true;
                }
                // is it a member?
                typeName = string.Join(".", parts.Take(parts.Length - 1));
                var memberName = parts.Last();
                type = types.SingleOrDefault(t => t.FullName.Equals(typeName, StringComparison.Ordinal));
                if (type != null)
                {
                    var member = type.Members.SingleOrDefault(m => m.Name.Equals(memberName));
                    if (member != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            if (string.IsNullOrEmpty(path))
            {
                var drive = this.PSDriveInfo as CSharpDecompilerDriveInfo;
                var types = drive.CSharpDecompiler.TypeSystem.Compilation.MainAssembly.GetAllTypeDefinitions();
                var namespaces = types.Select(t => t.Namespace)
                    .Distinct()
                    .Select(n => string.IsNullOrEmpty(n) ? GLOBAL_NAMESPACE_TEXT : n);
                var node = new ModuleNode(drive.CSharpDecompiler.TypeSystem.Compilation.MainAssembly.AssemblyName, null, drive.CSharpDecompiler)
                {
                    Namespaces = namespaces.Select(n => new NamespaceNode(n, null, drive.CSharpDecompiler))
                };

                WriteItemObject(node, path, true);
                return;
            }

            base.GetChildItems(path, recurse);
        }

        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            base.GetChildNames(path, returnContainers);
        }

        protected override bool IsValidPath(string path)
        {
            return true;
        }
    }
}

Create a new drive

```
New-PSDrive -PSProvider CSharpDecompiler -Name msil -Root "" -AssemblyPath j:\temp\a.dll
```

Get the drive info

```
(Get-PSDrive msil).GetType().FullName
```

Check root path

```
Get-Item msil:/
```

Check a type

```
Get-Item msil:/A
```

Nested Type

```
Get-Item msil:/A/NestedType
```

Member

```
Get-Item msil:/A/MethodName
```





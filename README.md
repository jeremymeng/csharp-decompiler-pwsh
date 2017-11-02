Create a new drive

```
New-PSDrive -PSProvider CSharpDecompiler -Name msil -Root ""
```

Get the drive info

```
(Get-PSDrive msil).GetType().FullName
```


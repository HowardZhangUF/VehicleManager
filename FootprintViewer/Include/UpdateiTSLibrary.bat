SET SrcDir="..\..\..\iTSLibrary\iTSLibrary\bin\Debug\"
SET DstDir=".\iTSLibrary\"

SET DirList="Image" "zh-TW"
SET FileList=Geometry.dll Geometry.pdb GLCore.dll GLCore.pdb GLStyle.dll GLStyle.pdb GLUI.dll GLUI.pdb IniFiles.dll IniFiles.pdb MapReader.dll MapReader.pdb MD5Hash.dll MD5Hash.pdb SharpGL.dll SharpGL.SceneGraph.dll SharpGL.Serialization.dll SharpGL.WinForms.dll Style.ini ThreadSafety.dll ThreadSafety.pdb

FOR %%i IN (%DirList%) DO (
	IF EXIST "%DstDir%%%i" RD /Q /S "%DstDir%%%i"
)

FOR %%i IN (%FileList%) DO (
	IF EXIST "%DstDir%%%i" DEL "%DstDir%%%i"
)

FOR %%i IN (%DirList%) DO (
	XCOPY "%SrcDir%%%i" "%DstDir%%%i" /I /S /E
)

FOR %%i IN (%FileList%) DO (
	XCOPY "%SrcDir%%%i" "%DstDir%%%i"*
)

PAUSE
